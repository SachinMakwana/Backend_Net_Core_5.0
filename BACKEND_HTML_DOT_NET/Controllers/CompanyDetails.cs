using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class CompanyDetails : Controller
    {
        public IActionResult CompanyAdd()
        {
            return View();
        }
        public IActionResult CompanyView()
        {
            return View();
        }
        public IActionResult CompanyEdit()
        {
            return View();
        }
    }
}
