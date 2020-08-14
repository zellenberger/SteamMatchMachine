using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Steam_Match_Machine.Models;
using Steam_Match_Machine.Models.API;
using Steam_Match_Machine.Services;

namespace Steam_Match_Machine.Controllers
{
    [Authorize]
    [Route("")]
    public class AccountController : Controller
    {
        private readonly DataService _dataService;

        private readonly SteamApi _steamApi;

        public AccountController(DataContext dataContext, SteamApi steamApi)
        {
            // Instantiate an instance of the data service.
            _dataService = new DataService(dataContext, steamApi);

            _steamApi = steamApi;
        }

        [AllowAnonymous]
        [Route("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet, Route("/sign-in")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost, Route("/sign-in")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            User user = _dataService.GetUser(loginViewModel.EmailAddress);

            if (user == null)
            {
                // Set email address not registered error message.
                ModelState.AddModelError("Error", "An account does not exist with that email address.");

                return View();
            }

            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            PasswordVerificationResult passwordVerificationResult =
                passwordHasher.VerifyHashedPassword(null, user.PasswordHash, loginViewModel.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                // Set invalid password error message.
                ModelState.AddModelError("Error", "Invalid password.");

                return View();
            }

            // Add the user's ID (NameIdentifier), first name and role
            // to the claims that will be put in the cookie.
            var claims = new List<Claim> {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ()),
                new Claim (ClaimTypes.Name, user.FirstName),
                new Claim (ClaimTypes.Email, user.EmailAddress),
                new Claim (ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties { };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        [HttpGet, Route("/sign-out")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet, Route("/register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost, Route("/register")]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            User existingUser = _dataService.GetUser(registerViewModel.EmailAddress);
            if (existingUser != null)
            {
                // Set email address already in use error message.
                ModelState.AddModelError("Error", "An account already exists with that email address.");

                return View();
            }

            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

            User user = new User()
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                EmailAddress = registerViewModel.EmailAddress,
                PasswordHash = passwordHasher.HashPassword(null, registerViewModel.Password)
            };

            _dataService.AddUser(user);

            return RedirectToAction("Login", "Account");
        }

        [Route("/user-profile")]
        public IActionResult UserProfile()
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            UserProfileViewModel userProfileViewModel = new UserProfileViewModel();

            // Gets and sets the recommended video games details.
            userProfileViewModel.Recs = _steamApi.SetVideoGameDetails(_dataService.GetRecommendations(userId));

            userProfileViewModel.Wishlist = _steamApi.SetVideoGameDetails(_dataService.GetUserWishlist(userId));

            // TODO: look at me
            return View(userProfileViewModel);
        }
    }
}