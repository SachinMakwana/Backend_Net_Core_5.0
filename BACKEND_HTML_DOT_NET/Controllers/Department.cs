using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class Department : Controller
    {
        public IActionResult DepartmentAdd()
        {
            return View();
        }
        public IActionResult DepartmentView()
        {
            return View();
        }
        public IActionResult DepartmentAllView()
        {
            return View();
        }
    }
}
