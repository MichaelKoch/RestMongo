using System;

namespace RestMongo.Extensions.Exceptions.Abstractions
{
    public abstract class HttpStatusCodeException : Exception
    {
        public HttpStatusCodeException(
                string message,
                int statusCode = 500,
                Exception innerException = null
                                        ) : base(message, innerException)
        {
            _httpStatusCode = statusCode;
        }
        private int _httpStatusCode;

        public int HttpStatusCode { get => _httpStatusCode; set => _httpStatusCode = value; }
    }
}
