using System;

namespace RestMongo.Exceptions
{
    public class PageSizeExeededException : Exception
    {


        public PageSizeExeededException(string message = "") : base(message)
        { }
    }
}
