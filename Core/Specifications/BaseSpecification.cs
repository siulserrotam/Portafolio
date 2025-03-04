using System.Linq.Expressions;
using System.Collections.Generic;

namespace Core.Specifications
{
    // Clase BaseSpecification que implementa la interfaz ISpecification
    public class BaseSpecification<T> : ISpecification<T>
    {
        // Constructor que permite definir un criterio de filtrado para la especificación
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        // Propiedad para almacenar el criterio de filtrado
        public Expression<Func<T, bool>> Criteria { get; }

        // Lista de expresiones para incluir entidades relacionadas (Eager Loading)
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        // Propiedad para especificar cómo deben ser ordenados los resultados (opcional)
        public Expression<Func<T, object>> OrderBy { get; private set; }

        // Propiedad para especificar cómo deben ser ordenados los resultados en orden descendente (opcional)
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        // Propiedades de paginación
        public int Skip { get; private set; }
        public int Take { get; private set; }

        // Método para agregar un Include (entidad relacionada) a la lista de inclusiones
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // Método para configurar el ordenamiento ascendente
        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        // Método para configurar el ordenamiento descendente
        public void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        // Método para agregar la paginación a la especificación
        public void AddPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }
    }
}
