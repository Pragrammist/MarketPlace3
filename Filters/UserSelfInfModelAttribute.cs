using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HukSleva.ViewModels;
using UserDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HukSleva.Filters
{
    public class UserSelfInfModelAttribute : Attribute, IActionFilter
    {
        UserDb _userDb;
        public UserSelfInfModelAttribute(UserDb userDb)
        {
            _userDb = userDb;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            bool isCurrent = false;
            UserSelfInfoModel model = null;

            string id;
            object oId;
            
            

            var isPared = context.ActionArguments.TryGetValue("id", out oId);
            if (isPared)
            {
                id = oId.ToString();
            }
            else
            {
                id = null;
            }

            var claimId =  context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            var controller = (Controller)context.Controller;




            if ((claimId == id || id == null) && claimId != null)
            {
                model = new UserSelfInfoModel(context.HttpContext.User);
                isCurrent = true;
            }
            else if (id != null)
            {
                int iId;
                var parsRes = int.TryParse(id, out iId);
                int dbId;
                if (parsRes)
                {
                    dbId = iId;
                }
                else
                {
                    controller.ViewBag.isCurrent = isCurrent;
                    return;
                }

                var user = _userDb.Users.Include(u => u.Reading).FirstOrDefault(u => u.Id == int.Parse(id));
                if (user == null)
                {
                    controller.ViewBag.isCurrent = isCurrent;
                    return;
                }


                model = new UserSelfInfoModel(user);
            }
            else if (claimId == null)
            {
                context.Result = controller.RedirectToAction("Index", "Home");
            }

            

            controller.ViewBag.isCurrent = isCurrent;
            controller.TempData["userModel"] = model;
        }
    }
}
