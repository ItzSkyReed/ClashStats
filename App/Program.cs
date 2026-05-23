using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.RateLimiting;
using App.Workers;
using Application.Interfaces;
using Application.Services;
using Ardalis.SmartEnum.SystemTextJson;
using Domain.Constants;
using Infrastructure.Api;
using Infrastructure.Api.Converters;
using Infrastructure.Middlewares;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace App;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // 1. Настройка конфигурации
        builder.Configuration.AddEnvironmentVariables();

        // 2. Регистрация сервисов (Dependency Injection)
        ConfigureServices(builder);

        // 3. Сборка хоста
        using var host = builder.Build();

        await ApplyMigrations(host.Services);

        // 4. Выполнение основной логики
        await RunAppLogicAsync(host.Services);

        // Запуск хоста (если есть BackgroundServices, они начнут работать здесь)
        await host.RunAsync();
    }

    private static void ConfigureServices(HostApplicationBuilder builder)
    {
        Action<JsonSerializerOptions> configureJson = options =>
        {
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.IncludeFields = true;
            options.Converters.Add(new ClashDateTimeConverter());
            options.Converters.Add(new SmartEnumValueConverter<WarFrequency, string>());
            options.Converters.Add(new SmartEnumValueConverter<ClanWarState, string>());
            options.Converters.Add(new SmartEnumValueConverter<ClanWarLeagueState, string>());
            options.Converters.Add(new SmartEnumValueConverter<ClanRole, string>());
            options.Converters.Add(new SmartEnumValueConverter<WarPreference, string>());
        };

        builder.Services.Configure(configureJson);

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            configureJson(options.SerializerOptions);
        });
        builder.Services.AddMemoryCache();
        builder.Services.AddTransient<CachingHandler>();

        builder.Services.AddHttpClient<ClashApiRequestExecutor>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["CLASH_OF_CLANS:API_URL"]!);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", builder.Configuration["CLASH_OF_CLANS:API_USER_TOKEN"]!);
        })
        .AddHttpMessageHandler<CachingHandler>()
        .AddResilienceHandler("ClashApiResiliencePolicy", httpBuilder =>
        {
            httpBuilder.AddRateLimiter(new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
            {
                PermitLimit = 7,
                Window = TimeSpan.FromSeconds(1),
                QueueLimit = 10000,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));

            httpBuilder.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true
            });
        });

        // Настройка БД
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddScoped<IAppDbContext>(provider =>
            provider.GetRequiredService<AppDbContext>());

        // Регистрация API клиента
        builder.Services.AddScoped<IClashApiClient, ClashApiClient>();

        builder.Services.AddScoped<IClanDataSyncService, ClanDataSyncService>();

        builder.Services.AddHostedService<StatsUpdateWorker>();

        builder.Services.AddKeyedSingleton("ClanTag", "#29RJ28YLG");
    }

    private static async Task ApplyMigrations(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        try
        {
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Error while applying migrations.");

            throw;
        }
    }

    private static async Task RunAppLogicAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var api = scope.ServiceProvider.GetRequiredService<IClashApiClient>();

        try
        {
            var result = await api.GetClanMembersAsync("#29RJ28YLG");

            if (result.Data is { } clan)
            {
                Console.WriteLine($"Получен клан: {clan}");
            }
            else if (result.Error is { } error)
            {
                Console.WriteLine($"Ошибка: {error.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошло исключение: {ex.Message}");
        }
    }
}