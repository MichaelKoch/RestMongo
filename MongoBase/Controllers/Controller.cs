
using System.Collections.Generic;
using MongoBase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using MongoBase.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MongoBase.Controllers
{
    public class Controller<TDocument> : ReadController<TDocument> where TDocument : IDocument
    {
        public Controller(IRepository<TDocument> repository):base(repository)
        {}

        [HttpGet("delta")]
        [SwaggerResponse(200)]
        public virtual IEnumerable<TDocument> Delta([FromQuery] long since = 0, [FromQuery] int skip = 0, [FromQuery] int take = 200)
        {
            var query = this._repository.AsQueryable().
                        Where(i => i.ChangedAt > since)
                        .Take(take)
                        .Skip(skip)
                        .OrderByDescending(c => c.ChangedAt);
            return query;
        }
        // POST api/<TestController>
        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(409, "CONFLICT")]
        public virtual ActionResult<string> Post([FromBody] TDocument value)
        {
            var instance = this._repository.FindById(value.Id);
            if (instance != null)
            {
                return Conflict("DUPLICATE KEY");
            }
            return this._repository.InsertOne(value).Id.ToString(); ;
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(409)]
        public virtual ActionResult Put(string id, [FromBody] TDocument value)
        {
            var instance = this._repository.FindById(value.Id);
            if (instance == null)
            {
                return NotFound();
            }
            if (instance.ChangedAt != value.ChangedAt)
            {
                return Conflict("CONCURRENT UPDATE");
            }
            value.Id = id;
            this._repository.ReplaceOne(value);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404)]
        [SwaggerOperation("Delete instance by ID")]
        public virtual ActionResult Delete(string id)
        {
            var instance = this._repository.FindById(id);
            if (instance == null)
            {
                return NotFound();
            }
            this._repository.DeleteById(id);
            return NoContent();
        }
    }
}