using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class About_CollegeController : Controller
    {
        public IActionResult About_add()
        {
            return View();
        }

        public IActionResult About_view()
        {
            return View();
        }
    }
}
