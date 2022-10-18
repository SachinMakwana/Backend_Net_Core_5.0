using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class GrievanceCell : Controller
    {
        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        RestClient client;
        public GrievanceCell(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }
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
