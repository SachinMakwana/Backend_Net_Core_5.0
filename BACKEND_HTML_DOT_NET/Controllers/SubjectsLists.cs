using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class SubjectsLists : Controller
    {
        public IActionResult SubjectAdd()
        {
            return View();
        }
        public IActionResult SubjectView()
        {
            return View();
        }
    }
}
