using E_Commerce.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specification.ProdutSpecification
{
    public class ProductWithCategorySpec : BaseSpecification<Product>
    {
        public ProductWithCategorySpec(ProductSpecParam spec) : base(p =>
        (spec.Search == null || p.Name.ToLower().Contains(spec.Search.ToLower()))
        && (string.IsNullOrEmpty(spec.CategoryId) || p.CategoryId == spec.CategoryId)
        )
        {
            if (!string.IsNullOrEmpty(spec.sort))
            {
                switch (spec.sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);// default return based on name
            }

            ApplyPagination((spec.PageIndex - 1) * spec.PageSize, spec.PageSize);//(Skip,Take)
            if (spec.DateFrom.HasValue)
                ApplyDateFiltration(spec.DateFrom.Value, spec.DateTo);
            else 
                ApplyDateFiltration(DateTime.MinValue, DateTime.MaxValue);
        }

        public ProductWithCategorySpec(string id) : base(P => P.Id == id)
        {
            Includes.Add(p => p.Reviews);
        }
    }
}
