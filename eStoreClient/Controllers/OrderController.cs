using eStoreClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eStoreClient.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
