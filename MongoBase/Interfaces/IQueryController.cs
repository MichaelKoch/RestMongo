using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using System.Collections.Generic;

namespace MongoBase.Controllers
{
    public interface IQueryController<TDocument> where TDocument : IDocument
    {

        ActionResult<PagedResultModel<TDocument>> Query([FromBody] dynamic query, [FromQuery(Name = "$orderby")] string orderby = "", [FromQuery(Name = "$expand")] string expand = "");
    }
}