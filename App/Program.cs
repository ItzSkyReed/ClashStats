using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.RateLimiting;
using App.Endpoints;
using App.Workers;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Api.ClashOfClans;
using Infrastructure.Api.ClashOfClans.ApiClient;
using Infrastructure.Api.Converters;
using Infrastructure.Middlewares;
using Infrastructure.Persistence;
using Infrastructure.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.OpenApi;
using Polly;

namespace App;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        ConfigureServices(builder);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Russian Winners Stats API",
                Version = "v1"
            });

            c.SupportNonNullableReferenceTypes();
            c.SchemaFilter<SmartEnumSchemaFilter>();
        });

        var app = builder.Build();

        app.UseSwagger();

        // Swagger UI
        app.UseSwaggerUI(c => { c.RoutePrefix = "swagger"; });

        // ReDoc
        app.UseReDoc(c =>
        {
            c.RoutePrefix = "docs";
            c.SpecUrl("/swagger/v1/swagger.json");
            c.DocumentTitle = "Russian Winners Stats API Docs";
        });

        await ApplyMigrations(app.Services);

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.MapEndpoints();

        app.MapFallbackToFile("index.html");

        await app.RunAsync();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        Action<JsonSerializerOptions> configureJson = options =>
        {
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new ClashDateTimeConverter());
        };

        builder.Services.Configure(configureJson);

        builder.Services.ConfigureHttpJsonOptions(options => { configureJson(options.SerializerOptions); });

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

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")
            );
        });

        builder.Services.AddScoped<IAppDbContext>(provider =>
            provider.GetRequiredService<AppDbContext>()
        );

        builder.Services.AddScoped<IClashApiClient, ClashApiClient>();
        builder.Services.AddScoped<IClanDataSyncService, ClanDataSyncService>();
        builder.Services.AddScoped<IWarLeagueService, WarLeagueService>();
        builder.Services.AddScoped<IPlayerActivityService, PlayerActivityService>();

        builder.Services.AddHostedService<StatsUpdateWorker>();

        var clanTag = builder.Configuration["CLASH_OF_CLANS:MAIN_CLAN_TAG"] ?? "TEMPORARY_MIGRATION_TAG";
        builder.Services.AddKeyedSingleton("ClanTag", clanTag);
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
}