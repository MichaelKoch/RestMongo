namespace RestMongo.Domain.Models
{
    public class ConcurrentKeyedDto: KeyedDto
    {
        public long Timestamp { get; set; }
    }
}