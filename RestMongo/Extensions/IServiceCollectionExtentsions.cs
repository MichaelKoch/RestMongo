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

        }
    }

}