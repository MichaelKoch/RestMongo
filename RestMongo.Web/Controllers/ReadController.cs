using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestMongo.Data.Abstractions.Extensions;
using RestMongo.Data.Abstractions.Repository;
using RestMongo.Data.Abstractions.Repository.Mongo.Documents;
using RestMongo.Web.Models;
using RestMongo.Web.Swashbuckle;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace RestMongo.Web.Controllers
{
    [Route("[controller]")]
    public abstract class ReadController<TEntity, TReadModel> : DocumentControllerBase<TReadModel>
            where TEntity : class, IDocument
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
        public virtual async Task<ActionResult<PagedResultModel<TReadModel>>> Query(
           JsonDocument query,
          [FromQuery(Name = "$orderby")] string orderby = "",
          [FromQuery(Name = "$expand")] string expand = ""
        )
        {
            var result = this._repository.Query(JsonSerializer.Serialize( query), orderby, out var total, this._maxPageSize).ToList();
            var retVal = new PagedResultModel<TReadModel>()
            {
                Total = total,
                Values = await this.LoadRelations(result.Transform<List<TReadModel>>(), expand),
                Skip = 0,
                Top = total
            };
            return Ok(retVal);
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "NOT FOUND", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public virtual async Task<ActionResult<TReadModel>> Get(string id,
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