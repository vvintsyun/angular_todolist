﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Todolist.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Route("/api/account/google-login")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult SignInWithGoogle()
        {
            var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action(nameof(HandleExternalLogin)));
            return Challenge(authenticationProperties, "Google");
        }

        [HttpGet("/api/account/isAuthenticated")]
        public bool IsAuthenticated()
        {
            var x = User;
            var q = _signInManager.IsSignedIn(x);
            var info = _signInManager.GetExternalLoginInfoAsync();
            return false;
        }

        [HttpGet("/signin-google")]
        public async Task<IActionResult> HandleExternalLogin()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();var qwe = _signInManager.GetExternalAuthenticationSchemesAsync();
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (!result.Succeeded) //user does not exist yet
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var newUser = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                var createResult = await _userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                    throw new Exception(createResult.Errors.Select(e => e.Description).Aggregate((errors, error) => $"{errors}, {error}"));

                await _userManager.AddLoginAsync(newUser, info);
                var newUserClaims = info.Principal.Claims.Append(new Claim("userId", newUser.Id));
                await _userManager.AddClaimsAsync(newUser, newUserClaims);
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }

            return Redirect("http://localhost:4200");
        }
    }
}