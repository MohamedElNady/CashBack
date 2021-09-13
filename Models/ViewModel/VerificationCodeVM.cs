using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cash_Back.Models.ViewModel
{
    public class VerificationCodeVM
    {
        [ Required]
        public string VerificationCode { get; set; }
    }
}
