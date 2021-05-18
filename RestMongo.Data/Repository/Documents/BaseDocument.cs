using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using RestMongo.Data.Abstractions.Repository.Mongo.Documents;
using MongoDB.Bson.Serialization.Attributes;
using RestMongo.Data.Attributes;

namespace RestMongo.Data.Repository.Documents
{
    [BsonIgnoreExtraElements]
    public class BaseDocument : IDocument
    {
        [IsQueryable]
        [JsonPropertyName("Id")]
        public virtual string Id { get; set; }


        public BaseDocument()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public virtual TTarget Transform<TTarget>()
        {
            return JsonSerializer.Deserialize<TTarget>(JsonSerializer.Serialize(this, this.GetType()),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
        }
    }
}