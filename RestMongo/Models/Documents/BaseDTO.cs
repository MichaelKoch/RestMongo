using RestMongo.Interfaces;
using System.Text.Json;

namespace RestMongo.Models
{
    public class BaseDTO : ITransformable
    {
        public virtual TTarget Transform<TTarget>()
        {
            return JsonSerializer.Deserialize<TTarget>(JsonSerializer.Serialize(this));
        }
    }
}
