
using System.Collections.Generic;

using MongoDB.Bson;
using MongoBase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace MongoBase.Controllers
{
    public class Controller<TDocument> : ControllerBase where TDocument : IDocument
    {
        private readonly IRepository<TDocument> _repository;

        public Controller(IRepository<TDocument> repository)
        {
            this._repository = repository;
        }

        
        /// <summary>
        ///     
        /// </summary>
        /// <param name="query">a mongo query for syntax see : https://docs.mongodb.com/manual/tutorial/query-documents/</param>
        /// <returns>the query result</returns>
        [HttpPost("query")]
        public virtual IEnumerable<TDocument> Query([FromBody] dynamic query)
        {
            return this._repository.Query(JsonSerializer.Serialize(query));
        }

        [HttpGet("delta")]
        public virtual IEnumerable<TDocument> delta([FromQuery] long since = 0, [FromQuery] int skip = 0, [FromQuery] int take = 200)
        {
            var query = this._repository.AsQueryable().
                        Where(i => i.ChangedAt >= since)
                        .Take(take)
                        .Skip(skip)
                        .OrderBy(c => c.ChangedAt);
            return query;

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