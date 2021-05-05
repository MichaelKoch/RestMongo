using System;

namespace RestMongo.Exceptions
{
    public class PageSizeExeededException : HttpStatusCodeException
    {

        public PageSizeExeededException(string message = "", Exception innerException = null) : base(message, 412)
        {

        }
    }
}
