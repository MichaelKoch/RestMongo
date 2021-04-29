using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestMongo.Interfaces
{
    public interface ITransformable
    {
        TTarget Transform<TTarget>();
    }
}
