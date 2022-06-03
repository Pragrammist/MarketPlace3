using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserDataBase;
namespace HukSleva.Controllers
{
    public class ValidCommentController : Controller
    {
        UserDb _db;
        public ValidCommentController(UserDb db)
        {
            _db = db;
        }
        public IActionResult ValidComment(int idBook, int idUser)
        {
            var user = _db.Users.Include(b => b.History).FirstOrDefault(u => u.Id == idUser);
            if (user.History == null)
            {
                user.History = new History { User = user, UserId = idUser };
            }
            if (ModelState.IsValid && user != null)
            {
                var form = ControllerContext.HttpContext.Request.Form;
                var text = form["commentText"];
                Comment comment = new Comment();
                comment.BookId = idBook;
                comment.Date = DateTime.Now;
                comment.Text = text;
                _db.Comments.Add(comment);
                user.History.Wrote = user.History.Wrote ?? new List<Comment>();
                user.History.Wrote.Add(comment);
                _db.Users.Update(user);
                _db.SaveChanges();
            }
            return RedirectToAction("GetBook", "Books", new { id = idBook });
        }
    }
}
