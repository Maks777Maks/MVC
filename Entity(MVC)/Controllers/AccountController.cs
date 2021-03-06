﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entity_MVC_.Data.EFContext;
using Entity_MVC_.Data.Models;
using Entity_MVC_.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Entity_MVC_.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> _signInManager;
        private readonly RoleManager<DbRole> _roleManager;
        private readonly EFDbContext _context;


        public AccountController(UserManager<DbUser> userManager, SignInManager<DbUser> signInManager,
            RoleManager<DbRole> roleManager, EFDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if(user==null)
            {
                ModelState.AddModelError("", "Некорректный логин");
                return View(model);
            }

            var result = _signInManager
              .PasswordSignInAsync(user, model.Password, false, false).Result;

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Некорректный пароль");
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            await Authenticate(model.Email); // аутентификация

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var roleName = "User";


            ////////////  Один раз
            //var result = _roleManager.CreateAsync(new DbRole
            //{
            //    Name = roleName
            //}).Result;

            //roleName = "Admin";

            //result = _roleManager.CreateAsync(new DbRole
            //{
            //    Name = roleName
            //}).Result;

            if (ModelState.IsValid)
            {
                UserProfile userProfile = new UserProfile
                {
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    RegistrationDate = DateTime.Now
                };

                DbUser dbUser = new DbUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    UserProfile = userProfile
                };

                var result = await _userManager.CreateAsync(dbUser, model.Password);
                result = _userManager.AddToRoleAsync(dbUser, roleName).Result;

                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(dbUser, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }

            return View(model);
        }


        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}