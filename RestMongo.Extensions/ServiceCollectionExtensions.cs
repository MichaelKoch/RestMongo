using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestMongo.Data.Abstractions.Repository;
using RestMongo.Data.Abstractions.Repository.Mongo.Configuration;
using RestMongo.Data.Repository;
using RestMongo.Data.Repository.Configuration;
using RestMongo.Extensions.Middleware;
using RestMongo.Web.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestMongo.Extensions
{
    public static class ServiceCollectionExtensions
    {

        private static string makeSigular(string value)
        {
            //TODO : Dirty remove "S" at the end --> use Pluralize / Singular Services 
            //possible services available in dependency 
            //SEE --> https://docs.microsoft.com/de-de/dotnet/api/system.data.entity.design.pluralizationservices.pluralizationservice.pluralize
            //NEED decission : Use or not to use ;)
            if (value.EndsWith("s") || (value.EndsWith("S")))
            {
                value = value.Substring(0, value.Length - 1);
            }
            return value;
        }
        private static string makeFirstLetterUpperCase(string value)
        {
            return value.ToUpper().Substring(0, 1) + value.ToLower().Substring(1, value.Length - 1);
        }
      
        private static string AddCustomOperationIds(ApiDescription apiDesc)
        {

            var retVal = "";
            if (apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
            {
                var operationInfo = methodInfo.GetCustomAttribute<SwaggerOperationAttribute>();
                if (operationInfo == null || string.IsNullOrEmpty(operationInfo.OperationId))
                {
                    var methodName = methodInfo.Name;
                    var routeName = apiDesc.RelativePath.Split("/")[0];
                    routeName = makeFirstLetterUpperCase(makeSigular(routeName));
                    retVal = methodName + routeName;
                    var queryParam = apiDesc.ParameterDescriptions.Where(c => c.Source.Id != "Body" && c.IsRequired);
                    if (queryParam.Count() > 0)
                    {
                        retVal += "By";
                        foreach (var parm in queryParam)
                        {
                            var parmName = makeFirstLetterUpperCase(parm.Name);
                            retVal += parmName;
                            if (queryParam.Last() != parm)
                            {
                                retVal += "And";
                            }
                        }
                    };
                }
                else
                {
                    retVal = operationInfo.OperationId;
                }
            }
            return retVal;
        }

        public static void AddRestMongo<TContext>(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped(typeof(TContext));
            ConnectionSettings mongoSettings = new ConnectionSettings();
            Configuration.GetSection("mongo").Bind(mongoSettings);
            services.AddSingleton<IConnectionSettings>(mongoSettings);
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            services.AddResponseCompression();
            services.AddSwaggerExamplesFromAssemblyOf<Query>();
            services.AddSwaggerGen(c =>
            {
                
                c.EnableAnnotations();
               // c.OperationFilter<ExamplesOperationFilter>();
                c.CustomOperationIds(AddCustomOperationIds);
                c.ResolveConflictingActions(apiDescriptions =>
                {
                    return apiDescriptions.First();
                });
                c.OperationFilter<AddCommonResponseTypesFilter>();
            });
        }
    }
}