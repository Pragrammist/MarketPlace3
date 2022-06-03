using HukSleva.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HukSleva.ViewModels.ValidationModel;
using UserDataBase;
using HukSleva.Filters;

namespace HukSleva.Controllers
{
    public enum FieldType { nickname, phone, email, none }
    public class ValidUserController : Controller
    {
        UserDb _userDb;
        public ValidUserController(UserDb userDb)
        {
            _userDb = userDb;
        }
        protected User GetUser(Expression<Func<User, bool>> predicate = null)
        {
            var sec_fetch_mode = ControllerContext.HttpContext.Request.Headers["sec-fetch-mode"]; //Проверка на то, через навигацию вызвалось или remote, cors - это через remote
            User res = null;
            if (predicate != null && sec_fetch_mode == "cors")
                res = _userDb.Users.FirstOrDefault(predicate);

            return res;
        }
        protected bool Search(Expression<Func<User, bool>> predicate, out User foundedUser)
        {
            foundedUser = GetUser(predicate);
            
            bool isNull = foundedUser is null;

            if (!isNull)
            {
                return true;
            }
            return false;
        }
        protected bool SearchEmail(string email, out User foundedUser)
        {
            return Search(u => u.Email == email, out foundedUser);
        }
        protected bool SearchNickname(string nickname, out User foundedUser)
        {

            return Search(u => u.NickName == nickname, out foundedUser);
        }
        protected bool SearchPhone(string phone, out User foundedUser)
        {
            return Search(u => u.PhoneNumber == phone, out foundedUser);
        }
        protected bool IsExist(string filed, User user = null)
        {
            var fT = GetFieldType(filed);
            bool res;
            if (fT == FieldType.email)
            {
                res = SearchEmail(filed, out user);
            }
            else if (fT == FieldType.phone)
            {
                res = SearchPhone(filed, out user);
            }
            else
                res = SearchNickname(filed, out user);
            return res;
        }


        
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsExit(string allPurposeField)
        {
            
            bool isExist = IsExist(allPurposeField);
            

            return Json(isExist);
        }
       
        [AcceptVerbs("Get", "Post")]
        //!!!!!!!!!!!!
        public IActionResult PhoneIsNotExist(string PhoneNumber)
        {
            bool isExist = IsExist(PhoneNumber);
            return Json(!isExist);
            
        }
       
        [AcceptVerbs("Get", "Post")]
        public IActionResult NickNameIsNotExist(string nickname)
        {
            bool isExist = IsExist(nickname);
            return Json(!isExist);
        }
        
        [AcceptVerbs("Get", "Post")]
        public IActionResult EmailIsNotExist(string email)
        {
            bool isExist = IsExist(email);
            return Json(!isExist);
        }
        protected FieldType GetFieldType(string value)
        {
            EmailAddressAttribute emailAddress = new EmailAddressAttribute();
            PhoneAttribute phoneAttribute = new PhoneAttribute();

            if (phoneAttribute.IsValid(value))
            {
                return FieldType.phone;
            }
            else if (emailAddress.IsValid(value))
            {
                return FieldType.email;
            }
            else
            {
                return FieldType.nickname;
            }
        }

        
        
    }
}
