using Cash_Back.Models;
using Cash_Back.Models.ViewModel;
using Cash_Back.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Twilio.Rest.Verify.V2.Service;

namespace Cash_Back.Controllers.Api
{
    [Route("api/AccountSettings")]
    [ApiController]


    public class AccountSettingsController : Controller
    {
        readonly ApplicationDbContext _db;

        //the most important classes in creating users&controlling them
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _loginUserId;
        private readonly TwilioVerifySettings _settings;

        public AccountSettingsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor,IOptions<TwilioVerifySettings> settings)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _loginUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _settings = settings.Value;
        }

        public string PhoneNumber { get; set; }
        
        public IActionResult AccountSettings()
        {
            return View();
        }
       
        [HttpGet]
        [Route("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            CommonResponse<ApplicationUser> response = new CommonResponse<ApplicationUser>();

            //var User = new ApplicationUser();
            //CommonResponse<List<ApplicationUser>> response = new CommonResponse<List<ApplicationUser>>();
            try
            {
            //    User.Id = _loginUserId;
            //    var doctors = (from user in _db.Users
            //         .Where(userN => userN.Id == _loginUserId)
            //                   select new ApplicationUser
            //                   {
            //                       Id = user.Id,
            //                       Name = user.Name,
            //                       Email = user.Email,
            //                       PhoneNumber = user.PhoneNumber,
            //                       UserName = user.UserName,
            //                       Address = user.Address,
            //                       Gender = user.Gender,
            //                       Birthdate = user.Birthdate,
            //                       PasswordHash = user.PasswordHash,
            //                       PhoneNumberConfirmed = user.PhoneNumberConfirmed
            //                   }).ToList();



            //    var final = doctors[0];
            //    response.dataeum = doctors;

            var CurrentUser = _db.Users.FirstOrDefault(x => x.Id == _loginUserId);
                response.dataeum = CurrentUser;


                response.status = 1;
                return Json(response);
            }
            catch (Exception e)
            {

                response.massage = e.Message;
                response.status = 0;
            }
            return Ok(response);



        }



        [HttpPost]
        [Route("UpdateUserInfo")]
        public async Task<IActionResult> UpdateUserInfo(ApplicationUser model)
        {
            var User = new ApplicationUser();
            CommonResponse<List<ApplicationUser>> response = new CommonResponse<List<ApplicationUser>>();
            try
            {
                var CurrentUser = _db.Users.FirstOrDefault(x => x.Id == _loginUserId);

                CurrentUser.Name = model.Name;
                CurrentUser.PhoneNumber = model.PhoneNumber;
                CurrentUser.UserName = model.UserName;
                CurrentUser.Address = model.Address;
                CurrentUser.Gender = model.Gender;
                CurrentUser.Birthdate = model.Birthdate;
                _db.Update(CurrentUser);
                await _db.SaveChangesAsync();

                response.status = 1;
                response.massage = "Updated Successfully";

            }
            catch (Exception e)
            {

                response.massage = e.Message;
                response.status = 0;
            }
            return Ok(response);



        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var User = new ApplicationUser();
            CommonResponse<List<ApplicationUser>> response = new CommonResponse<List<ApplicationUser>>();
            try
            {
                var updateduser = await _db.Users.FindAsync(_loginUserId);
                if (updateduser != null)
                {
                    _db.Remove(updateduser);
                    await _signInManager.SignOutAsync();
                    await _db.SaveChangesAsync();
                    response.status = 1;

                }

            }
            catch (Exception e)
            {

                response.massage = e.Message;
                response.status = 0;
            }
            return Ok(response);



        }


        [HttpPost]
        [Route("ChangeCurrentPassword")]
        public async Task<IActionResult> ChangeCurrentPassword(ChangePasswordSettingVM model)
        {
            CommonResponse<List<ApplicationUser>> response = new CommonResponse<List<ApplicationUser>>();
            try
            {

                var CurrentUser = await _db.Users.FindAsync(_loginUserId);


                // ChangePasswordAsync changes the user password
                var result = await _userManager.ChangePasswordAsync(CurrentUser,
                    model.Password, model.NewPassword);
                await _signInManager.RefreshSignInAsync(CurrentUser);
                if (result.Succeeded)
                {
                    response.status = 1;
                }



            }
            catch (Exception e)
            {

                response.massage = e.Message;
                response.status = 0;
            }
            return Ok(response);



        }

        [HttpPost]
        [Route("PostNewItemMethod")]
        public async Task<IActionResult> AddnewItem(AddItemVM model)
        {

            CommonResponse<List<AddItemVM>> response = new CommonResponse<List<AddItemVM>>();

            try
            {
                var AddNewItem = _db.CardItems.FirstOrDefault(x => x.Id == model.Id);
                AddItemVM addtodata = new AddItemVM()
                {
                    Id = model.Id,
                    Title = model.Title,
                    BigImgUrl = model.BigImgUrl,
                    smallImgUrl = model.smallImgUrl,
                    Description = model.Description,
                    discount = model.discount

                };
                await _db.CardItems.AddAsync(addtodata);
                await _db.SaveChangesAsync();
                response.status = 1;
                response.massage = "New Item Card Created Successfully";


            }
            catch (Exception e)
            {
                response.massage = e.Message;
                response.status = 0;
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetCardData")]
        public async Task<IActionResult> GetCardData(int id)
        {
            IEnumerable<AddItemVM> ObjectList = _db.CardItems;

            CommonResponse<List<AddItemVM>> response = new CommonResponse<List<AddItemVM>>();

            try
            {

                var getcards = (from items in _db.CardItems
                                select new AddItemVM
                                {
                                    Id = items.Id,
                                    BigImgUrl = items.BigImgUrl,
                                    smallImgUrl = items.smallImgUrl,
                                    discount = items.discount,
                                    Description = items.Description,
                                    Title = items.Title

                                }).ToList();

                var newlsit = getcards;
                response.status = 1;
                response.dataeum = newlsit;


            }
            catch (Exception e)
            {
                response.massage = e.Message;
                response.status = 0;
            }
            return Ok(response);
        }


        [HttpGet]
        [Route("DeleteCardItem/{id}")]

        public async Task<IActionResult> DeleteCardItem(int id)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            try
            {
                var card = _db.CardItems.FirstOrDefault(x => x.Id == id);
                if (card != null)
                {
                    _db.CardItems.Remove(card);
                    await _db.SaveChangesAsync();
                    response.status = 1;
                    response.massage = "Deleted Successfully";
                }
            }
            catch (Exception e)
            {

                response.massage = e.Message;
                response.status = 0;
            }


            return Ok(response);
        }



        [HttpPost]
        [Route("updateModalItem/{id}")]
        public async Task<IActionResult> updateModalItem(AddItemVM model, int id)
        {

            CommonResponse<List<AddItemVM>> response = new CommonResponse<List<AddItemVM>>();
            try
            {
                var card = _db.CardItems.FirstOrDefault(x => x.Id == id);
                if (card != null)
                {


                    card.Id= id;
                    card.Title =model.Title;
                    card.Description =model.Description;
                    card.smallImgUrl =model.smallImgUrl;
                    card.BigImgUrl=model.BigImgUrl ;
                    card.discount=model.discount;



                    _db.Update(card);
                    await _db.SaveChangesAsync();

                    response.status = 1;
                    response.massage = "Updated Successfully";
                }
            }
            catch (Exception e)
            {

                response.massage = e.Message;
                response.status = 0;
            }
            return Ok(response);



        }

        [HttpGet]
        [Route("LoadPhoneNumber")]
        public async Task<IActionResult> OnGetAsync()
        {
            await LoadPhoneNumber();
            return Ok();
        }
        [HttpPost]
        [Route("VerifyPhoneNumber")]
        public async Task<IActionResult> OnPostAsync()
        {
            await LoadPhoneNumber();

            try
            {
                CommonResponse<int> response = new CommonResponse<int>();

                var verification = await VerificationResource.CreateAsync(
                    locale: "en",
                    to: PhoneNumber,
                    channel: "sms",
                    pathServiceSid: _settings.VerificationServiceSID
                );

                if (verification.Status == "pending")
                {
                    response.status = 1;
                    return Ok(response);

                    //return RedirectToAction("ConfirmPhone2");
                }


                ModelState.AddModelError("", $"There was an error sending the verification code: {verification.Status}");
            }
            catch (Exception)
            {
                ModelState.AddModelError("",
                    "There was an error sending the verification code, please check the phone number is correct and try again");
            }

            return Ok();
        }
        [HttpGet]
        [Route("LoadPhoneNumber2")]
        private async Task LoadPhoneNumber()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            PhoneNumber = user.PhoneNumber;
        }

        [HttpGet]
        [Route("ConfirmPhone2")]
        public async Task<IActionResult> ConfirmPhone2()
        {
            var user = await _userManager.GetUserAsync(User);
            PhoneNumber = user.PhoneNumber;
            @ViewBag.MobileNumber1 = PhoneNumber;
            return View();
        }

        
        [HttpPost]
        
        [Route("ConfirmPhone")]
        public async Task<IActionResult> ConfirmPhone()
        {
            await LoadPhoneNumber();
            return View();
        }

        [Route("onValidateNum")]
        public async Task<IActionResult> onValidateNum( VerificationCodeVM model)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            await LoadPhoneNumber();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
               
                
                var verification = await VerificationCheckResource.CreateAsync(
                    to: PhoneNumber,
                    code: model.VerificationCode,
                    pathServiceSid: _settings.VerificationServiceSID
                );
                if (verification.Status == "approved")
                {
                    var identityUser = await _userManager.GetUserAsync(User);
                    identityUser.PhoneNumberConfirmed = true;
                    var updateResult = await _userManager.UpdateAsync(identityUser);

                    if (updateResult.Succeeded)
                    {
                        response.status = 1;
                       return Ok(response);
                    }
                    else
                    {
                        response.status = 0;
                        response.massage = "There was an error confirming the verification code, please try again";
                        ModelState.AddModelError("", "There was an error confirming the verification code, please try again");
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"There was an error confirming the verification code: {verification.Status}");
                    response.massage = $"There was an error confirming the verification code: {verification.Status}";
                    response.status = 0;
                }
            }
            catch (Exception)
            {
                response.status = 0;
                response.massage = "There was an error confirming the code, please check the verification code is correct and try again";
                ModelState.AddModelError("",
                    "There was an error confirming the code, please check the verification code is correct and try again");
                
            }

            return Ok(response);


        }

    }
}
