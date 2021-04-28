using System.Linq;

using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using MongoBase.Interfaces;
using MongoBase.Models;
using MongoBase.Repositories;
using MongoBase.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoBase
{
    public static class IServiceCollectionExtentsions
    {
        public static void AddMongoBase<TContext>(this IServiceCollection services, IConfiguration Configuration)
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