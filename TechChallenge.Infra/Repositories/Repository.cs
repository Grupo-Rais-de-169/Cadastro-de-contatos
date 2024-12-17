using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Infra.Context;

namespace TechChallenge.Infra.Repositories
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

        public async Task<T> GetByIdAsync(int id) => await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

        public IList<T> GetAll() => _dbSet.ToList();

        public async Task<IList<T>> GetAllAsync() => await _dbSet.ToListAsync();
       
        [ExcludeFromCodeCoverage]
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
       
        [ExcludeFromCodeCoverage]
        public void SaveChanges() => _context.SaveChanges();
       
        [ExcludeFromCodeCoverage]
        public IQueryable<T> GetAllAsNoTracking() => _context.Set<T>().AsNoTracking();
    }
}