using System;
using RestMongo.Extensions.Exceptions.Abstractions;

namespace RestMongo.Extensions.Exceptions
{
    public class PageSizeExeededException : HttpStatusCodeException
    {

        public PageSizeExeededException(string message = "", Exception innerException = null) : base(message, 412)
        {

        }
    }
}
