using Microsoft.AspNetCore.Mvc;
using MongoBase.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoBase.Interfaces.Controllers
{
    public interface IReadController<TDocument> where TDocument : IDocument
    {
        ActionResult<PagedResultModel<TDocument>> Get([FromQuery(Name = "$top")] int top = 200, [FromQuery(Name = "$skip")] int skip = 0, [FromQuery(Name = "$filter")] string filter = "", [FromQuery(Name = "$expand")] string expand = "");
        ActionResult<TDocument> Get(string id, [FromQuery(Name = "$expand")] string expand = "");


    }
}