using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using System.Collections.Generic;

namespace MongoBase.Controllers
{
    public interface IReadWriteController<TDocument> where TDocument : IDocument
    {
        ActionResult Delete(string id);
        ActionResult<string> Post([FromBody] TDocument value);
        ActionResult Put(string id, [FromBody] TDocument value);
    }
}