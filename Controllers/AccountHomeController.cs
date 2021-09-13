using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cash_Back.Models;
using Cash_Back.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cash_Back.Controllers
{
   

    public class AccountHomeController : Controller

    {


        readonly ApplicationDbContext _db;

        //the most important classes in creating users&controlling them
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _loginUserId;

       
		public AccountHomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
		{
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _loginUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize]

        public IActionResult Index()
        {
            IEnumerable<AddItemVM> ObjectList = _db.CardItems;
            return View(ObjectList);
        }

        public IActionResult AccountSettings()
        {
            return View();
        }
    }
}
