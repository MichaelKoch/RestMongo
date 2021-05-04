using MongoDB.Driver;
using RestMongo.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestMongo
{
    public static class IListExtentsions
    {
        public static IList<TTarget> Transform<TSource, TTarget>(this IList<TSource> source)
            where TTarget : class
            where TSource : ITransformable
        {
            var temp = new ConcurrentBag<TTarget>();
            Parallel.ForEach(source, (instance) =>
            {
                TTarget clone = instance as TTarget;
            });
            return temp.ToList();
        }
    }
}