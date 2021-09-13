using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cash_Back.Controllers
{
    public class ConfirmPhone : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
