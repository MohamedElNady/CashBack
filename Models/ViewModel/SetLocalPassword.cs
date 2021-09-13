using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cash_Back.Models.ViewModel
{
    public class SetLocalPassword
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="New Password")]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage ="InValid Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="Passwords should be the same")]
        public string ConfirmPassword {  get; set; }    
    }
}
