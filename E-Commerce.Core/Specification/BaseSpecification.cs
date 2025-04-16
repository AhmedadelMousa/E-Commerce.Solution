using E_Commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set ; }
        public List<Expression<Func<T, object>>> Includes { get ; set ; }=  new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get ; set; }
        public Expression<Func<T, object>> OrderByDesc { get ; set ; }
        public int Take { get ; set ; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get ; set; }
        public BaseSpecification()
        {
            // Criteria= null
            //  Includes = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecification(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
            //  Includes=new List<Expression<Func<T, object>>>();

        }
        public void AddOrderBy(Expression<Func<T, object>> expression)//Just Setter for OrderBy
        {
            OrderBy = expression;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> expression)
        {
            OrderByDesc = expression;
        }
        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Take = take;
            Skip = skip;
        }
    }
}
