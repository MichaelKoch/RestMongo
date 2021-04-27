using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Query.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData;
using Microsoft.OData.UriParser;
using MongoBase.Attributes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MongoBase.Utils
{

    public static class ODataQueryHelper
    {
        private static Dictionary<Type, ODataQueryContext> _cache = new Dictionary<Type, ODataQueryContext>();
        internal static IServiceCollection services = null;
        internal static ServiceProvider serviceProvider = null;
        private static void init()
        {
            if (serviceProvider == null)
            {
                var collection = new ServiceCollection();

                collection.AddOData();
                collection.AddODataQueryFilter();
                collection.AddTransient<ODataUriResolver>();
                collection.AddTransient<ODataQueryValidator>();
                collection.AddTransient<TopQueryValidator>();
                collection.AddTransient<FilterQueryValidator>();
                collection.AddTransient<SkipQueryValidator>();
                collection.AddTransient<OrderByQueryValidator>();
                var provider = collection.BuildServiceProvider();
              
                serviceProvider = collection.BuildServiceProvider();
            }

        }
        public static ODataQueryContext GetODataQueryContext(Type type)
        {
            if (_cache.ContainsKey(type))
            {
                return _cache[type];
            }
            var model = new ODataModelBuilder();
            var entityType = model.AddEntityType(type);
            foreach (var pi in type.GetProperties())
            {
                var modelInfo = new List<IsQueryableAttribute>(pi.GetCustomAttributes(typeof(IsQueryableAttribute), true) as IsQueryableAttribute[]).LastOrDefault();
                if (modelInfo != null)
                {
                    entityType.AddProperty(pi);
                }
            }
            var retVal = new ODataQueryContext(model.GetEdmModel(), type, new Microsoft.AspNet.OData.Routing.ODataPath());
            _cache.Add(type, retVal);
            return retVal;
        }
        public static IQueryable Apply<TType>(string filter, IQueryable query) where TType : class
        {
            if (string.IsNullOrEmpty(filter))
            {
                return query;

            }
            var type = typeof(TType);
            serviceProvider = services.BuildServiceProvider();
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            //TODO --> GET RID OF THAT MOCK FUCK 
            var routeBuilder = new RouteBuilder(Mock.Of<IApplicationBuilder>(x => x.ApplicationServices == serviceProvider));
            routeBuilder.EnableDependencyInjection();
            httpContext.Request.QueryString = new QueryString("?$filter=" + filter);
            var modelBuilder = new ODataConventionModelBuilder(serviceProvider);
            modelBuilder.EntitySet<TType>(type.Name);
            var model = modelBuilder.GetEdmModel();
            var context = GetODataQueryContext(type);
            var options = new ODataQueryOptions(context, httpContext.Request);
            return options.ApplyTo(query);
        }
    }
}
