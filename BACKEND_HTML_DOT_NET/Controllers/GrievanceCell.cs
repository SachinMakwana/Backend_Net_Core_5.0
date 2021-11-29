using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class GrievanceCell : Controller
    {
        public IActionResult GreivanceCellAdd()
        {
            return View();
        }
        public IActionResult GreivanceCellComplaintView()
        {
            return View();
        }
        public IActionResult GreivanceCellView()
        {
            return View();
        }
        public IActionResult GrievanceAttachmentsAdd()
        {
            return View();
        }
        public IActionResult GrievanceAttachmentsView()
        {
            return View();
        }
        public IActionResult GrievanceCellMembersAdd()
        {
            return View();
        }
        public IActionResult GrievanceCellMembersDetails()
        {
            return View();
        }
    }
}
