using Data.Enums;

public class MongoEvent : IMongoEvent
{
    public string CollectionName { get;set; }
    public string ObjectId { get;set; }
    public long ChangeAt { get;set; }
    public ChangeTypeEnum ChangeType {get;set;}
}