using Cash_Back.Models;
using Cash_Back.Models.ViewModel;
using Cash_Back.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cash_Back.Controllers
{
	public class AccountController : Controller
	{
		readonly ApplicationDbContext _db;

		//the most important classes in creating users&controlling them
		 UserManager<ApplicationUser> _userManager;
		SignInManager<ApplicationUser> _signInManager;
		RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public string UserId { get; set; }
		public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		RoleManager<IdentityRole> roleManager, IEmailSender emailSender , IHttpContextAccessor httpContextAccessor )
		{
			_db = db;
			_userManager= userManager;
			_signInManager= signInManager;	
			_roleManager= roleManager;
			_emailSender = emailSender;
			_httpContextAccessor = httpContextAccessor;
		
		}

		[HttpGet]
		public async Task<IActionResult> Login(string returnUrl)
		{
			//setup login with providers
			LoginVM model = new LoginVM
			{
				ReturnUrl = returnUrl,
				ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
		};
			var t = returnUrl;

			ViewBag.newUserSWL = TempData["newUserSWL"] as string;
			return View(model);
		}

		//POST
		[HttpPost]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public async Task<IActionResult> Login(LoginVM model)
		{

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);

				if (user != null && !user.EmailConfirmed && await _userManager.CheckPasswordAsync(user, model.Password))
				{
					//send value to viewbag to digger to event
					ViewBag.ConfrimEmail = "sss";

					model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

					ModelState.AddModelError("", "Email not confirmed yet");

					return View(model);
				}

				var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

				if (result.Succeeded)
				{

					return RedirectToAction("Index", "AccountHome");
				}
				ModelState.AddModelError("", "Invalid login attempt");
			}
			LoginVM retuenvm = new LoginVM
			{
				Email = model.Email,
				ReturnUrl = model.ReturnUrl,
				Password = model.Password,
				ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
			};


			return View(retuenvm);
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult ExternalLogin(	string provider, string returnUrl)
        {
			var redirectUrl = Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = returnUrl });
			var properties= _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

			return new ChallengeResult(provider, properties);
        }


		[AllowAnonymous]
		public async Task<IActionResult>
					ExternalLoginCallback(string returnUrl = null, string remoteError = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");

            LoginVM loginViewModell = new LoginVM
			{
				ReturnUrl = returnUrl,
				ExternalLogins =
						(await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
			};

			if (remoteError != null)
			{
				ModelState
					.AddModelError(string.Empty, $"Error from external provider: {remoteError}");

				return View("Login", loginViewModell);
			}

			// Get the login information about the user from the external login provider
			var info = await _signInManager.GetExternalLoginInfoAsync();
			if (info == null)
			{
				ModelState
					.AddModelError(string.Empty, "Error loading external login information.");

				return View("Login", loginViewModell);
			}

			// If the user already has a login (i.e if there is a record in AspNetUserLogins
			// table) then sign-in the user with this external login provider
			var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
				info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

			
			if (signInResult.Succeeded)
			{
				
				return RedirectToAction("SetPassword");

			//	return RedirectToAction("Register","Account");
              
               // return LocalRedirect(returnUrl);
			}
			// If there is no record in AspNetUserLogins table, the user may not have
			// a local account
			else
			{
				// Get the email claim value
				var email = info.Principal.FindFirstValue(ClaimTypes.Email);

				if (email != null)
				{
					// Create a new user without password if we do not have a user already
					var user = await _userManager.FindByEmailAsync(email);

					if (user == null)
					{
						user = new ApplicationUser
						{
							UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
							Email = info.Principal.FindFirstValue(ClaimTypes.Email),
							Name = info.Principal.FindFirstValue(ClaimTypes.Name),
							Role = "User",
							Address = info.Principal.FindFirstValue(ClaimTypes.Country),

							EmailConfirmed = true
							
						};

						await _userManager.CreateAsync(user);
						await _userManager.AddToRoleAsync(user, "User");

					}

					// Add a login (i.e insert a row for the user in AspNetUserLogins table)
					await _userManager.AddLoginAsync(user, info);
					await _signInManager.SignInAsync(user, isPersistent: false);
					TempData["PasswordSetBag"] = "sss";
					return RedirectToAction(nameof(SetPassword));
				//	return LocalRedirect(returnUrl);
				}

				// If we cannot find the user email we cannot continue
				ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
				ViewBag.ErrorMessage = "Please contact support on Mohamedashash27@gmail.com";

				return View("Error");
			}
		}

		//Set Password for externalLogin Account
		// Return View
		[HttpGet]
		public async Task<IActionResult> SetPassword ()
		{
			ViewBag.PasswordSetBag = TempData["PasswordSetBag"] as string;

			var user = await _userManager.GetUserAsync(User);
			var userHasPassword = await _userManager.HasPasswordAsync(user);
				if (!userHasPassword)
            {
				return View();
            }
			return RedirectToAction("Index","AccountHome");
        }
		[HttpPost]
		public async Task<IActionResult> SetPassword(SetLocalPassword model)
		{
            if (ModelState.IsValid)
            {
				var userr = await _userManager.GetUserAsync(User);

				var result = await _userManager.AddPasswordAsync(userr, model.ConfirmPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
						ModelState.AddModelError(string.Empty,error.Description);

                    }
					return View();
                }
				await _signInManager.RefreshSignInAsync(userr);
				ViewBag.PasswordSetBag = "sss";

				TempData["PasswordSetBag"] = "sss";
				return RedirectToAction("Index", "AccountHome");


			}
			return View(model);
			
		}


		

		//Get ForgetPassword
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ForgetPassword()
		{
			ViewBag.ForgetPassword = TempData["ForgetPassword"] as string;
			return View();
		}
		//ForgetPassword
		[HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPassword model)
		{
			if (ModelState.IsValid)
			{
				// Find the user by email
				var user = await _userManager.FindByEmailAsync(model.Email);
				// If the user is found AND Email is confirmed
				if (user != null && await _userManager.IsEmailConfirmedAsync(user))
				{
					// Generate the reset password token
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);

					// Build the password reset link
					var passwordResetLink = Url.Action("ResetPassword", "Account",
							new { email = model.Email, token = token }, Request.Scheme);

					//Send Email to get the new Link 
					await _emailSender.SendEmailAsync(user.Email, "Welcome This Is Reset Password Email",  "Welcom In Cash Back APP" +
					$"<a href=\"{passwordResetLink}\">Change Password</a>");

					TempData["ForgetPassword"] = "sss";
					
					// Send the user to Forgot Password Confirmation view
					return View(model);
				}

				// To avoid account enumeration and brute force attacks, don't
				// reveal that the user does not exist or is not confirmed
				return View(model);
			}

			return View(model);

		}

		//Get ResendPasswordView
		[HttpGet]
		[AllowAnonymous]
		public IActionResult ResetPassword(string token, string email)
		{
			// If password reset token or email is null, most likely the
			// user tried to tamper the password reset link
			if (token == null || email == null)
			{
				ModelState.AddModelError("", "Invalid password reset token");
			}
			return View();
		}
		//ResendPassword
		public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
		{
			if (ModelState.IsValid)
			{
				// Find the user by email
				var user = await _userManager.FindByEmailAsync(model.Email);

				if (user != null)
				{
					// reset the user password
					var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
					if (result.Succeeded)
					{
						
						TempData["PasswordSuccess"] = "sss";
						return RedirectToAction("login");
					}
					// Display validation errors. For example, password reset token already
					// used to change the password or password complexity rules not met
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					return View(model);
				}

				// To avoid account enumeration and brute force attacks, don't
				// reveal that the user does not exist
				return View("Login");
			}
			// Display validation errors if model state is not valid
			return View(model);

		}


		public  async Task<IActionResult> Logoff()
		{
			await _signInManager.SignOutAsync();
			
			
			return RedirectToAction("Index", "Home");
		}
		public IActionResult Register()
		{
			
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				var	NewUser = new ApplicationUser();
				NewUser.Name = model.Name;
				NewUser.UserName = model.UserEmail;
				NewUser.Email = model.UserEmail;
				NewUser.Role = "User";

				var result =await _userManager.CreateAsync(NewUser, model.Password);


				


				if (result.Succeeded)
				{
					//await _userManager.AddToRoleAsync(NewUser, "User");				
					//   await _emailSender.SendEmailAsync(NewUser.UserName, "Welcome Confiramtion Email", "Welcom In Cash Back APP");

					//if (!User.IsInRole("Admin"))
					//{
					//await _signInManager.SignInAsync(NewUser, isPersistent: false);
					//	TempData["newUserSWL"] = "Thank You "+NewUser.Name+" ,"+"Confirmation Email Send, Please Confirm then login";
					//}
					//else
					//{
					//	TempData["newAdminSignUp"] = NewUser.Name;
					//}
					
					await _userManager.AddToRoleAsync(NewUser, "User");
					
					var code = await _userManager.GenerateEmailConfirmationTokenAsync(NewUser);

					var link = Url.Action(nameof(vrefiedAccount),"Account",new { userId=NewUser.Id,code}, Request.Scheme, Request.Host.ToString());

					await _emailSender.SendEmailAsync(NewUser.UserName, "Welcome This Is Confiramtion Email", "Welcom In Cash Back APP"+
						$"<a href=\"{link}\">Verify Email</a>");

					TempData["UserIDD"] = NewUser.Id;

					TempData["UserEmail"] = NewUser.UserName;



					return RedirectToAction("VerifyEmailLayer");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
					

				}
			}
			return View(model);
		}

		public async Task<IActionResult> vrefiedAccount(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) { BadRequest(); }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
			
				TempData["newUserSWL"] = "Thanks";
				return RedirectToAction("Login", "Account");
            }
            else
            {
                return BadRequest();
            }     

    }

		

		public  IActionResult VerifyEmailLayer()
		{
			ViewBag.UserEmail = TempData["UserEmail"] as string;
				
			return View();
		}
		//Post
		[HttpPost]
		public async Task<IActionResult> VerifyEmailLayer(EmailConfirmVM email)
		{
			if (ModelState.IsValid)
			{
				var User = new ApplicationUser();
				var emailconfirm = new EmailConfirmVM();
				emailconfirm.EmailConfrirm = email.EmailConfrirm;
				User.Email = email.EmailConfrirm;
				var emailll = User.Email;


				var doctors = (from user in _db.Users
					 .Where(userN => userN.Email == emailll)

							   select new ApplicationUser
							   {
								
								   Id = user.Id,
								   Name = user.Name,
								   Email=user.Email
								   
							   }).ToList();


				var resultt = await _userManager.UpdateSecurityStampAsync(doctors[0]);

				var code2 = await _userManager.GenerateEmailConfirmationTokenAsync(doctors[0]);

				var link = Url.Action(nameof(vrefiedAccount), "Account", new { userId = doctors[0].Id, code2 }, Request.Scheme, Request.Host.ToString());

				await _emailSender.SendEmailAsync(doctors[0].Email, "Welcome This Is Confiramtion Email", "Welcom In Cash Back APP" +
					$"<a href=\"{link}\">Verify Email</a>");

				return View(email);
			}

				ModelState.AddModelError("", "Invalid login attempt");


				
			return View(email);
		}

		
	}
} 
