
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoBase.Interfaces;
using MongoBase.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace MongoBase.Controllers
{
    public class Controller<TDocument> : ControllerBase where TDocument : IDocument
    {
        private readonly IRepository<TDocument> _repository;

        public Controller(IRepository<TDocument> repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IQueryable<TDocument> Get()
        {
            return this._repository.AsQueryable();
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public virtual TDocument Get(string id)
        {
            return this._repository.FindById(id);
        }

        // POST api/<TestController>
        [HttpPost]
        public virtual string Post([FromBody] TDocument value)
        {
            return this._repository.InsertOne(value).Id.ToString();
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public virtual void Put(string id, [FromBody] TDocument value)
        {
            value.Id = new ObjectId(id);
            this._repository.ReplaceOne(value);
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public virtual void Delete(string id)
        {
            this._repository.DeleteById(id);
        }


    }
}