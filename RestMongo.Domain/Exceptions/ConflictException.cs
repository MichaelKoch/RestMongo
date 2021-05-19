using System;
using RestMongo.Domain.Abstractions.Exceptions;

namespace RestMongo.Domain.Exceptions
{
    public class ConflictException : HttpStatusCodeException
    {
        public ConflictException(string message = "", Exception innerException = null) : base(message, 409)
        {
        }
    }
}