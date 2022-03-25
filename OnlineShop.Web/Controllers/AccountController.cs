using BotDetect.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OnlineShop.Common;
using OnlineShop.Model.Models;
using OnlineShop.Web.App_Start;
using OnlineShop.Web.infrastructure.Custom;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = _userManager.Find(model.UserName, model.Password);
                if(user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = model.RememberMe;
                    authenticationManager.SignIn(props, identity);

                    if(Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                    ViewBag.Message = "Tên đăng nhập hoặc mật khẩu không đúng.";
                }
            }

            return View(model);
        }


        [HttpPost]
        //[CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã xác nhận không đúng")]
        [ValidateGoogleCaptcha]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {

            if(ModelState.IsValid)
            {
                var userByEmail = await _userManager.FindByEmailAsync(model.Email);
                if(userByEmail != null)
                {
                    ModelState.AddModelError("email", "Email đã tồn tại");
                    return View(model);
                }
                var userByUserName = await _userManager.FindByNameAsync(model.UserName);
                if (userByUserName != null)
                {
                    ModelState.AddModelError("UserName", "Tài khoản đã tồn tại");
                    return View(model);
                }
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address

                };

                await _userManager.CreateAsync(user, model.PassWord);

                var adminUser = await _userManager.FindByEmailAsync(model.Email);
                if (adminUser != null)
                    await _userManager.AddToRolesAsync(adminUser.Id, new string[] { "SeenProduct" });

                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/newuser.html"));
                content = content.Replace("{{UserName}}", adminUser.FullName);
                content = content.Replace("{{Link}}", ConfigHelper.currentLink + "dang-nhap.html");

                MailHelper.SendMail(adminUser.Email, "Đăng ký thành công", content);

                ViewData["SuccessMsg"] = "Đăng ký thành công";

            }

            return View();
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //Đăng nhập bằng mạng xã ho

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {

            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private const string XsrfKey = "XsrfId";
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        public ActionResult EditUser()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = _userManager.FindById(currentUserId);  //.Users.FirstOrDefault(x => x.Id == currentUserId);


            EditUserViewModel user = new EditUserViewModel();

            //UserEdit user = new UserEdit();

            user.FullName = currentUser.FullName;
            user.Email = currentUser.Email;
            user.Address = currentUser.Address;
            user.PhoneNumber = currentUser.PhoneNumber;
            user.UserName = currentUser.UserName;
          
            return View(user);
        }

        [HttpPost]
        [ValidateGoogleCaptcha]
        public async Task<ActionResult> EditUser(EditUserViewModel model)
        {

            ViewBag.Message = "";
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser currentUser = _userManager.FindByEmail(model.Email);

            ApplicationUser user = _userManager.Find(currentUser.UserName ,model.PassWordLast);

            if (user == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                ViewBag.Message = "Mật khẩu không chính xác";

                return View(model);
            }    

            currentUser.FullName = model.FullName;
            currentUser.Address = model.Address;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.UserName = model.UserName;

            currentUser.PasswordHash = _userManager.PasswordHasher.HashPassword(model.PassWordNew);//Thay đổi mật khẩu

            await _userManager.UpdateAsync(currentUser);

            string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/newuser.html"));
            content = content.Replace("{{UserName}}", currentUser.FullName);
            content = content.Replace("{{Link}}", ConfigHelper.currentLink + "dang-nhap.html");

            MailHelper.SendMail(currentUser.Email, "Sửa đổi tài khoản thành công", content);

            ViewData["SuccessMsg"] = "Sửa đổi thành công";

            return View(model);
        }

        public ActionResult MyProfile()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = _userManager.FindById(currentUserId);  //.Users.FirstOrDefault(x => x.Id == currentUserId);

            MyProfileViewModel user = new MyProfileViewModel();

            user.FullName = currentUser.FullName;
            user.UserName = currentUser.UserName;
            user.Email = currentUser.Email;
            user.Address = currentUser.Address;
            user.PhoneNumber = currentUser.PhoneNumber;

            return View(user);
        }

    }
}





