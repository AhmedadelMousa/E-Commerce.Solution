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

        public int PageSize { set; get; } = 10;
        public int PageIndex { get; set; } = 1;
        public string Search { get; set; } = string.Empty;
        public DateTime? DateFrom { set; get; }
        public DateTime? DateTo { set; get; }
    }
}
