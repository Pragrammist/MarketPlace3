using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HukSleva.ViewModels.ValidationModels.BookValid;
using UserDataBase;
using System.IO;
using HukSleva.ViewModels.BookController;
using HukSleva.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HukSleva.Controllers
{
    public class BooksController : Controller
    {
        UserDb _db;
        UserFilesDb _filesDb;
        BookViewModelsCollectionService _booksS;

        public BooksController(UserDb userDb, UserFilesDb filesDb, BookViewModelsCollectionService booksS)
        {
            _db = userDb;
            _filesDb = filesDb;
            _booksS = booksS;
            if (_booksS.Count < 1)
            {
                Book[] bs2 = _db.Books.Include(b => b.Comments).ToArray();
                for (int i = 0; i < bs2.Length && i < 10; i++)// 10 ЧИСЛО книг отображаемых на странице - о: оптимизация
                {

                    var img = _filesDb.Files.Find(bs2[i].ImageBytesId).Bytes;
                    var viewModel = new BookViewModel(bs2[i], img);
                    _booksS.Add(viewModel);
                }
            }
        }
        [HttpPost]
        public IActionResult AddBook(AddBookValid book)
        {
            if (book == null)
                return Content("");
            //if (ModelState.IsValid) { }

            if (book.BookBytes == null || book.Img == null)
            {
                ModelState.AddModelError("", "Загрузите книгу и фотографию");
            }
            if (ModelState.IsValid)
            {
                Book book1 = new Book { Author = book.Author, Name = book.Name, Price = book.Price };
                UserFile fImg = new UserFile();
                UserFile fBook = new UserFile();
                fImg.FileType = fileType.img;
                fBook.FileType = fileType.book;
                fImg.Name = "";
                fBook.Name = "";


                using (MemoryStream stream = new MemoryStream())
                {
                    var t = book.Img.FirstOrDefault();
                    if (t == null)
                    {
                        return Content(t + "is null(img)");
                    }
                    t.CopyTo(stream);
                    fImg.Extention = Path.GetExtension(t.Name);
                    t.CopyTo(stream);
                    fImg.Bytes = stream.ToArray();
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    var t = book.BookBytes.FirstOrDefault();
                    if (t == null)
                    {
                        return Content(t + "is null(book)");
                    }
                    t.CopyTo(stream);
                    fBook.Extention = Path.GetExtension(t.Name);
                    t.CopyTo(stream);
                    fBook.Bytes = stream.ToArray();
                }

                var imgEnt = _filesDb.Files.Add(fImg);
                var bookEnt = _filesDb.Files.Add(fBook);
                _filesDb.SaveChanges();


                var imgId = imgEnt.Entity.Id;
                var bookId = bookEnt.Entity.Id;


                book1.ImageBytesId = imgId;
                book1.BookBytesId = bookId;
                var viewModel = new BookViewModel(book1, fImg.Bytes);
                _booksS.Add(viewModel);
                _db.Books.Add(book1);

                _db.SaveChanges();
                return RedirectToAction("Catalog", "Books");

            }
            return View("~/Views/MemberPages/AddBook.cshtml");
        }
        public IActionResult Catalog()
        {
            //var admin = _db.Users.Find(24); admin.Money = 1000; _db.Users.Update(admin); _db.SaveChanges();

            return View(_booksS);
        }
        public IActionResult GetBook(int id)
        {   
            if (id < 1)
            {
                return RedirectToAction("Catalog");
            }
            BookViewModel book = _booksS.FirstOrDefault(b => b.Id == id);
            var b2 = _db.Books.Include(b => b.Comments)?.FirstOrDefault(b => b.Id == id);
            book?.Refresh(b2);
            ViewBag.UserId = int.Parse(User.FindFirst("id").Value);
            return View(book);
        }
        public IActionResult MakeReading(int idBook, int idUser)
        {
            var book = _db.Books.Find(idBook);
            var user = _db.Users.Include(u => u.History).Include(u => u.Reading).FirstOrDefault(u => u.Id == idUser);
            user.History = user.History ?? new History { User = user, UserId = user.Id };
            if (book != null && user != null && user.History.Bought.FirstOrDefault(b => b.Id == idBook) != null)
            {
                var identity = User.Identity as ClaimsIdentity;
                identity.RemoveClaim(identity.FindFirst("readingBookId"));
                identity.AddClaim(new Claim("readingBookId", $"{idBook}"));
                // установка аутентификационных куки
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                user.Reading = book;
                _db.SaveChanges();
            }
            return RedirectToAction("GetBook", new { id = idBook });
        }
        public IActionResult MakeRead(int idBook, int idUser)
        {
            var book = _db.Books.Find(idBook);
            var user = _db.Users.Include(u => u.History).Include(u => u.Reading).FirstOrDefault(u => u.Id == idUser);
            user.History = user.History ?? new History { User = user, UserId = user.Id };
            if (book != null && user != null && user.History.Bought.FirstOrDefault(b => b.Id == idBook) != null)
            {
                var identity = User.Identity as ClaimsIdentity;
                var readingIdBook = user.Reading?.Id;
                if (readingIdBook == idBook)
                {
                    identity.RemoveClaim(identity.FindFirst("readingBookId"));
                    identity.AddClaim(new Claim("readingBookId", $""));
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                    user.Reading = null;
                }

                user.History.Read = user.History.Read ?? new List<Book>();
                if (user.History.Read.FirstOrDefault(b => b.Id == book.Id) == null)
                {
                    user.History.Read.Add(book);
                }
                _db.Users.Update(user);
                _db.SaveChanges();
            }
            return RedirectToAction("GetBook", new { id = idBook });
        }
        public IActionResult AddRating(int idBook, int score, int idUser)
        {
            var b = _db.Books.Find(idBook);
            
            var user = _db.Users.Include(u => u.History).Include(u => u.History.Reated).FirstOrDefault(u => u.Id == idUser);
            if (user.History == null)
            {
                user.History = new History();
            }
            if (user.History.Reated == null)
            {
                user.History.Reated = new List<Book>();
            }
            var isNotRated = user.History.Reated.FirstOrDefault(b => b.Id == idBook) is null;
            if (b != null && user != null && isNotRated)
            {
                var rating = ((b.Rating * b.NumRated) + score) / (++b.NumRated);
                b.Rating = rating;


                

                user.History.Reated.Add(b);
                _db.Users.Update(user);
                _db.Books.Update(b);
                _db.SaveChanges();
            }
            return RedirectToAction("GetBook", new { id = idBook });
        }
        public IActionResult BuyBook(int idBook, int idUser)
        {
            var book = _db.Books.Find(idBook);
            var user = _db.Users.Include(u => u.History).Include(u => u.Reading).FirstOrDefault(u => u.Id == idUser);
            if (book != null && user != null)
            {
                var readingIdBook = user.Reading?.Id;
                user.History = user.History ?? new History { User = user, UserId = user.Id };
                user.History.Bought = user.History.Bought ?? new List<Book>();
                if (book.Price <= user.Money && user.History.Bought.FirstOrDefault(b => b.Id == book.Id) == null)
                {
                    user.Money -= book.Price;
                    var identity = User.Identity as ClaimsIdentity;
                    identity.RemoveClaim(identity.FindFirst("money"));
                    identity.AddClaim(new Claim("money", $"${user.Money}"));
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                    user.History.Bought.Add(book);
                }
                _db.Users.Update(user);
                _db.SaveChanges();
            }
            return RedirectToAction("GetBook", new { id = idBook });
        }
        public IActionResult ShopingCard()
        {
            int userId = -1;
            int.TryParse(User.Claims.First(c => c.Type == "id").Value, out userId);
            
            var user = _db.Users.Include(u => u.ShoppingCard).Include(u => u.ShoppingCard.Books).FirstOrDefault(u => u.Id == userId); 
            if (user != null)
            {
                if (user.ShoppingCard == null)
                {
                    user.ShoppingCard = new ShoppingCard();
                }
                var bList = user.ShoppingCard.Books ?? new List<Book>();

                BookViewModelsCollectionService books = new BookViewModelsCollectionService();

                var coll = _booksS.Where(b => b.Id == (bList.FirstOrDefault(b2 => b.Id == b2.Id) ?? new Book {Id = -1 }).Id).ToArray();

                books.AddRange(coll);
                return View("Catalog", books);
            }

            return RedirectToAction("Catalog", "Books", null);
        }
        public IActionResult AddBookInShopingCard(int idBook, int idUser)
        {
            var user = _db.Users.Include(u => u.ShoppingCard).Include(u => u.ShoppingCard.Books).FirstOrDefault(u => u.Id == idUser);
            var book = _db.Books.Find(idBook);

            if (user != null && book != null)
            {
                if (user.ShoppingCard == null)
                {
                    user.ShoppingCard = new ShoppingCard();
                    user.ShoppingCard.Books = new List<Book>();
                }


                var b2 = user.ShoppingCard.Books.Find(b => b.Id == idBook);
                if (b2 == null)
                {
                    user.ShoppingCard.Books.Add(book);
                    _db.Update(user);
                    _db.Update(book);
                    _db.SaveChanges();
                }
                
            }

            return RedirectToAction("Catalog", "Books", _booksS);
        }
    }
}
