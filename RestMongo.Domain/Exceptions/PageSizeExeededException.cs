using System;
using RestMongo.Domain.Abstractions.Exceptions;

namespace RestMongo.Domain.Exceptions
{
    public class PageSizeExeededException : HttpStatusCodeException
    {

        public PageSizeExeededException(string message = "", Exception innerException = null) : base(message, 412)
        {

        }
    }
}
