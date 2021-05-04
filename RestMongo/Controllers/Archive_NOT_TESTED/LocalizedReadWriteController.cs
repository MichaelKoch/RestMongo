



//using System.Collections.Generic;
//using RestMongo.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;
//using System.Linq;
//using Microsoft.AspNet.OData.Query;
//using System;
//using System.Web;
//using Microsoft.AspNetCore.Http;
//using RestMongo.Attributes;
//using Swashbuckle.AspNetCore.Annotations;
//using RestMongo.Models;

//namespace RestMongo.Controllers
//{
//    public abstract class LocalizedReadWriteController<TEntity, TModel, TUpdateModel> : LocalizedFeedController<TEntity, TModel>
//        where TEntity :LocalizedFeedDocument
//        where TModel : class
//        where TUpdateModel : class
//    {

//        private bool _enableConcurrency;

//        public LocalizedReadWriteController(IRepository<TEntity> repository, int maxPageSize = 100,bool enableConcurrency = false) : base(repository, maxPageSize)
//        {
//            this._repository = repository;
//            this._maxPageSize = maxPageSize;
//            this._enableConcurrency = enableConcurrency;
//        }


//        [HttpPost]
//        [SwaggerResponse(200)]
//        [SwaggerResponse(409, "CONFLICT")]
//        [SwaggerOperation("create new instance")]
//        public virtual ActionResult<TModel> Post([FromBody] TUpdateModel value)
//        {
//            var feedInfo = value.Transform<FeedDocument>();
//            var instance = this._repository.FindById(feedInfo.Id);
//            if (instance != null)
//            {
//                return Conflict("DUPLICATE KEY");
//            }
//            feedInfo.Timestamp = DateTime.UtcNow.Ticks;
//            TEntity insert = value.Transform<TEntity>();
//            this._repository.InsertOne(insert);
//            return insert.Transform<TModel>();

//        }


//        [HttpPut("{id}")]
//        [SwaggerResponse(204)]
//        [SwaggerResponse(404)]
//        [SwaggerOperation("replace instance by ID")]
//        public virtual ActionResult Put(string id, [FromBody] TUpdateModel value)

//        {
//            var feedInfo = value.Transform<FeedDocument>();
//            var instance = this._repository.FindById(id);
//            if (instance == null)
//            {
//                return NotFound();
//            }
//            var updateInstance = value.Transform<TEntity>();
//            updateInstance.Id = id;
//            if (this._enableConcurrency)
//            {
//                if ((feedInfo.Timestamp == 0) || (instance.Timestamp != feedInfo.Timestamp))
//                {
//                    return Conflict("CONCURRENCY CONFLICT");
//                }
//            }
//            feedInfo.Id = id;
//            this._repository.ReplaceOne(updateInstance);
//            return NoContent();
//        }


//        //[HttpPatch("{id}")]
//        //[SwaggerResponse(204)]
//        //[SwaggerResponse(404)]
//        //[SwaggerOperation("patch instance values by ID")]
//        //public virtual ActionResult Patch(string id, [FromBody] Partial<TEntity> value)
//        //{

//        //    if(value.Remove<TEntity>(c=> c.Id))

//        //    var feedInfo = value.Transform<FeedDocument>();
//        //    var instance = this._repository.FindById(feedInfo.Id);
//        //    if (instance == null)
//        //    {
//        //        return NotFound();
//        //    }
//        //    var updateInstance = value.Transform<TEntity>();

//        //    value.ApplyTo(instance);
//        //    this._repository.ReplaceOne(updateInstance);
//        //    return NoContent();
//        //}

//        [HttpDelete("{id}")]
//        [SwaggerResponse(204)]
//        [SwaggerResponse(404)]
//        [SwaggerOperation("Delete instance by ID")]
//        public virtual ActionResult Delete(string id)
//        {
//            var instance = this._repository.FindById(id);
//            if (instance == null)
//            {
//                return NotFound();
//            }
//            this._repository.DeleteById(id);
//            return NoContent();
//        }
//    }
//}