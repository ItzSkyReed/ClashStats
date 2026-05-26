using System.Reflection;
using Microsoft.OpenApi;
using System.Text.Json.Nodes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Swagger;

public sealed class SmartEnumSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;

        if (!IsTypeDerivedFromGenericType(type, typeof(Ardalis.SmartEnum.SmartEnum<,>))) return;
        if (schema is not OpenApiSchema mutableSchema) return;

        mutableSchema.Type = JsonSchemaType.String;

        mutableSchema.Properties?.Clear();

        var listProperty = type.GetProperty("List", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        if (listProperty?.GetValue(null) is not IEnumerable<object> enumValues) return;
        mutableSchema.Enum = new List<JsonNode>();

        foreach (var enumValue in enumValues)
        {
            var valProperty = type.GetProperty("Value");
            var val = valProperty?.GetValue(enumValue)?.ToString();

            if (val != null)
            {
                mutableSchema.Enum.Add(val);
            }
        }
    }

    private static bool IsTypeDerivedFromGenericType(Type? type, Type genericType)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return true;
            type = type.BaseType;
        }
        return false;
    }
}