using System.Linq;

using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using RestMongo.Interfaces;
using RestMongo.Models;
using RestMongo.Repositories;
using RestMongo.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.AspNetCore.Builder;

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
                c.ResolveConflictingActions(apiDescriptions =>
                {
                    return apiDescriptions.First();
                });
            });
        }
    }
}