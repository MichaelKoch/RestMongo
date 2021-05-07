

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestMongo.Interfaces;
using RestMongo.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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

        public override bool TryValidateModel(object model)
        {
            return base.TryValidateModel(model);
        }
        
        //TODO : common error response model for all actions 
        public ReadController(IRepository<TEntity> repository, int maxPageSize = 1000)
        {
            _maxPageSize = maxPageSize;
            _repository = repository;
        }

        [HttpPost("queries")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412, "MAX PAGE SIZE EXCEEDED", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerRequestExample(typeof(JsonDocument),typeof(QueryModelSampleProvider))]
        [SwaggerOperation("query the ressource based on the mongo query language see:https://docs.mongodb.com/manual/tutorial/query-documents/")]
        public async virtual Task<ActionResult<PagedResultModel<TReadModel>>> Query(
           JsonDocument query,
          [FromQuery(Name = "$orderby")] string orderby = "",
          [FromQuery(Name = "$expand")] string expand = ""
        )
        {


            PagedResultModel<TEntity> result = this._repository.Query(JsonSerializer.Serialize( query), orderby, this._maxPageSize);
            var retVal = new PagedResultModel<TReadModel>()
            {
                Total = result.Total,
                Values = await this.LoadRelations(result.Values.Transform<List<TReadModel>>(), expand),
                Skip = 0,
                Top = result.Total
            };
            return Ok(retVal);
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "NOT FOUND", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
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