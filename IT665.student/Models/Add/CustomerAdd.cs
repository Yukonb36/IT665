using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT665.Models.Add
{
    public class CustomerAdd
    {
        public string nameStyle { get; set; }
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Title field accepts only letters")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "First Name field must be between 2 and 10 letters")]
        public string title { get; set; }
        [RegularExpression("[A-Za-z]+", ErrorMessage ="First Name field accepts only letters")]
        [Required(ErrorMessage="First Name field is required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "First Name field must be between 3 and 25 letters")]
        public string firstName { get; set; }
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Middle Name field accepts only letters")]
        [StringLength(25, MinimumLength = 0, ErrorMessage = "Middle name cannot be longer than 25 letters")]
        public string middleName { get; set; }
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Last Name field accepts only letters")]
        [Required(ErrorMessage = "Last Name field is required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Last Name field must be between 3 and 25 letters")]
        public string lastName { get; set; }
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Suffix field accepts only letters")]
        [StringLength(10, MinimumLength = 0, ErrorMessage = "Suffix field must be between 0 and 10 letters")]
        public string suffix { get; set; }
        public string emailPromotion { get; set; }
        public IList<CustomerAddResults> Results { get; set; }
    }
}