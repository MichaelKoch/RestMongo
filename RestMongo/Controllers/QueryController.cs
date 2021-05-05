using Microsoft.AspNetCore.Mvc;
using RestMongo.Interfaces;
using RestMongo.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestMongo.Controllers
{
    [Route("[controller]")]
    public abstract class QueryController<TEntity, TDataTransfer> : DocumentControllerBase<TEntity, TDataTransfer>
            where TEntity : BaseDocument
            where TDataTransfer : class
    {
        protected IRepository<TEntity> _repository;
        protected int _maxPageSize = 0;

        public QueryController(IRepository<TEntity> repository, int maxPageSize = 1000)
        {
            _maxPageSize = maxPageSize;
            _repository  = repository;
        }

        [HttpPost("queries")]
        [SwaggerResponse(200)]
        [SwaggerResponse(412, "MAX PAGE SIZE EXCEEDED", typeof(ProblemDetails))]
        public async virtual Task<ActionResult<PagedResultModel<TDataTransfer>>> Query(
            [FromBody] dynamic query,
            [FromQuery(Name = "$orderby")] string orderby = "",
            [FromQuery(Name = "$expand")]  string expand  = ""
        )
        {
            PagedResultModel<TEntity> result = this._repository.Query(JsonSerializer.Serialize(query), orderby, this._maxPageSize);
            var retVal = new PagedResultModel<TDataTransfer>()
            {
                Total = result.Total,
                Values = await this.LoadRelations(result.Values.Transform<List<TDataTransfer>>(), expand),
                Skip = 0,
                Top = result.Total
            };
            return Ok(retVal);
        }
    }
}
