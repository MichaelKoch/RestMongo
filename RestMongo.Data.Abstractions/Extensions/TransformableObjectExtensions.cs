using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RestMongo.Data.Abstractions.Transform;

namespace RestMongo.Data.Abstractions.Extensions
{
    public static class TransformableObjectExtensions
    {

        public static IList<TTarget> Transform<TSource, TTarget>(this IList<TSource> source)
                where TTarget : class
                where TSource : ITransformable
        {
            var temp = new ConcurrentBag<TTarget>();
            Parallel.ForEach(source, (instance) =>
            {
                TTarget clone = instance.Transform<TTarget>();
            });
            return temp.ToList();
        }

        public static TTarget Transform<TTarget>(this object source)
               where TTarget : class
        {
            if (source.GetType().IsAssignableTo(typeof(ITransformable)))
            {
                return ((ITransformable)source).Transform<TTarget>();
            }
            return JsonSerializer.Deserialize<TTarget>(JsonSerializer.Serialize(source, source.GetType()),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
        }
    }
}