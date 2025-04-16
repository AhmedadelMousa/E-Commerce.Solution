using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specification.ProdutSpecification
{
    public class ProductSpecParam
    {
        public string? sort { get; set; }
        public string? CategoryId { get; set; }

        private int pageSize;
        private const int MaxPageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public int PageIndex { get; set; } = 1;
        public string? Search { get; set; }
    }
}
