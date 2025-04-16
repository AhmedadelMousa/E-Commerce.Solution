using E_Commerce.Core.Entities;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Specification;
using E_Commerce.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Repository.Contract
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _store;

        public GenericRepository( StoreContext store)
        {
           _store = store;
        }
        public async Task AddAsync(T entity)
        {
          await _store.AddAsync(entity);
        }

        public async void DeleteAsync(T entity)
        {
           _store.Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _store.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _store.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _store.Set<T>().FindAsync(id);
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> GetWithSpecAsync(ISpecification<T> spec)
        {
           return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public void UpdateAsync(T entity)
        {
            _store.Update(entity);
        }
        //to avoid the repeat code use this method
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_store.Set<T>(), spec);
        }
    }
}
