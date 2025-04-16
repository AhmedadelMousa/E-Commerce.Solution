using E_Commerce.Core.Entities;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Repository.Contract
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _store;
        private Hashtable _repositorires;

        public UnitOfWork(StoreContext store)
        {
            _store = store;
            _repositorires = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
            return await _store.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
           await _store.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var Key = typeof(TEntity).Name;
            if(!_repositorires.ContainsKey(Key) )
            {
                var Repository= new GenericRepository<TEntity>(_store);
                _repositorires.Add(Key, Repository);
            }
            return _repositorires[Key] as IGenericRepository<TEntity>;
        }
    }
}
