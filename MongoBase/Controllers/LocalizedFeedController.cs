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
using MongoBase.Utils;
using System.Threading.Tasks;
using MongoBase.Models;

namespace MongoBase.Controllers
{
    public abstract class LocalizedFeedController<TEntity, TDataTransfer> : LocalizedReadController<TEntity, TDataTransfer>
           where TEntity : LocalizedFeedDocument
    {
        [HttpGet("delta")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412)]
        public async virtual Task<ActionResult<IEnumerable<TDataTransfer>>> Delta([FromQuery] long since = 0,
             [FromQuery(Name = "$top")] int top = 200,
             [FromQuery(Name = "$skip")] int skip = 0)
        {
            if (top > this._maxPageSize)
            {
                return StatusCode(412, "MAX PAGE SIZE EXCEEDED");

            }
            var result = await Task.Run(() =>
            {
                var query =
                this._repository.AsQueryable()
                           .Where(i => i.Timestamp > since)
                           .Take(top)
                           .Skip(skip)
                           .OrderByDescending(c => c.Timestamp);
                return query.ToList();
            });
            return Ok(CopyUtils<List<TDataTransfer>>.Convert(result));
        }

        public LocalizedFeedController(IRepository<TEntity> repository, int maxPageSize = 500) : base(repository)
        {
            this._repository = repository;
            this._maxPageSize = maxPageSize;
        }
    }
}