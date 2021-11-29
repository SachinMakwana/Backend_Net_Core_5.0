using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class CollegeGallery : Controller
    {
        public IActionResult GalleryView()
        {
            return View();
        }
        public IActionResult GalleryAdd()
        {
            return View();
        }
    }
}
