

using System.Collections.Generic;
using RestMongo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using RestMongo.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using RestMongo.Models;
using RestMongo.Utils;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace RestMongo.Controllers
{
    public abstract class ReadWriteController<TEntity, TModel,TUpdateModel> : FeedController<TEntity, TModel>
        where TEntity : FeedDocument
        where TModel : class
        where TUpdateModel : class
    {
        private bool _enableConcurrency;

        public ReadWriteController(IRepository<TEntity> repository, int maxPageSize = 200,bool enableConcurrency = false) : base(repository, maxPageSize)
        {
            this._repository = repository;
            this._maxPageSize = maxPageSize;
            this._enableConcurrency = enableConcurrency;
        }


        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(409, "CONFLICT")]
        [SwaggerOperation("create new instance")]
        public virtual async Task<ActionResult<TModel>> Create([FromBody] TUpdateModel value)
        {
            var feedInfo = value.Transform<FeedDocument>();
            var instance =await  this._repository.FindByIdAsync(feedInfo.Id);
            if (instance != null)
            {
                return Conflict("DUPLICATE KEY");
            }
            TEntity insert = value.Transform<TEntity>();
            this._repository.InsertOne(insert);
            return insert.Transform<TModel>();
             
        }


        [HttpPut("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404)]
        [SwaggerOperation("replace instance by ID")]
        public virtual async Task<ActionResult> Update(string id, [FromBody] TUpdateModel value)

        {
            var feedInfo = value.Transform<FeedDocument>();
            var instance = await this._repository.FindByIdAsync(id);
            if (instance == null)
            {
                return NotFound();
            }
            var updateInstance = value.Transform<TEntity>();
            updateInstance.Id = id;
            if (this._enableConcurrency)
            {
                if ((feedInfo.Timestamp ==0) || (instance.Timestamp != feedInfo.Timestamp))
                {
                    return Conflict("CONCURRENCY CONFLICT");
                }
            }   
            this._repository.ReplaceOne(updateInstance);
            return NoContent();
        }
          
        [HttpDelete("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404)]
        [SwaggerOperation("Delete instance by ID")]
        public async virtual Task<ActionResult> Delete(string id)
        {
            var instance = this._repository.FindById(id);
            if (instance == null)
            {
                return NotFound();
            }
            await this._repository.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
