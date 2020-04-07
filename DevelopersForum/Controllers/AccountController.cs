using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevelopersForum.Models;
using DevelopersForum.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevelopersForum.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly SignInManager<ApplicationUsers> _signInManager;

        public AccountController(UserManager<ApplicationUsers> userManager, 
            SignInManager<ApplicationUsers> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUsers
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    MemberSince = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if(result.Succeeded)
                {
                   await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "home");
                }

                foreach( var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {             
                var result = await _signInManager.PasswordSignInAsync
                    (
                        model.UserName, 
                        model.Password, 
                        model.RememberMe, false
                    );

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "home");
                }

                ModelState.AddModelError("", "Invalid Login Attempt");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}