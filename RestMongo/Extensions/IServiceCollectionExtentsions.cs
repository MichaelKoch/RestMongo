using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
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
        public static void AddRestMongo<TContext>(this IServiceCollection services, IConfiguration Configuration)
        {





            services.AddOData();
            services.AddScoped(typeof(TContext));
            ConnectionSettings mongoSettings = new ConnectionSettings();
            Configuration.GetSection("mongo").Bind(mongoSettings);
            services.AddSingleton<IConnectionSettings>(mongoSettings);
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().ToList())
                {
                    options.OutputFormatters.Remove(outputFormatter);
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().ToList())
                {
                    options.InputFormatters.Remove(inputFormatter);
                }
            });
            services.AddResponseCompression();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.CustomOperationIds(apiDesc =>
                {
                    var retVal = "";
                    //TODO --> seperate to utils;
                    if (apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                    {
                        var operationInfo = methodInfo.GetCustomAttribute<SwaggerOperationAttribute>();
                        if (operationInfo == null || string.IsNullOrEmpty(operationInfo.OperationId))
                        {
                            retVal = methodInfo.Name + apiDesc.RelativePath.Split("/")[0];
                            var queryParam = apiDesc.ParameterDescriptions.Where(c => c.Source.Id != "Body" && !c.Name.Contains("$"));
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
                c.ResolveConflictingActions(apiDescriptions =>
                {
                    return apiDescriptions.First();
                });
            });
        }
    }
}