namespace RestMongo.Interfaces
{
    public interface ITransformable
    {
        TTarget Transform<TTarget>();
    }
}
