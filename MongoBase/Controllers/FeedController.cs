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
    public class LocalizedFeedController<TDocument> : LocalizedReadController<TDocument> where TDocument : ILocalizedFeedDocument
    {
        [HttpGet("delta")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412)]
        public virtual ActionResult<IEnumerable<TDocument>> Delta([FromQuery] long since = 0,
             [FromQuery(Name = "$top")] int top = 200,
             [FromQuery(Name = "$skip")] int skip = 0)
        {
            if (top > this._maxPageSize)
            {
                StatusCode(412, "MAX PAGE SIZE EXCEEDED");
            }
            var query = this._repository.AsQueryable()
                        .Where(i => i.Timestamp > since)
                        .Take(top)
                        .Skip(skip)
                        .OrderByDescending(c => c.Timestamp);
            return query.ToList();
        }

        public LocalizedFeedController(IRepository<TDocument> repository,int maxPageSize = 500):base(repository)
        {
            this._repository = repository;
            this._maxPageSize = maxPageSize;
        }
    }
}