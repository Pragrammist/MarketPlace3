using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserDataBase;

namespace HukSleva.Controllers
{
    public class ValidBookController : Controller
    {
        UserDb _userDb;
        public ValidBookController(UserDb userDb)
        {
            _userDb = userDb;
        }
        protected Book GetBook(string name, string author)
        {
            var res = _userDb.Books.FirstOrDefault(b => b.Name == name && b.Author == author);
            return res;
        }
        protected bool BookIsExist(string name, string author, out Book book)
        {
            book = GetBook(name, author);
            if (book != null)
                return true;
            return false;
        }
        
        [AcceptVerbs("GET", "SET")]
        public IActionResult IsExist(string name, string author)
        {
            if (author == null || name == null)
            {
                return Content("");
            }

            Book b;
            var res = BookIsExist(name, author, out b);
            if (res)
            {
                TempData["currentBook"] = b;
                return Json(res);
            }
            else
            {
                return Json("Книга не существует");
            }
        }
        [AcceptVerbs("GET", "SET")]
        public IActionResult IsNotExist(string name, string author)
        {
            if (author == null || name == null)
            {
                return Content("");
            }

            Book b;
            var res = !BookIsExist(name, author, out b);

            if (res)
            {
                TempData["currentBook"] = b;
                return Json(res);
            }
            else
            {
                return Json("Книга уже есть");
            }
            
        }
    }
}
