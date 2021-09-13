using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cash_Back.Models.ViewModel
{
    public class ForgetPassword
    {
        
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        
    }
}
