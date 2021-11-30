using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class Images : Controller
    {
        public IActionResult ImagesView()
        {
            return View();
        }
        public IActionResult ImagesAdd()
        {
            return View();
        }
    }
}
