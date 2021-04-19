using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using System.Collections.Generic;

namespace MongoBase.Controllers
{
    public interface IDeltaController<TDocument> where TDocument : IDocument
    {

        ActionResult<IEnumerable<TDocument>> Delta([FromQuery] long since = 0, [FromQuery(Name = "$top")] int top = 200, [FromQuery(Name = "$skip")] int skip = 0);

    }
}