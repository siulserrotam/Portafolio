namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // Criteria for filtering
        System.Linq.Expressions.Expression<Func<T, bool>> Criteria { get; }

        // Includes for eager loading
        List<System.Linq.Expressions.Expression<Func<T, object>>> Includes { get; }
        

    }
}