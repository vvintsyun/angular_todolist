using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Todolist.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        [HttpGet]
        [Route("/api/google-login")]
        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync("Google", new AuthenticationProperties() { RedirectUri = "/signin-google" });
        }

        [HttpGet]
        [Route("/api/usercheck")]
        public async Task CheckUser()
        {
            var x = User;
            await HttpContext.ChallengeAsync("Google", new AuthenticationProperties() { RedirectUri = "/signin-google" });
        }
    }
}
