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
    public class PlacementCell : Controller
    {
        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        RestClient client;
        public PlacementCell(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }
        public IActionResult PlacementAdd()
        {
            return View();
        }
        public IActionResult PlacementView()
        {
            return View();
        }
        public IActionResult PlacementEdit()
        {
            return View();
        }
        public IActionResult PlacementDetails()
        {
            return View();
        }
        public IActionResult PlacementAttachmentsAdd()
        {
            return View();
        }
        public IActionResult PlacementAttachmentsView()
        {
            return View();
        }
        public IActionResult PlacementMemberAdd()
        {
            return View();
        }
        public IActionResult PlacementMemberView()
        {
            return View();
        }
    }
}
