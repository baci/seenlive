using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SeenLive.Web.Filters;

public class RequireNonNullablePropertiesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        var additionalRequiredProps = model.Properties
            .Where(x => !x.Value.Nullable && !model.Required.Contains(x.Key))
            .Select(x => x.Key);
        foreach (var propKey in additionalRequiredProps)
        {
            model.Required.Add(propKey);
        }
    }
}