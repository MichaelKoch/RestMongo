

using Microsoft.AspNetCore.Mvc;
using RestMongo.Interfaces;
using RestMongo.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace RestMongo.Controllers
{
    [Route("[controller]")]
    public abstract class ReadController<TEntity, TReadModel> : DocumentControllerBase<TEntity, TReadModel>
            where TEntity : BaseDocument
            where TReadModel : class
    {
        protected IRepository<TEntity> _repository;
        protected int _maxPageSize = 0; //TODO => get it from configuration


        //TODO : common error response model for all actions 
        public ReadController(IRepository<TEntity> repository, int maxPageSize = 1000)
        {
            _maxPageSize = maxPageSize;
            _repository = repository;

        }

        [HttpGet("{id}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "NOT FOUND", typeof(ProblemDetails))]
        [SwaggerResponse(400, "BAD REQUEST", typeof(ProblemDetails))]
        [SwaggerResponse(500, "INTERNAL SERVER ERROR", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation("Get item by id ")]
        public async virtual Task<ActionResult<TReadModel>> Get(string id,
                [FromQuery(Name = "$expand")] string expand = "")
        {
            TEntity instance = this._repository.FindById(id);
            if (instance == null)
            {
                return NotFound();
            }
            TReadModel dto = instance.Transform<TReadModel>();
            var result = await this.LoadRelations(dto as TReadModel, expand);
            return result;
        }
    }
}