using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HukSleva.Controllers
{
    [Authorize("member")]
    public class MemberPagesController : Controller
    {
        
        public IActionResult MainPage()
        {
            return View();
        }
        
        public IActionResult AddBook()
        {
            return View();
        }

        
    }
}
