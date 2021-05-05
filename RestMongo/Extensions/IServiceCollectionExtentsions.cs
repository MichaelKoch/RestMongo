using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RestMongo.Interfaces;
using RestMongo.Models;
using RestMongo.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;
namespace RestMongo
{
    public static class IServiceCollectionExtentsions
    {
        private static void AddDefaultResponseCode(SwaggerGenOptions options)
        {
           
        }
        private static void AddCustomOperationIds(SwaggerGenOptions options)
        {
            options.CustomOperationIds(apiDesc =>
            {
                var retVal = "";
                if (apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                {
                    var operationInfo = methodInfo.GetCustomAttribute<SwaggerOperationAttribute>();
                    if (operationInfo == null || string.IsNullOrEmpty(operationInfo.OperationId))
                    {
                        retVal = methodInfo.Name + apiDesc.RelativePath.Split("/")[0];

                        //TODO : Dirty remove "S" at the end --> use Pluralize / Singular Services 
                        //possible services available in dependency 
                        //SEE --> https://docs.microsoft.com/de-de/dotnet/api/system.data.entity.design.pluralizationservices.pluralizationservice.pluralize
                        //NEED decission : Use or not to use ;)

                        if (retVal.EndsWith("s") || (retVal.EndsWith("S")))
                        {
                            retVal = retVal.Substring(0, retVal.Length - 2);
                        }
                        var queryParam = apiDesc.ParameterDescriptions.Where(c => c.Source.Id != "Body" && c.IsRequired);
                        if (queryParam.Count() > 0)
                        {
                            retVal += "By";
                            foreach (var parm in queryParam)
                            {
                                var parmName = parm.Name;
                                parmName = parmName[0].ToString().ToUpper() + parmName.Substring(1, parmName.Length - 1);
                                retVal += parmName;
                                if (queryParam.Last() != parm)
                                {
                                    retVal += "And";
                                }
                            }
                        };
                        return retVal;
                    }
                    else
                    {
                        return operationInfo.OperationId;
                    }
                }
                return retVal;
            });
        }

        public static void AddRestMongo<TContext>(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped(typeof(TContext));
            ConnectionSettings mongoSettings = new ConnectionSettings();
            Configuration.GetSection("mongo").Bind(mongoSettings);
            services.AddSingleton<IConnectionSettings>(mongoSettings);
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            services.AddResponseCompression();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                AddCustomOperationIds(c);
                c.ResolveConflictingActions(apiDescriptions =>
                {
                    return apiDescriptions.First();
                });
            });
        }
    }
}