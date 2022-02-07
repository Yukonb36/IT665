using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT665.Models.Search
{
    public class CustomerSearch
    {
        [RegularExpression("[A-Za-z]+", ErrorMessage ="Part of Last Name field accepts only letters")]
        [Required(ErrorMessage="Part of Last Name field is required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Part Of Last Name field must be between 3 and 25 letters")]
        public string PartialLastName { get; set; }
        public IList<CustomerSearchResult> Results { get; set; }
    }
}