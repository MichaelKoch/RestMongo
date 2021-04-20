using MongoBase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using System.Web;
using Microsoft.AspNetCore.Http;
using MongoBase.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System;

namespace MongoBase.Controllers
{
    public class ReadDeltaController<TDocument> : ReadController<TDocument>,IDeltaController<TDocument> where TDocument : IDocument
    {
        public ReadDeltaController(IRepository<TDocument> repository):base(repository)
        {
            
        }   
        [HttpGet("delta")]
        [SwaggerResponse(200)]
        public virtual ActionResult<IEnumerable<TDocument>> Delta([FromQuery] long since = 0,
             [FromQuery(Name = "$top")] int top = 200,
             [FromQuery(Name = "$skip")] int skip = 0)
        {
            if (top - skip > this.maxPageSize)
            {
                StatusCode(412, "MAX PAGE SIZE EXCEEDED");
            }
            var query = this._repository.AsQueryable()
                        .Where(i => i.ChangedAt > since)
                        .Take(top)
                        .Skip(skip)
                        .OrderByDescending(c => c.ChangedAt);
            return query.ToList();
        }
    }
}