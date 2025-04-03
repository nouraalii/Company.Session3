using Company.Session3.DAL.Models;
using Company.Session3.DAL.SMS;
using Company.Session3.PL.Dtos;
using Company.Session3.PL.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Session3.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly ITwilioService _twilioService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, ITwilioService twilioService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _twilioService = twilioService;
        }

        #region SignUp
        [HttpGet] //Get: /Account/SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        //P@ssW0rd
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                            IsAgree = model.IsAgree,
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid SignUp !!");
            }

            return View(model);
        }
        #endregion

        #region SignIn
        [HttpGet] 
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user =await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag) 
                    {
                        //Sign In
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }

                ModelState.AddModelError("","Invalid Login !");
            }

            return View(model);
        }
        #endregion

        #region SignOut
        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPassword(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Generate Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create Reset Password URL
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    // If sending email
                    if (model.SendViaEmail)
                    {
                        var email = new Email()
                        {
                            To = model.Email,
                            Subject = "Reset Password",
                            Body = url
                        };

                        // Send Email
                        _mailService.SendEmail(email);
                    }

                    // If sending SMS
                    if (model.SendViaSMS)
                    {
                        // Format Phone Number (ensure it has a "+" prefix for international numbers)
                        if (!user.PhoneNumber.StartsWith("+"))
                        {
                            user.PhoneNumber = "+20" + user.PhoneNumber.TrimStart('0');
                        }

                        var sms = new SMS()
                        {
                            To = user.PhoneNumber,
                            Body = url
                        };

                        // Send SMS
                        _twilioService.SendSMS(sms);
                    }

                    return RedirectToAction("CheckYourInbox");
                }
                else
                {
                    ModelState.AddModelError("", "Email not found.");
                }
            }

            // Return back to the ForgetPassword view if validation fails
            return View("ForgetPassword", model);
        }




        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model, string Email, string Token)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Token))
                {
                    return BadRequest("Invalid operation. Missing email or token.");
                }

                var user = await _userManager.FindByEmailAsync(Email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, Token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid or expired reset token.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User not found.");
                }
            }

            return View();
        }

        #endregion


        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties() 
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var cliams = result.Principal.Identities.FirstOrDefault().Claims.Select(
                cliams => new
                {
                    cliams.Type,
                    cliams.Value,
                    cliams.Issuer,
                    cliams.OriginalIssuer
                }
                );
            return RedirectToAction("Index", "Home");
        }




        public IActionResult FacebookLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("FacebookResponse")
            };
            return Challenge(prop, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            var cliams = result.Principal.Identities.FirstOrDefault().Claims.Select(
                cliams => new
                {
                    cliams.Type,
                    cliams.Value,
                    cliams.Issuer,
                    cliams.OriginalIssuer
                }
                );
            return RedirectToAction("Index", "Home");
        }
    }
}
