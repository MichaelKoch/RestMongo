

using RestMongo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using System.Web;
using Microsoft.AspNetCore.Http;
using RestMongo.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using RestMongo.Exceptions;
using RestMongo.Utils;
using RestMongo.Models;
using System;

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
        [SwaggerResponse(400)]
        [SwaggerResponse(500)]
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