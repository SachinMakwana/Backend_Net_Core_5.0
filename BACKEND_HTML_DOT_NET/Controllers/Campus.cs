using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class Campus : Controller
    {
        public IActionResult CampusAdd()
        {
            return View();
        }
        public IActionResult CampusView()
        {
            return View();
        }
        public IActionResult CampusEdit()
        {
            return View();
        }
    }
}
