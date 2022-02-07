using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT665.Models.Login
{
    public class Login
    {
        [RegularExpression("[A-Za-z]+", ErrorMessage ="User Name accepts only letters")]
        [Required(ErrorMessage="User Name is required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "User Name must be between 3 and 25 letters")]
        public string UserName{ get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 25 letters")]
        public string Password { get; set; }


        public bool LoginFailed{ get; set; }
    }
}