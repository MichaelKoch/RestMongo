using RestMongo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using System.Web;
using Microsoft.AspNetCore.Http;
using RestMongo.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System;
using RestMongo.Models;
using System.Threading.Tasks;

namespace RestMongo.Controllers
{
    public abstract class FeedController<TEntity, TDataTransfer> : ReadController<TEntity, TDataTransfer>
            where TEntity : FeedDocument
            where TDataTransfer:class
    {
        [HttpGet("delta")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412)]
        public virtual async Task<ActionResult<IEnumerable<TDataTransfer>>> Delta([FromQuery] long timestamp = 0,
             [FromQuery(Name = "$top")] int top = 200,
             [FromQuery(Name = "$skip")] int skip = 0)
        {
            if (top - skip > 100)
            {
                StatusCode(412, "MAX PAGE SIZE EXCEEDED");
            }
            var result = await Task.Run<IList<TEntity>>(() =>
            {
                return this._repository.AsQueryable()
                       .Where(i => i.Timestamp > timestamp)
                       .Take(top)
                       .Skip(skip)
                       .OrderByDescending(c => c.Timestamp).ToList();
            });

            return Ok(result);
        }

        public FeedController(IRepository<TEntity> repository, int maxPageSize = 100) : base(repository)
        {
            this._repository = repository;
            this._maxPageSize = maxPageSize;
        }
    }
}