using MongoDB.Bson;
using MongoDB.Driver;
using RestMongo.Interfaces;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestMongo
{
    public static class ObjectExtentsions
    {
        public static TTarget Transform<TTarget>(this object source) 
               where TTarget:class
        {
            if(source.GetType().IsAssignableTo(typeof(ITransformable)))
            {
                return ((ITransformable)source).Transform<TTarget>();
            }
            return JsonSerializer.Deserialize<TTarget>(JsonSerializer.Serialize(source,source.GetType()),
                new JsonSerializerOptions() {
                PropertyNameCaseInsensitive=true,
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase                
             });
        }
    }
}