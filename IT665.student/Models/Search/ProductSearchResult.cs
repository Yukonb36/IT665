using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT665.Models.Search
{
    public class ProductSearchResult
    {
        public string Name { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public int ProductId { get; set; } = -1;
    }
}