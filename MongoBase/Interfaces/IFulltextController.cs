using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using System.Collections.Generic;

namespace MongoBase.Controllers
{
    public interface IReadController<TDocument> where TDocument : IDocument
    {
        ActionResult<PagedResultModel<TDocument>> Get([FromQuery(Name = "$top")] int top = 200, [FromQuery(Name = "$skip")] int skip = 0, [FromQuery(Name = "$filter")] string filter = "", [FromQuery(Name = "$expand")] string expand = "");
        ActionResult<TDocument> Get(string id, [FromQuery(Name = "$expand")] string expand = "");
        ActionResult<PagedResultModel<TDocument>> MongoQuery([FromBody] dynamic query, [FromQuery(Name = "$orderby")] string orderby = "", [FromQuery(Name = "$expand")] string expand = "");
        ActionResult<IList<TDocument>> Search([FromQuery(Name = "$term")] string searchTerm, [FromQuery(Name = "$expand")] string expand = "");
    }
}