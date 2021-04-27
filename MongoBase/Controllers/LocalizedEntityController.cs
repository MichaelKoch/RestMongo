



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
using MongoBase.Models;

namespace MongoBase.Controllers
{
    public abstract class LocalizedEntityController<TEntity, TDataTransfer> : LocalizedFeedController<TEntity, TDataTransfer>
        where TEntity : LocalizedFeedDocument
    {
        public LocalizedEntityController(IRepository<TEntity> repository, int maxPageSize = 100) : base(repository, maxPageSize)
        {
            this._repository = repository;
            this._maxPageSize = maxPageSize;
        }

        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(409, "CONFLICT")]
        [SwaggerOperation("create new instance")]
        public virtual ActionResult<string> Post([FromBody] TEntity value)
        {
            var instance = this._repository.FindById(value.Id);
            if (instance != null)
            {
                return Conflict("DUPLICATE KEY");
            }
            return this._repository.InsertOne(value).Id.ToString(); ;
        }


        [HttpPut("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404)]
        [SwaggerOperation("Update instance by ID")]
        public virtual ActionResult Put(string id, [FromBody] TEntity value)
        {
            var instance = this._repository.FindById(value.Id);
            if (instance == null)
            {
                return NotFound();
            }
            if (instance.Timestamp != value.Timestamp)
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