namespace Core.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface ISpecification<T>
    {
        // Criteria for filtering
        Expression<Func<T, bool>> Criteria { get; }

        // Includes for eager loading
        List<Expression<Func<T, object>>> Includes { get; }
    }
}
