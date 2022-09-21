using Core.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MyWeb.DataLayer;
using MyWeb.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Twilio;

namespace MyWeb
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            const string userName = "";
            const string from = "";
            const string password = "";
            const int port = 587;

            var smtpClient = new SmtpClient("smtp.gmail.com", port);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(userName, password);

            var mailMessage = new MailMessage(from, message.Destination, message.Subject, message.Body);
            //ERROR Gmail Error :The SMTP server requires a secure connection or the client was not authenticated. The server response was: 5.5.1 Authentication Required
            //http://stackoverflow.com/questions/20906077/gmail-error-the-smtp-server-requires-a-secure-connection-or-the-client-was-not
            //http://stackoverflow.com/questions/1311749/c-sharp-smtpclient-class-not-able-to-send-email-using-gmail
            return smtpClient.SendMailAsync(mailMessage);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            const string accountSid = "ACaba493016601b41636ae1d3fb6fd63c4";
            const string authToken = "cad99a78cdd84c78dd6e6f00caa0eaf6";
            const string phoneNumber = "+6281289672888";

            var twilioRestClient = new TwilioRestClient(accountSid, authToken);
            twilioRestClient.SendSmsMessage(phoneNumber, message.Destination, message.Body);
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        SqlHelper query = new SqlHelper("UserConnection");

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public ApplicationUser GetUser(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ApplicationUser user = this.FindById(id);
                return user;
            }
            return null;
        }
        public string GetUserName(string id)
        {
            ApplicationUser user = GetUser(id);
            if (user != null)
            {
                return user.UserName ?? string.Empty;
            }
            return string.Empty;
        }
        public string GetFullName(string id)
        {
            ApplicationUser user = GetUser(id);
            if (user != null)
            {
                return user.FullName ?? string.Empty;
            }
            return string.Empty;
        }
        public int? GetBranch(string id)
        {
            return GetUser(id).BranchId;           
        }
        public int? GetCompany(string id)
        {
            return GetUser(id).CompanyId;
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                //http://stackoverflow.com/questions/21057304/why-is-usermanager-find-returning-null-if-my-username-is-email-based
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(1);
            manager.MaxFailedAccessAttemptsBeforeLockout = 3;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        //http://stackoverflow.com/questions/31559088/build-a-custom-user-check-password-in-asp-net-identity-2
        //public override async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        //{
        //    return await Task.Run(() =>
        //    {     
        //        return true;
        //    });
        //}
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        SqlHelper query = new SqlHelper("UserConnection");
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        private async Task<CoreSignInStatus> CheckPasswordAsync(string username, string password)
        {
            SqlParameter OutputParam = new SqlParameter("@cMessage", SqlDbType.VarChar, 50);
            OutputParam.Direction = ParameterDirection.Output;

            await query.ExecNonQueryProcAsync("WebLogin"
                , "@cUserName", username
                , "@cPassword", password
                , OutputParam);

            if (OutputParam.Value != null && OutputParam.Value != DBNull.Value && !string.IsNullOrEmpty((string)OutputParam.Value))
            {
                string message = (string)OutputParam.Value;
                if(message.ToLower() == "user_tidak_terdaftar")
                    return CoreSignInStatus.NotRegistered;
                else if(message.ToLower() == "User_tidak_aktif")
                    return CoreSignInStatus.InActive;
                else if(message.ToLower() == "user_expired")
                    return CoreSignInStatus.Expired;
                else if(message.ToLower() == "password_salah")
                    return CoreSignInStatus.IncorrectPassword;               
            }

            return CoreSignInStatus.Success;
        }


        //SignInManager wrapper class
        //http://stackoverflow.com/questions/25551295/custom-asp-net-identity-2-0-userstore-is-implementing-all-interfaces-required        
        private async Task<CoreSignInStatus> SignInOrTwoFactor(ApplicationUser user, bool isPersistent)
        {
            var id = Convert.ToString(user.Id);

            if (UserManager.SupportsUserTwoFactor
                && await UserManager.GetTwoFactorEnabledAsync(user.Id)
                                    //.WithCurrentCulture()
                && (await UserManager.GetValidTwoFactorProvidersAsync(user.Id)).Count > 0
                                     //.WithCurrentCulture()).Count > 0
                    && !await AuthenticationManager.TwoFactorBrowserRememberedAsync(id))
                                                   //.WithCurrentCulture())
            {
                var identity = new ClaimsIdentity(
                    DefaultAuthenticationTypes.TwoFactorCookie);

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));

                AuthenticationManager.SignIn(identity);
                
                return CoreSignInStatus.RequiresVerification;
            }
            await SignInAsync(user, isPersistent, false);//.WithCurrentCulture();

            //federated authentication
            //FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(GetSecurityTokenForFormsAuthentication(id, user.UserName));
            //federated authentication

            return CoreSignInStatus.Success;
        }        

        //single sign on forms federated authentication        
        //FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(GetSecurityTokenForFormsAuthentication(id, user.UserName));
        //single sign on forms federated authentication
        //http://chris.59north.com/post/Claims-based-identities-in-ASPNET-MVC-45-using-the-standard-ASPNET-providers
        private static SessionSecurityToken GetSecurityTokenForFormsAuthentication(string id, string username)
        {
            var claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, id), 
                            new Claim(ClaimTypes.Name, username)
                        };
            var identity = new ClaimsIdentity(claims, "Forms");
            var principal = new ClaimsPrincipal(identity);

            return new SessionSecurityToken(principal);
        }
        //private static SessionSecurityToken GetSecurityTokenForMembershipUser(MembershipUser user)
        //{
        //    var claims = new List<Claim>
        //                {
        //                    new Claim(ClaimTypes.NameIdentifier, user.Email), 
        //                    new Claim(ClaimTypes.Name, user.UserName)
        //                };

        //    var profile = ProfileBase.Create(user.UserName, true);
        //    foreach (SettingsProperty property in ProfileBase.Properties)
        //    {
        //        claims.Add(new Claim(property.Attributes["CustomProviderData"].ToString(), profile[property.Name].ToString()));
        //    }

        //    var identity = new ClaimsIdentity(claims, "Forms");
        //    var principal = new ClaimsPrincipal(identity);

        //    return new SessionSecurityToken(principal);
        //}


        //SignInManager wrapper class
        //http://stackoverflow.com/questions/25551295/custom-asp-net-identity-2-0-userstore-is-implementing-all-interfaces-required
        public async Task<CoreSignInStatus> PasswordSignInAsync2(
        string userName,
        string password,
        bool isPersistent,
        bool shouldLockout)
        {
            if (UserManager == null)
            {
                return CoreSignInStatus.Failure;
            }

            var user = await UserManager.FindByNameAsync(userName); //.WithCurrentCulture();
            if (user == null)
            {
                return CoreSignInStatus.Failure;
            }

            if (UserManager.SupportsUserLockout
                && await UserManager.IsLockedOutAsync(user.Id)) //.WithCurrentCulture())
            {
                return CoreSignInStatus.LockedOut;
            }

            CoreSignInStatus checkPasswordAsync = await CheckPasswordAsync(user.UserName, password);
            if(checkPasswordAsync != CoreSignInStatus.Success)
            {
                return checkPasswordAsync;
            }

            if (UserManager.SupportsUserPassword)
                //&& await UserManager.CheckPasswordAsync(user, password)
                //                .WithCurrentCulture())
            {
                return await SignInOrTwoFactor(user, isPersistent);//.WithCurrentCulture();
            }

            if (shouldLockout && UserManager.SupportsUserLockout)
            {
                // If lockout is requested, increment access failed count
                // which might lock out the user
                await UserManager.AccessFailedAsync(user.Id);//.WithCurrentCulture();
                if (await UserManager.IsLockedOutAsync(user.Id))//.WithCurrentCulture())
                {
                    return CoreSignInStatus.LockedOut;
                }
            }
            return CoreSignInStatus.Failure;
        }
        
    }

    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole,string> iRolesStore)
            : base(iRolesStore)
        {

        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context){

            return new ApplicationRoleManager(new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));
        }
    }
}
