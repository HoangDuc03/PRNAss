using eStoreClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eStoreClient.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit() { 
            return View();
        }


    }
}
