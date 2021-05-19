namespace RestMongo.Data.Abstractions.Transform
{
    public interface ITransformable
    {
        TTarget Transform<TTarget>();
    }
}
