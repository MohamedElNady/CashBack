using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cash_Back.Models.ViewModel
{
    public class EmailConfirmVM 
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailConfrirm { get; set; }
    }
}
