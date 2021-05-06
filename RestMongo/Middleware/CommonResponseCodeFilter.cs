using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

public class AddCommonResponseTypesFilter : IOperationFilter
{

    private OpenApiResponse getApiResponse(int code, string description, OpenApiSchema schema)
    {
        var retVal = new OpenApiResponse() { Description = description };
        retVal.Content.Add("application/json", new OpenApiMediaType()
        {
            Schema = schema
        });
        return retVal;
    }
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        AddDefaultResponseCodes(operation, context);
        SetOperationDescription(operation, context);
    }
    private void SetOperationDescription(OpenApiOperation operation, OperationFilterContext context)
    {
        if (string.IsNullOrEmpty(operation.Summary))
        {
            var paramDesc = string.Join(" and ", operation.Parameters.Where(p => p.Required).Select(p => p.Name));
            var type = context.ApiDescription.RelativePath.Split("/")[0];
            if (string.IsNullOrWhiteSpace(paramDesc))
            {
                operation.Summary = $"{context.MethodInfo.Name} {type}".ToLower();
            }
            else
            {
                operation.Summary = $"{context.MethodInfo.Name} {type} by {paramDesc}".ToLower();
            }
        }
        else
        {
            operation.Summary = operation.Summary.ToLower();
        }
    }

    private void AddDefaultResponseCodes(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
        var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

        var problemType = context.SchemaRepository.Schemas["ProblemDetails"];
        if (problemType != null)
        {
            if (!operation.Responses.ContainsKey("400"))
            {
                operation.Responses.Add("400", getApiResponse(400, "BAD REQUEST", context.SchemaRepository.Schemas["ProblemDetails"]));
            }
            if (!operation.Responses.ContainsKey("500"))
            {
                operation.Responses.Add("500", getApiResponse(500, "INTERNAL SERVER ERROR", context.SchemaRepository.Schemas["ProblemDetails"]));
            }
        }

    }
}