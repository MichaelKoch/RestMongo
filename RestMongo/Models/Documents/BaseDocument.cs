using MongoDB.Bson.Serialization.Attributes;
using RestMongo.Attributes;
using RestMongo.Interfaces;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestMongo.Models
{
    [BsonIgnoreExtraElements]
    public class BaseDocument : IDocument
    {
        [IsQueryableAttribute]
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