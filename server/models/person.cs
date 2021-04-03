using Data.Attributes;
using Data.Models;
namespace Models
{
    [BsonCollection("collection2")]
    public class Person : Document
    {
        public string FirstName { get; set;}
        public string LastName { get; set;}
    }
}