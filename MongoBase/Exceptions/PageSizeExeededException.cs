using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoBase.Exceptions
{
    public class PageSizeExeededException:Exception
    {

   
        public PageSizeExeededException(string? message):base(message)
        { }
    }
}
