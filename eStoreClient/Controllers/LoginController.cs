using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using BusinessObject.Models;

using System.Text;
using Microsoft.Extensions.Configuration;
using DataAccess.Dto;

namespace eStoreClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            // URL của Member API, bạn có thể đặt trong appsettings.json để dễ dàng thay đổi
            ApiUrl = _configuration["API"] + "Member/Login";
        }

        public IActionResult Index()
        {
            // Kiểm tra nếu đã đăng nhập thì chuyển hướng về trang chủ
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            // Tạo đối tượng chứa thông tin đăng nhập để gửi đi
            var loginData = new { Email = email, Password = password };
            string jsonPayload = JsonSerializer.Serialize(loginData);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Gọi API bằng phương thức POST
            HttpResponseMessage response = await client.PostAsync(ApiUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Deserialize dữ liệu user trả về từ API
                MemberDto user = JsonSerializer.Deserialize<MemberDto>(strData, options);
                if(user.Email == null)
                {
                    ViewBag.Message = "Incorrect email or password. Please try again.";
                    return View();
                }
                else
                {
                    HttpContext.Session.SetString("User", JsonSerializer.Serialize(user));
                    HttpContext.Session.SetInt32("Role", (int)user.Role);
                    if (user.Role == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                
            }
            else
            {
                ViewBag.Message = "Incorrect email or password. Please try again.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa toàn bộ session
            return RedirectToAction("Index", "Login");
        }
    }
}