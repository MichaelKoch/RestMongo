using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using System.Collections.Generic;

namespace MongoBase.Controllers
{
    public interface IFulltextController<TDocument> where TDocument : IDocument
    {
        ActionResult<IList<TDocument>> Search([FromQuery(Name = "$term")] string searchTerm, [FromQuery(Name = "$expand")] string expand = "");
    }
}