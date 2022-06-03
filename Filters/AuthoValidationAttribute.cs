using HukSleva.Controllers;
using HukSleva.Models;
using HukSleva.ViewModels.ValidationModel;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserDataBase;
namespace HukSleva.Filters
{
    public class AuthoValidationAttribute : Attribute, IActionFilter
    {
        UserDb _userDb;
        public AuthoValidationAttribute(UserDb userDb)
        {
            _userDb = userDb;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
             
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            UserValidAutho user = context.ActionArguments["user"] as UserValidAutho;
            User dbUser = null;
            if (user == null)
                return;


            var fT = user.GetFieldType(user.AllPurposeField);
            var field = user.AllPurposeField;

            if (fT == FieldType.email)
            {
                dbUser = _userDb.Users.FirstOrDefault(u => u.Email == field);
            }
            else if (fT == FieldType.phone)
            {
                dbUser = _userDb.Users.FirstOrDefault(u => u.PhoneNumber == field);
            }
            else
                dbUser = _userDb.Users.FirstOrDefault(u => u.NickName == field);

            context.HttpContext.Items.Add("user", dbUser);

            if (dbUser.Password != dbUser.GetHashCode(user.Password))
            {
                context.ModelState.AddModelError("Password", "Имя пользователя или пароль не верны");
                return;
            }

        }

        
    }
    
}
