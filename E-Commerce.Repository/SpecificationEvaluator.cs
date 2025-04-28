using E_Commerce.Core.Entities;
using E_Commerce.Core.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public class SpecificationEvaluator <TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> innerQuery,ISpecification<TEntity> spec)
        {

            var query = innerQuery; 
                                    // if there is where condition
            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);//_store.Set<product>().Where(P=>P.Id==id)
            }
            if (spec.DateFiltrationCriteria is not null)
            {
                query = query.Where(spec.DateFiltrationCriteria);
            }
            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            

            query = spec.Includes.Aggregate(query, (CurrentQuery, includeExpression) => CurrentQuery.Include(includeExpression));
            return query;
        }
    }
}
