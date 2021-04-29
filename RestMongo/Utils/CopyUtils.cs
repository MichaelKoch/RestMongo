using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestMongo.Utils
{
    public static class CopyUtils<TTarget>
    {
        public static TTarget Convert(object source)
        {
            return JsonSerializer.Deserialize<TTarget>(JsonSerializer.Serialize(source));
        }
        public static TTarget Convert(List<object> source)
        {
            return JsonSerializer.Deserialize<TTarget>(JsonSerializer.Serialize(source));
        }
    }
}
