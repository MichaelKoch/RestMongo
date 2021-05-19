using System;
using RestMongo.Domain.Abstractions.Exceptions;

namespace RestMongo.Domain.Exceptions
{
    public class NotFoundException : HttpStatusCodeException
    {
        public NotFoundException(string message = "", Exception innerException = null) : base(message, 404)
        {
        }
    }
}