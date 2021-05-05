using Microsoft.AspNetCore.Mvc;
using RestMongo.Interfaces;
using RestMongo.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace RestMongo.Controllers
{
    public abstract class ReadWriteController<TEntity, TReadModel, TCreateModel, TUpdateModel> : ReadController<TEntity, TReadModel>
        where TEntity : FeedDocument
        where TReadModel : class
        where TCreateModel : class
        where TUpdateModel : class
    {
        private bool _enableConcurrency;

        public ReadWriteController(IRepository<TEntity> repository, int maxPageSize = 200, bool enableConcurrency = false) : base(repository, maxPageSize)
        {
            this._repository = repository;
            this._maxPageSize = maxPageSize;
            this._enableConcurrency = enableConcurrency;
        }


        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(400, "BAD REQUEST", typeof(ProblemDetails))]
        [SwaggerResponse(409, "CONFLICT", typeof(ProblemDetails))]
        [SwaggerResponse(500, "INTERNAL SERVER ERROR", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation("create new instance")]
        public virtual async Task<ActionResult<TReadModel>> Create([FromBody] TCreateModel value)
        {
            var feedInfo = value.Transform<FeedDocument>();
            var instance = await this._repository.FindByIdAsync(feedInfo.Id);
            if (instance != null)
            {
                return Conflict("DUPLICATE KEY");
            }
            TEntity insert = value.Transform<TEntity>();
            this._repository.InsertOne(insert);
            return insert.Transform<TReadModel>();
        }


        [HttpPut("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404, "NOT FOUND", typeof(ProblemDetails))]
        [SwaggerOperation("replace instance by ID")]
        [SwaggerResponse(400, "BAD REQUEST", typeof(ProblemDetails))]
        [SwaggerResponse(500, "INTERNAL SERVER ERROR", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
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
                if ((feedInfo.Timestamp == 0) || (instance.Timestamp != feedInfo.Timestamp))
                {
                    return Conflict("CONCURRENCY CONFLICT");
                }
            }
            this._repository.ReplaceOne(updateInstance);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404, "NOT FOUND", typeof(ProblemDetails))]
        [SwaggerResponse(400, "BAD REQUEST")]
        [SwaggerResponse(500, "INTERNAL SERVER ERROR", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
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
