using MongoBase.Enums;
namespace MongoBase.Interfaces
{
    public interface IMongoEvent
    {
        string CollectionName { get; set; }
        string ObjectId { get; set; }
        long ChangeAt { get; set; }
        ChangeTypeEnum ChangeType { get; set; }
    }
}
