using TechChallenge.DAO.Worker.Entities;

namespace TechChallenge.DAO.Worker.Infra.Repository.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
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
