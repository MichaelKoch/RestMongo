using System;
using RestMongo.Extensions.Exceptions.Abstractions;

namespace RestMongo.Extensions.Exceptions
{
    public class NotFoundException : HttpStatusCodeException
    {

        public NotFoundException(string message = "", Exception innerException = null) : base(message, 404)
        {

        }
    }
}
