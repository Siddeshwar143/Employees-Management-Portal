using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MVCDHProject.Models;
using System.Security.Claims;
using System.Text;

namespace MVCDHProject.Controllers
***REMOVED***
    public class AccountController : Controller
    ***REMOVED***
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        ***REMOVED***
            this.userManager = userManager;
            this.signInManager = signInManager;
      ***REMOVED***
        public IActionResult Register()
        ***REMOVED***
            return View();
      ***REMOVED***
        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel userModel)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                //IdentityUser represents a new user with a given set of attributes
                IdentityUser identityUser = new IdentityUser
                ***REMOVED***
                    UserName = userModel.Name,
                    Email = userModel.Email,
                    PhoneNumber = userModel.Mobile
              ***REMOVED***;
                //Creates a new user and returns a result which tells about success or failure
                var result = await userManager.CreateAsync(identityUser, userModel.Password);
                if (result.Succeeded)
                ***REMOVED***
                    //Implementing logic for sending a mail to confirm the Email
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    var confirmationUrlLink = Url.Action(
                 "ConfirmEmail", "Account", new ***REMOVED*** UserId = identityUser.Id, Token = token ***REMOVED***, Request.Scheme);
                    //Passing the information to SendMail method to send the Mail
                    SendMail(identityUser, confirmationUrlLink, "Email Confirmation Link");
                    TempData["Title"] = "Email Confirmation Link";
                    TempData["Message"] = "A confirm email link has been sent to your registered mail, click on it to confirm.";
                    return View("DisplayMessages");
              ***REMOVED***

                else
                ***REMOVED***
                    foreach (var Error in result.Errors)
                    ***REMOVED***
                        ModelState.AddModelError("", Error.Description);
                  ***REMOVED***
              ***REMOVED***
          ***REMOVED***
            return View(userModel);
      ***REMOVED***

        public void SendMail(IdentityUser identityUser, string requestLink, string subject)
        ***REMOVED***
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("Hello " + identityUser.UserName + "<br /><br />");
            if (subject == "Email Confirmation Link")
            ***REMOVED***
                mailBody.Append("Click on the link below to confirm your email:");
          ***REMOVED***
            else if (subject == "Change Password Link")
            ***REMOVED***
                mailBody.Append("Click on the link below to reset your password:");
          ***REMOVED***
            mailBody.Append("<br />");
            mailBody.Append(requestLink);
            mailBody.Append("<br /><br /> ");
            mailBody.Append("Regards");
            mailBody.Append("<br /><br />");
            mailBody.Append("Customer Support.");

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailBody.ToString();

            MailboxAddress fromAddress = new MailboxAddress("Customer Support", "maligesiddeshwar@gmail.com");
            MailboxAddress toAddress = new MailboxAddress(identityUser.UserName, identityUser.Email);

            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(fromAddress);
            mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = bodyBuilder.ToMessageBody();
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 465, true);
            smtpClient.Authenticate("maligesiddeshwar@gmail.com", "tuyv bikt voib djvl");
            smtpClient.Send(mailMessage);
      ***REMOVED***

        [HttpGet]
        public IActionResult Login()
        ***REMOVED***
            return View();
      ***REMOVED***

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel, string returnUrl = null)
        ***REMOVED***
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            ***REMOVED***
                var result = await signInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password, loginModel.Rememberme, false);
                if (result.Succeeded)
                ***REMOVED***
                    if (loginModel.Rememberme)
                    ***REMOVED***
                        Response.Cookies.Append("RememberedName", loginModel.Name, new CookieOptions
                        ***REMOVED***
                            Expires = DateTimeOffset.Now.AddDays(30),
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                      ***REMOVED***);
                  ***REMOVED***
                    else
                    ***REMOVED***
                        Response.Cookies.Delete("RememberedName");
                  ***REMOVED***

                    return RedirectToLocal(returnUrl);
              ***REMOVED***
          ***REMOVED***
            return View(loginModel);
      ***REMOVED***

        private IActionResult RedirectToLocal(string returnUrl)
        ***REMOVED***
            if (Url.IsLocalUrl(returnUrl))
            ***REMOVED***
                return RedirectToAction("Index","Home");
          ***REMOVED***
            else
            ***REMOVED***
                return RedirectToAction("Login");
          ***REMOVED***
      ***REMOVED***

        public async Task<IActionResult> Logout()
        ***REMOVED***
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
      ***REMOVED***

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        ***REMOVED***
            if (userId != null && token != null)
            ***REMOVED***
                var User = await userManager.FindByIdAsync(userId);
                if (User != null)
                ***REMOVED***
                    var result = await userManager.ConfirmEmailAsync(User, token);
                    if (result.Succeeded)
                    ***REMOVED***
                        TempData["Title"] = "Email Confirmation Success.";
                        TempData["Message"] = "Email confirmation is completed. You can now login into the application.";
                        return View("DisplayMessages");
                  ***REMOVED***
                    else
                    ***REMOVED***
                        StringBuilder Errors = new StringBuilder();
                        foreach (var Error in result.Errors)
                        ***REMOVED***
                            Errors.Append(Error.Description + "<br />");
                      ***REMOVED***
                        TempData["Title"] = "Confirmation Email Failure";
                        TempData["Message"] = Errors.ToString();
                        return View("DisplayMessages");
                  ***REMOVED***
              ***REMOVED***
                else
                ***REMOVED***
                    TempData["Title"] = "Invalid User Id.";
                    TempData["Message"] = "User Id which is present in confirm email link is in-valid.";
                    return View("DisplayMessages");
              ***REMOVED***
          ***REMOVED***
            else
            ***REMOVED***
                TempData["Title"] = "Invalid Email Confirmation Link.";
                TempData["Message"] = "Email confirmation link is invalid, either missing the User Id or Confirmation Token.";
                return View("DisplayMessages");
          ***REMOVED***
      ***REMOVED***

        public IActionResult ForgotPassword()
        ***REMOVED***
            return View();
      ***REMOVED***
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ChangePasswordViewModel model)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                var User = await userManager.FindByNameAsync(model.Name);
                if (User != null && await userManager.IsEmailConfirmedAsync(User))
                ***REMOVED***
                    var token = await userManager.GeneratePasswordResetTokenAsync(User);
                    var confirmationUrlLink = Url.Action("ChangePassword", "Account", new ***REMOVED*** UserId = User.Id, Token = token ***REMOVED***,
           Request.Scheme);
                    SendMail(User, confirmationUrlLink, "Change Password Link");
                    TempData["Title"] = "Change Password Link";
                    TempData["Message"] = "Change password link has been sent to your mail, click on it and change password.";
                    return View("DisplayMessages");
              ***REMOVED***
                else
                ***REMOVED***
                    TempData["Title"] = "Change Password Mail Generation Failed.";
                    TempData["Message"] = "Either the Username you have entered is in-valid or your email is not confirmed.";
                    return View("DisplayMessages");
              ***REMOVED***
          ***REMOVED***
            return View(model);
      ***REMOVED***
        public IActionResult ChangePassword()
        ***REMOVED***
            return View();
      ***REMOVED***
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordViewModel model)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                var User = await userManager.FindByIdAsync(model.UserId);
                if (User != null)
                ***REMOVED***
                    var result = await userManager.ResetPasswordAsync(User, model.Token, model.Password);
                    if (result.Succeeded)
                    ***REMOVED***
                        TempData["Title"] = "Reset Password Success";
                        TempData["Message"] = "Your password has been reset successfully.";
                        return View("DisplayMessages");
                  ***REMOVED***
                    else
                    ***REMOVED***
                        ***REMOVED***
                            foreach (var Error in result.Errors)
                                ModelState.AddModelError("", Error.Description);
                      ***REMOVED***
                  ***REMOVED***
              ***REMOVED***
                else
                ***REMOVED***
                    TempData["Title"] = "Invalid User";
                    TempData["Message"] = "No user exists with the given User Id.";
                    return View("DisplayMessages");
              ***REMOVED***
              ***REMOVED***
                return View(model);
          ***REMOVED***

        public IActionResult ExternalLogin(string returnUrl, string Provider)
        ***REMOVED***
            var url = Url.Action("CallBack", "Account", new ***REMOVED*** ReturnUrl = returnUrl ***REMOVED***);
            var properties = signInManager.ConfigureExternalAuthenticationProperties(Provider, url);
            return new ChallengeResult(Provider, properties);
      ***REMOVED***
        public async Task<IActionResult> CallBack(string returnUrl)
        ***REMOVED***
            if (string.IsNullOrEmpty(returnUrl))
            ***REMOVED***
                returnUrl = "~/";
          ***REMOVED***
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            ***REMOVED***
                ModelState.AddModelError("", "Error loading external login information.");
                return View("Login");
          ***REMOVED***
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            if (signInResult.Succeeded)
            ***REMOVED***
                return LocalRedirect(returnUrl);
          ***REMOVED***
            else
            ***REMOVED***
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                ***REMOVED***
                    var user = await userManager.FindByEmailAsync(email);
                    if (user == null)
                    ***REMOVED***
                        user = new IdentityUser
                        ***REMOVED***
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone),
                      ***REMOVED***;
                        var identityResult = await userManager.CreateAsync(user);
                  ***REMOVED***
                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, false);
                    return LocalRedirect(returnUrl);
              ***REMOVED***
                TempData["Title"] = "Error";
                TempData["Message"] = "Email claim not received from third party provided.";
                return RedirectToAction("DisplayMessages");
          ***REMOVED***
      ***REMOVED***

  ***REMOVED***
***REMOVED***

      