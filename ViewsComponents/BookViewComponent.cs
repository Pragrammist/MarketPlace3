using HukSleva.ViewModels.BookController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HukSleva.ViewsComponents
{
    public class BookViewComponent : ViewComponent
    {
        private bool _isFullView;
        public IViewComponentResult Invoke(BookViewModel book, bool isFullView = false)
        {
            _isFullView = isFullView;

            ViewBag.isFullView = _isFullView;

            return View(book);
        }
    }
}
