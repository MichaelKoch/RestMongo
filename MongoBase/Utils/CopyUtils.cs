using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MongoBase.Utils
{
    public static class CopyUtils<TTarget>
    {
        public static TTarget Convert(object source)
        {
            if (source == null) return default(TTarget);
            return JsonSerializer.Deserialize<TTarget>( JsonSerializer.Serialize(source));
        }
        public static TTarget Convert(List<object> source)
        {
            if (source == null) return default(TTarget);
            return JsonSerializer.Deserialize<TTarget>(JsonSerializer.Serialize(source));
        }
    }
}
