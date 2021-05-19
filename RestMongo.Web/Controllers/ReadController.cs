using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestMongo.Domain.Abstractions.Models;
using RestMongo.Domain.Abstractions.Services;
using RestMongo.Web.Swashbuckle;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace RestMongo.Web.Controllers
{
    [Route("[controller]")]
    public abstract class ReadController<TReadModel> : ControllerBase
        where TReadModel : class
    {
        private readonly IReadDomainService<TReadModel> _readDomainService;
        protected int _maxPageSize; //TODO => get it from configuration

        public override bool TryValidateModel(object model)
        {
            return base.TryValidateModel(model);
        }

        //TODO : common error response model for all actions 
        public ReadController(IReadDomainService<TReadModel> readDomainService, int maxPageSize = 1000)
        {
            _readDomainService = readDomainService;
            _maxPageSize = maxPageSize;
        }

        [HttpPost("queries")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412, "MAX PAGE SIZE EXCEEDED", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerRequestExample(typeof(JsonDocument), typeof(QueryModelSampleProvider))]
        [SwaggerOperation(
            "query the ressource based on the mongo query language see:https://docs.mongodb.com/manual/tutorial/query-documents/")]
        public virtual async Task<ActionResult<IPagedResultModel<TReadModel>>> Query(
            JsonDocument query,
            [FromQuery(Name = "$orderby")] string orderby = "",
            [FromQuery(Name = "$expand")] string expand = ""
        )
        {
            var retVal = await _readDomainService
                .Query(JsonSerializer.Serialize(query), orderby, expand, _maxPageSize);
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
            var result = await _readDomainService.GetById(id, expand);
            return result;
        }
    }
}