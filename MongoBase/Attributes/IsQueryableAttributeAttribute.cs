using System;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Builder;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using MongoBase.Interfaces;
using Microsoft.OData.UriParser;
using Microsoft.AspNet.OData.Query.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.OData.Query;

namespace MongoBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class IsQueryableAttribute : Attribute
    {
        //TODO : Use threadsafe caching solution
        private static Dictionary<Type, ODataQueryContext> _cache = new Dictionary<Type, ODataQueryContext>();

        //public static ODataQueryContext GetODataQueryOptions(Type type)
        //{

        //    var collection = new ServiceCollection();
        //    collection.AddTransient<ODataUriResolver>();
        //    collection.AddTransient<ODataQueryValidator>();
        //    collection.AddTransient<TopQueryValidator>();
        //    collection.AddTransient<FilterQueryValidator>();
        //    collection.AddTransient<SkipQueryValidator>();
        //    collection.AddTransient<OrderByQueryValidator>();
        //    var provider = collection.BuildServiceProvider();
        //    var routeBuilder = new RouteBuilder(Mock.Of<IApplicationBuilder>(x => x.ApplicationServices == provider));
        //    routeBuilder.EnableDependencyInjection();

        //    var modelBuilder = new ODataConventionModelBuilder(provider);
        //    modelBuilder.EntitySet<TestModel>("TestModels");
        //    var model = modelBuilder.GetEdmModel();


        //    var httpContext = new DefaultHttpContext
        //    {
        //        RequestServices = provider
        //    };
        //    var httpcontext = new ODataQueryContext(model, type, new Microsoft.AspNet.OData.Routing.ODataPath());
        //    var options = new ODataQueryOptions<type>(httpcontext, a.Request);
        //}





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
            // var context = new ODataQueryContext(model.GetEdmModel(), typeof(TestModel), new Microsoft.AspNet.OData.Routing.ODataPath());
            var retVal = new ODataQueryContext(model.GetEdmModel(), type, new Microsoft.AspNet.OData.Routing.ODataPath());
            _cache.Add(type, retVal);
            return retVal;
        }


        public static bool IsAssignedTo(MemberInfo member)
        {
            if (member.DeclaringType.IsAssignableTo(typeof(ILocalizedDocument)))
            {
                if (member.DeclaringType.GetCustomAttributes(typeof(IsQueryableAttribute), false).Length > 0)
                {
                    return true;
                }
            }
            return false;
        }


        public IsQueryableAttribute()
        {
        }
    }
}
