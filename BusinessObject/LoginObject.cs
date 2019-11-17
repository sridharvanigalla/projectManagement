using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class LoginObject
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Enter Your User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter Your Password")]
        public string Password { get; set; }
    }
}
