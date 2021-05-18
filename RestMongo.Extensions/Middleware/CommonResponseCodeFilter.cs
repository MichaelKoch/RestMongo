using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestMongo.Extensions.Middleware
{
    internal class QuerySample : IOpenApiAny
    {
        public AnyType AnyType { get {
                return AnyType.Object;
            } 
        }

        public void Write(IOpenApiWriter writer, OpenApiSpecVersion specVersion)
        {
            //writer.WriteRaw("Michael");
        }
    }
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
      
            //operation.RequestBody = new OpenApiRequestBody();
            //operation.RequestBody.Content.Add("application/json", new OpenApiMediaType()
            //{
            //    Example = new OpenApiString("{\"test\":\"michael\"}")
            //});
            AddQuerySamples(operation, context);
            AddDefaultResponseCodes(operation, context);
            SetOperationDescription(operation, context);
        }

        private void AddQuerySamples(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.RelativePath.ToLower().EndsWith("/queries"))
            {
                var definition = operation.RequestBody.Content["application/json"];
                definition.Examples.Add("simple query", new OpenApiExample() { Summary = "simple", Value = new OpenApiString("{\"Id\":\"\"}") });
                definition.Examples.Add("$eq operator", new OpenApiExample() { Summary = "$eq operator", Value = new OpenApiString("{\"Id\":{\"$eq\":\"\"}}") });
                definition.Examples.Add("$gt operator", new OpenApiExample() { Summary = "$gt operator", Value = new OpenApiString("{\"Timestamp\":{\"$gt\":0}}") });
                definition.Examples.Add("$lt operator", new OpenApiExample() { Summary = "$lt operator", Value = new OpenApiString("{\"Timestamp\":{\"$lt\":637557223220510000}}") });
                definition.Examples.Add("$in operator", new OpenApiExample() { Summary = "$in operator", Value = new OpenApiString("{\"Id\":{\"$in\":[\"value1\",\"value2\"]}}") });
                definition.Examples.Add("MORE INFO", new OpenApiExample() { Summary = "MORE INFO", Value = new OpenApiString("{\"see\":\"https://docs.mongodb.com/manual/tutorial/query-documents/\"}") });

            }
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
                    operation.Responses.Add("400", getApiResponse(400, "BAD REQUEST", problemType));
                }
                if (!operation.Responses.ContainsKey("500"))
                {
                    operation.Responses.Add("500", getApiResponse(500, "INTERNAL SERVER ERROR", problemType));
                }
            }

        }
    }
}