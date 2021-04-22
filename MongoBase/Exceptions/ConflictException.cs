using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoBase.Exceptions
{
   public class ConflictExceptionException:Exception
    {

        public ConflictExceptionException() { }
        public ConflictExceptionException(string? message) : base(message)
        { }
    }
}
