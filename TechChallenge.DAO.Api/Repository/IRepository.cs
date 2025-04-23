using TechChallenge.DAO.Api.Entities;

namespace TechChallenge.DAO.Api.Repository
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
