using System.Linq;

using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using MongoBase.Interfaces;
using MongoBase.Models;
using MongoBase.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoBase
{
    public static class IServiceCollectionExtentsions
    {
        public static void AddMongoBase(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddControllers();
            services.AddOData();
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }

}