using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using System.Collections.Generic;

namespace MongoBase.Interfaces.Controllers
{
    public interface IReadWriteController<TDocument> : IQueryController<TDocument> where TDocument : IDocument
    {
        ActionResult Delete(string id);
        ActionResult<string> Post([FromBody] TDocument value);
        ActionResult Put(string id, [FromBody] TDocument value);
    }
}