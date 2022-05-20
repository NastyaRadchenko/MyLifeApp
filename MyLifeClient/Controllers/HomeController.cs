using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLifeClient.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLifeClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<JsonResult> ValidateLoginPassword(string email, string password)
        {
            var hasher = new PasswordHasher<User>();
            var httpClient = new HttpClient();
            var responce = await httpClient.GetAsync($"https://localhost:5001/api/users/emails/{email}");
            var user = JsonConvert.DeserializeObject<User>(responce.Content.ReadAsStringAsync().Result);
            var verificationResult = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verificationResult.Equals(PasswordVerificationResult.Success))
            {
                return Json(true);
            }
            return Json("Неверный пароль");
        }

        public async Task<JsonResult> ValidateRegisterEmail(string email)
        {
            var httpClient = new HttpClient();
            var responce = await httpClient.GetAsync($"https://localhost:5001/api/users/emails/{email}");
            if (responce.IsSuccessStatusCode)
            {
                return Json("Пользователь с таким email уже существует");
            }
            return Json(true);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Auth()
        {
            return View();
        }

        public IActionResult Quit()
        {
            MainUser.Quit();
            return RedirectToAction("Login", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var hasher = new PasswordHasher<User>();
            var httpClient = new HttpClient();
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email                
            };
            if (model.Password.Equals(model.ConfirmPassword))
            {
                user.PasswordHash = hasher.HashPassword(user, model.Password);
            }
            var userJson = JsonConvert.SerializeObject(user);
            HttpContent httpContent = new StringContent(userJson);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var responce = await httpClient.PostAsync(new Uri("https://localhost:5001/api/users"), httpContent);
            if (responce.IsSuccessStatusCode)
            {
                MainUser.NewMain(user);
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Register", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var httpClient = new HttpClient();
            var responce = await httpClient.GetAsync($"https://localhost:5001/api/users/emails/{model.Email}");
            var user = JsonConvert.DeserializeObject<User>(responce.Content.ReadAsStringAsync().Result);
           if (user != null)
            {
                MainUser.NewMain(user);
                return RedirectToAction("Index", "Home");
            } 
            else return RedirectToAction("Login", "Home");
        }

    }
}
