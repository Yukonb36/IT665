using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT665.Models.Search
{
    public class ProductSearch
    {
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Product Search field accepts only letters")]
        [Required(ErrorMessage = "Part of Product Name is required.")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Part Of Product Name field must be between 3 and 25 letters")]
        public string PartialProductName { get; set; } = string.Empty;
        public IList<ProductSearchResult> Results { get; set; }
    }
}