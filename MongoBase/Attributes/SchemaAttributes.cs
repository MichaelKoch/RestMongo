using System;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Builder;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;

namespace MongoBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class SchemaAttribute : Attribute
    {

        public static ODataQueryContext GetODataQueryContext(Type type)
        {
      
            var model = new ODataModelBuilder();
            var entityType = model.AddEntityType(type);
            foreach (var pi in type.GetProperties())
            {
                var modelInfo = new List<SchemaAttribute>(pi.GetCustomAttributes(typeof(SchemaAttribute), true) as SchemaAttribute[]).LastOrDefault();
                if (modelInfo != null)
                {
                    if (modelInfo.IsCollection)
                    {
                        entityType.AddCollectionProperty(pi);
                    }
                    if (modelInfo.IsSimple)
                    {
                        entityType.AddProperty(pi);
                    }
                    if (modelInfo.IsComplex)
                    {
                        entityType.AddComplexProperty(pi);
                    }
                }
            }
            // var context = new ODataQueryContext(model.GetEdmModel(), typeof(TestModel), new Microsoft.AspNet.OData.Routing.ODataPath());
            return new ODataQueryContext(model.GetEdmModel(), type, new Microsoft.AspNet.OData.Routing.ODataPath());
        }
        public Boolean IsCollection = false;
        public Boolean IsComplex = false;
        public Boolean IsSimple = false;
        public SchemaAttribute(Boolean isSimple = false, Boolean isComplex = false, Boolean isCollection = false)
        {
            this.IsComplex = isComplex;
            this.IsSimple = isSimple;
            this.IsCollection = isCollection;
        }
    }
}
