using DataAccess.Dto;
using eStoreClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace eStoreClient.Controllers
{
    public class MemberController : Controller
    {
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        private readonly IConfiguration _configuration;

        public MemberController(IConfiguration configuration)
        {
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUrl = _configuration["API"]+"Member/";
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ApiUrl+"GetAll");

            if (response.IsSuccessStatusCode)
            {
                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                // Deserialize dữ liệu thành danh sách MemberDto
                
                List<MemberDto> members = JsonSerializer.Deserialize<List<MemberDto>>(strData, options);
                ViewBag.Members = members;
                return View();
            }
            else
            {
                ViewBag.ErrorMessage = "Error retrieving member data.";
                return View();
            }
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MemberCreateDto _member)
        {
            var member = new MemberDto
            {
                Email = _member.Email,
                Password = _member.Password,
                City = _member.City,
                CompanyName = _member.CompanyName,
                Country = _member.Country
            };

            if (!ModelState.IsValid)
            {
                return View(member);
            }

            try
            {
                string jsonMember = JsonSerializer.Serialize(member);
                var content = new StringContent(jsonMember, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(ApiUrl + "Create", content);

                if (response.IsSuccessStatusCode)
                {
                    string strData = await response.Content.ReadAsStringAsync();
                    int result = int.Parse(strData);

                    if (result > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else if (result == 0)
                    {
                        ModelState.AddModelError("Email", "This email address is already in use.");
                        return View(member);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the member.");
                        return View(member);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred. Status code: " + response.StatusCode);
                    return View(member);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An exception occurred: " + ex.Message);
                return View(member);
            }
        }

    }
}
