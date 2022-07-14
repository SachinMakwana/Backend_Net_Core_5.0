using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BACKEND_HTML_DOT_NET.Models;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class News : Controller
    {
        public IActionResult NewsView()
        {
            IEnumerable<NewsVM> news = null;


            return View(news);
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
