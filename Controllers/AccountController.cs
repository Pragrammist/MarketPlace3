using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HukSleva.Models;
using System.Linq.Expressions;
using HukSleva.ViewModels.ValidationModel;
using HukSleva.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using UserDataBase;
using Microsoft.AspNetCore.Mvc.Filters;
using HukSleva.ViewModels;
using HukSleva.Services;
using Microsoft.EntityFrameworkCore;
using HukSleva.ViewModels.BookController;

namespace HukSleva.Controllers
{
    
    public class AccountController : Controller
    {
        UserDb _userDb;
        UserFilesDb _files;
        BookViewModelsCollectionService _booksService;
        public AccountController(UserDb userDb, BookViewModelsCollectionService booksService, UserFilesDb userFiles)
        {
            _userDb = userDb;
            _booksService = booksService;
            _files = userFiles;
        }
        
        public IActionResult Registration()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Content(User.Identity.Name);
            }

            return View();
        }
        
        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthoValidationAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AuthorizationAsync(UserValidAutho user, string id)
        {
            if (ModelState.IsValid)
            {
                var userDb = ControllerContext.HttpContext.Items["user"] as User;
                await Authenticate(userDb);
                return RedirectToAction("SelfInfo", "Account");
            }
            else
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationAsync(UserValidReg user)
        {
            if (ModelState.IsValid)
            {
                User regUser = new User { BirthDate = user.BirthDate, Email = user.Email, NickName = user.NickName, PhoneNumber = user.PhoneNumber, SmallDiscription = user.SmallDiscription };
                regUser.Password = regUser.GetHashCode(user.Password);
                regUser.Role = Role.user;
                regUser.Money = 1000;
                await _userDb.Users.AddAsync(regUser);
                await _userDb.SaveChangesAsync();
                await Authenticate(regUser);

                return RedirectToAction("SelfInfo", "Account");
                
            }
            else
                return View();
        }
        private async Task Authenticate(User user)
        {
            string role = user.Role.ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.NickName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                new Claim("dateBirthday", user.BirthDate.ToString("d")),
                new Claim("email", user.Email ?? ""),
                new Claim("historyId", user?.History?.Id.ToString() ?? ""),
                new Claim("phoneNumber", user.PhoneNumber ?? ""),
                new Claim("readingBookId", user?.Reading?.Id.ToString() ?? ""),
                new Claim("role", user.Role.ToString()),
                new Claim("shoppingCardId", user?.ShoppingCard?.Id.ToString() ?? ""),
                new Claim("smallDiscription", user.SmallDiscription ?? ""),
                new Claim("id", user.Id.ToString()),
                new Claim("money", user?.Money.ToString() ?? "")
                
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        
        [ServiceFilter(typeof(UserSelfInfModelAttribute))]
        public IActionResult SelfInfo(string id = null)
        {
            UserSelfInfoModel model = (UserSelfInfoModel)TempData["userModel"];
            int id2;
            var isParsed = int.TryParse(model.BookReadingId, out id2);
            if (isParsed)
                ViewBag.BookName = _userDb.Books.Find(id2).Name;
            return View(model);
        }

        
         
    }
}
