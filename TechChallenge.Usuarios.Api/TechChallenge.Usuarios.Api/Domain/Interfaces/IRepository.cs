using TechChallenge.Usuarios.Api.Domain.Entities;

namespace TechChallenge.Usuarios.Api.Domain.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<IList<T>> GetAllAsync();
        IList<T> GetAll();
        Task<T> GetByIdAsync(int id);
        T GetById(int id);
        Task AddAsync(T entidade);
        void Add(T entidade);
        void Update(T entidade);
        void Delete(int id);
        Task SaveChangesAsync();
        void SaveChanges();
        IQueryable<T> GetAllAsNoTracking();
    }
}
