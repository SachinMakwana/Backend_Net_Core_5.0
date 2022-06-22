using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class News : Controller
    {
        public IActionResult NewsView()
        {
            return View();
        }
        public IActionResult NewsAdd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewsAdd(FromBodyAttribute fromBodyAttribute)
        {

            return View();
        }
    }
}
