using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces  // Cambio de MySkinet.Core.Interfaces a Core.Interfaces
{
    // Interfaz genérica que hereda de BaseEntity para trabajar con cualquier entidad
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Método para obtener una entidad por su ID
        Task<T> GetByIdAsync(int id);//Obtener entidad por ID
        Task<IReadOnlyList<T>> ListAllAsync();//Listar todas las entidades
        Task<T> GetEntityWithSpec(ISpecification<T> spec);//Obtener entidad con especificación
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);//Listar entidades con especificación
    }
}