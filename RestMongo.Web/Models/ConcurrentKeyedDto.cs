namespace RestMongo.Web.Controllers
{
    public class ConcurrentKeyedDto: KeyedDto
    {
        public long Timestamp { get; set; }
    }
}