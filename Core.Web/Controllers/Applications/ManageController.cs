using AutoMapper;
using Core.Web.Controllers;
using Core.Web.Helpers;
using Core.Web.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyWeb.Models;
using MyWeb.ViewModels.Applications;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Controllers
{
    [MyAuthorize]
    public class ManageController : CoreControllerBase
    {
        SqlHelper query = new SqlHelper("UserConnection");

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //public async Task<ActionResult> ChangeProfile()
        //{
        //    ApplicationUser applicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //    if (applicationUser != null)
        //    {
        //        return View(applicationUser);
        //    }
        //    return RedirectToAction("Index", new { Message = ManageMessageId.Error });
        //}

        public async Task<ActionResult> ChangeProfile()
        {
            var model = Mapper.Map<ProfileViewModel>(
                await UserManager.FindByIdAsync(User.Identity.GetUserId()));

            return View(model);
        }       

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProfile(ProfileViewModel applicationUser)//ApplicationUser applicationUser)
        {
            ManageMessageId manageMessageId;

            ApplicationUser retrievedApplicationUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            retrievedApplicationUser.FirstName = applicationUser.FirstName;
            retrievedApplicationUser.LastName = applicationUser.LastName;
            if (!string.IsNullOrEmpty(retrievedApplicationUser.FirstName))
            {
                retrievedApplicationUser.FullName = retrievedApplicationUser.FirstName + " " + retrievedApplicationUser.LastName ?? string.Empty;
            }
            retrievedApplicationUser.Email = applicationUser.Email;
            retrievedApplicationUser.EmailConfirmed = true;
            //retrievedApplicationUser.Address = applicationUser.Address;
            //retrievedApplicationUser.City = applicationUser.City;
            //retrievedApplicationUser.State = applicationUser.State;
            //retrievedApplicationUser.ZipCode = applicationUser.ZipCode;

            // Update the Profile
            var result = await UserManager.UpdateAsync(retrievedApplicationUser);
            if (result.Succeeded)
            {
                manageMessageId = ManageMessageId.ChangeProfileSuccess;

                //// If the Email address changed, sync both Email and UserName to it
                //if (retrievedApplicationUser.Email != applicationUser.Email)
                //{
                //    // Update the UserName
                //    string previousUserName = retrievedApplicationUser.UserName;
                //    retrievedApplicationUser.UserName = applicationUser.Email;
                //    result = await UserManager.UpdateAsync(retrievedApplicationUser);
                //    if (result.Succeeded)
                //    {
                //        // Update the Email address
                //        result = await UserManager.SetEmailAsync(retrievedApplicationUser.Id, applicationUser.Email);
                //        if (result.Succeeded)
                //        {
                //            manageMessageId = ManageMessageId.ChangeEmailSuccess;

                //            //string code = await UserManager.GenerateEmailConfirmationTokenAsync(retrievedApplicationUser.Id);

                //            //var callbackUrl = Url.Action(
                //            //    "ConfirmEmail",
                //            //    "Account",
                //            //    new { userId = retrievedApplicationUser.Id, code = code }, protocol: Request.Url.Scheme);

                //            //await UserManager.SendEmailAsync(
                //            //    retrievedApplicationUser.Id,
                //            //    "Confirm your account",
                //            //    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                //        }
                //        else
                //        {
                //            retrievedApplicationUser.UserName = previousUserName;
                //            result = await UserManager.UpdateAsync(retrievedApplicationUser);
                //            manageMessageId = ManageMessageId.Error;
                //        }
                //    }
                //    else
                //    {
                //        retrievedApplicationUser.UserName = previousUserName;
                //        result = await UserManager.UpdateAsync(retrievedApplicationUser);
                //        manageMessageId = ManageMessageId.Error;
                //    }
                //}
            }
            else
                manageMessageId = ManageMessageId.Error;

            return RedirectToAction("Index", new { Message = manageMessageId });
            //return JsonSuccess(true);            
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.ChangeProfileSuccess ? "Your profile has been changed."
                : message == ManageMessageId.ChangeEmailSuccess ? "Check your email."
                : "";

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId()),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId()),
                Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId()),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId())
            };
            return View(model);
        }               

        //
        // GET: /Manage/RemoveLogin
        public ActionResult RemoveLogin()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        [AllowAnonymous]
        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword(string username, string message, string returnUrl)
        {            
            if (!string.IsNullOrEmpty(username))
            {
                ViewBag.Username = username;
            }
            else if (User.Identity.IsAuthenticated)
            {
                ViewBag.Username = User.Identity.GetUserName();
            }

            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }           

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model, string returnUrl)
        {
            var username = (!string.IsNullOrEmpty(User.Identity.GetUserName())) ? User.Identity.GetUserName() : model.Username ?? string.Empty;
            ViewBag.Username = username;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            //var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    
            SqlParameter OutputParam = new SqlParameter("@cMessage", SqlDbType.VarChar, 50);
            OutputParam.Direction = ParameterDirection.Output;

            try
            {                
                await query.ExecNonQueryProcAsync("UserChangePassword"
                    , "@cUserName", username
                        , "@cOldPassword", model.OldPassword
                        , "@cNewPassword", model.NewPassword
                        //, OutputParam
                     );

                if (OutputParam.Value != null && OutputParam.Value != DBNull.Value && !string.IsNullOrEmpty((string)OutputParam.Value))
                {
                    string message = (string)OutputParam.Value;
                    ModelState.AddModelError("", message);
                }
                else
                {
                    ApplicationUser user = null;
                    if(!string.IsNullOrEmpty(User.Identity.GetUserId())){
                        user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    }else{
                        user = await UserManager.FindByNameAsync(model.Username ?? string.Empty);
                    }

                    if (user != null)
                    {
                        //await SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Login", "Account", new { message = "Change Password Success! Silahkan Login", returnUrl = returnUrl });
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
            }
            catch (Exception ex)
            {
               ModelState.AddModelError("", ex.Message);   
            }            

            //if (result.Succeeded)
            //{
            //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            //    if (user != null)
            //    {
            //        await SignInAsync(user, isPersistent: false);
            //    }
            //    return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            //}

            //AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error,
            ChangeProfileSuccess,
            ChangeEmailSuccess
        }

#endregion
    }
}