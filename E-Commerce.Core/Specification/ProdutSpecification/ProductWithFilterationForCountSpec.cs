using E_Commerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specification.ProdutSpecification
{
    public  class ProductWithFilterationForCountSpec:BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpec(ProductSpecParam spec):base(p=>
        (spec.Search == null || p.Name.ToLower().Contains(spec.Search.ToLower()))
        && (string.IsNullOrEmpty(spec.CategoryId) || p.CategoryId == spec.CategoryId))

        {
            
        }
    }
}
