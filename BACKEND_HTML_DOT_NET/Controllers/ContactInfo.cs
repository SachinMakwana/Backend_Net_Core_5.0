using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class ContactInfo : Controller
    {
        public IActionResult ContactInfoAdd()
        {
            return View();
        }
        public IActionResult ContactInfoEdit()
        {
            return View();
        }
        public IActionResult ContactInfoView()
        {
            return View();
        }
    }
}
