using System;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Builder;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;

namespace MongoBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class IsQueryableAttribute : Attribute
    {
        //TODO : Use threadsafe caching solution
        private static Dictionary<Type, ODataQueryContext> _cache = new Dictionary<Type, ODataQueryContext>();

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
        public IsQueryableAttribute()
        {
        }
    }
}
