using RestMongo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestMongo.Models
{
    public class BaseDTO:ITransformable
    {
        public virtual TTarget Transform<TTarget>()
        {
            return JsonSerializer.Deserialize<TTarget>(JsonSerializer.Serialize(this));
        }
    }
}
