using MongoBase.Enums;
using MongoBase.Interfaces;
namespace MongoBase.Attributes
{
    public class MongoEvent : IMongoEvent
    {
        public string CollectionName { get; set; }
        public string ObjectId { get; set; }
        public long ChangeAt { get; set; }
        public ChangeTypeEnum ChangeType { get; set; }
    }
}
