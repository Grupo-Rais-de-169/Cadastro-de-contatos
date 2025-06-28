using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.DAO.Worker.Entities;
using TechChallenge.DAO.Worker.Infra.Context;
using TechChallenge.DAO.Worker.Infra.Repository.Interfaces;

namespace TechChallenge.DAO.Worker.Infra.Repository
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        protected MainContext _context;
        protected DbSet<T> _dbSet;

        public Repository(MainContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entidade)
        {
            _dbSet.Add(entidade);
            _context.SaveChanges();
        }

        public async Task AddAsync(T entidade)
        {
            await _dbSet.AddAsync(entidade);
            await _context.SaveChangesAsync();
        }

        [ExcludeFromCodeCoverage]
        public void Update(T entidade)
        {
            _dbSet.Update(entidade);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _dbSet.Remove(GetById(id));
            _context.SaveChanges();
        }

        public T GetById(int id) => _dbSet.FirstOrDefault(x => x.Id == id);

        [ExcludeFromCodeCoverage]
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        [ExcludeFromCodeCoverage]
        public void SaveChanges() => _context.SaveChanges();

        [ExcludeFromCodeCoverage]
        public IQueryable<T> GetAllAsNoTracking() => _context.Set<T>().AsNoTracking();
    }
}
