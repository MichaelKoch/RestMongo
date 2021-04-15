using Sample.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Interfaces
{
    public interface IProductContext
    {
        ProductRepository Products { get; }
        ProductColorRepository ProductColors { get; }
        ProductColorSizeRepository ProductColorSizes { get; }
    }
}
