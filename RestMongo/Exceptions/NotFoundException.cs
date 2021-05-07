using System;

namespace RestMongo.Exceptions
{
    public class NotFoundException : HttpStatusCodeException
    {

        public NotFoundException(string message = "", Exception innerException = null) : base(message, 404)
        {

        }
    }
}
