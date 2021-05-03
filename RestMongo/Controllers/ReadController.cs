

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
    public abstract class ReadController<TEntity, TDataTransfer> : DocumentControllerBase<TEntity, TDataTransfer>
            where TEntity : BaseDocument
            where TDataTransfer : class
    {
        protected IRepository<TEntity> _repository;
        protected int _maxPageSize = 0; //TODO => get it from configuration

        public ReadController(IRepository<TEntity> repository, int maxPageSize = 1000)
        {
            _maxPageSize = maxPageSize;
            _repository = repository;
            
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404, "NOT FOUND", typeof(string))]
        [SwaggerOperation("Get item by id ")]
        public async virtual Task<ActionResult<TDataTransfer>> Get(string id, 
                [FromQuery(Name = "$expand")] string expand = "")
        {
            TEntity instance = this._repository.FindById(id);
            if (instance == null)
            {
                return NotFound();
            }

            TDataTransfer dto = instance.Transform<TDataTransfer>();
            var result = await this.LoadRelations(dto as TDataTransfer, expand);
            return result;
        }

        
       
    }
}