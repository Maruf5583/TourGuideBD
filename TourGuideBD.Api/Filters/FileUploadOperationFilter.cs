using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TourGuideBD.Api.Filters;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasFileParam = context.MethodInfo.GetParameters()
            .Any(p => p.ParameterType == typeof(IFormFile)
                   || p.ParameterType == typeof(List<IFormFile>));

        if (!hasFileParam) return;

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = context.MethodInfo.GetParameters()
                            .Where(p => p.ParameterType == typeof(IFormFile)
                                     || p.ParameterType == typeof(List<IFormFile>))
                            .ToDictionary(
                                p => p.Name!,
                                p => p.ParameterType == typeof(List<IFormFile>)
                                    ? new OpenApiSchema
                                    {
                                        Type = "array",
                                        Items = new OpenApiSchema { Type = "string", Format = "binary" }
                                    }
                                    : new OpenApiSchema { Type = "string", Format = "binary" }
                            )
                    }
                }
            }
        };
    }
}