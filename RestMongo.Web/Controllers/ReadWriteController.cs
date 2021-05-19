using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestMongo.Domain.Abstractions.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace RestMongo.Web.Controllers
{
    public abstract class
        ReadWriteController<TReadModel, TCreateModel, TUpdateModel> : ReadController<TReadModel>
        where TReadModel : class
        where TCreateModel : class
        where TUpdateModel : class
    {
        private readonly IReadWriteDomainService<TReadModel, TCreateModel, TUpdateModel> _readWriteDomainService;
        private bool _enableConcurrency;

        public ReadWriteController(
            IReadWriteDomainService<TReadModel, TCreateModel, TUpdateModel> readWriteDomainService,
            int maxPageSize = 200, bool enableConcurrency = false
        ) : base(readWriteDomainService, maxPageSize)
        {
            _maxPageSize = maxPageSize;
            _readWriteDomainService = readWriteDomainService;
            _enableConcurrency = enableConcurrency;
        }

        [HttpPost]
        [SwaggerResponse(200)]
        [SwaggerResponse(409, "CONFLICT", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public virtual async Task<ActionResult<TReadModel>> Create([FromBody] TCreateModel value)
        {
            var retVal = await _readWriteDomainService.Create(value);
            return Ok(retVal);
        }


        [HttpPut("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404, "NOT FOUND", typeof(ProblemDetails))]
        [Consumes("application/json")]
        [Produces("application/json")]
        public virtual async Task<ActionResult> Update(string id, [FromBody] TUpdateModel value)
        {
            await _readWriteDomainService.UpdateById(id, value, _enableConcurrency);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404, "NOT FOUND", typeof(ProblemDetails))]
        [SwaggerResponse(400, "BAD REQUEST")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public virtual async Task<ActionResult> Delete(string id)
        {
            await _readWriteDomainService.DeleteById(id);
            return NoContent();
        }
    }
}