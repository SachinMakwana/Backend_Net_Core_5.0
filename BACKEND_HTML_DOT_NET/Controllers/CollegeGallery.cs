using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class CollegeGallery : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private List<GalleryVM> galleryVMList = new List<GalleryVM>();

        public IActionResult GalleryView(int id=0)
        {

            GalleryVM galleryVM = new GalleryVM();
            galleryVM = galleryVMList.Where(m => m.Id == id).FirstOrDefault();

            return View(galleryVM);
        }
        public IActionResult GalleryAdd()
        {
            return View();
        }
    }
}
