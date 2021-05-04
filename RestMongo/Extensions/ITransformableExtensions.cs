using RestMongo.Interfaces;
using System.Text.Json;

namespace RestMongo
{
    public static class ObjectExtentsions
    {
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