using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLifeClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var _passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
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
            var responce = await httpClient.PostAsync(new Uri("https://localhost:5001/api/users"), httpContent); //начни с закладки в edge
            if (responce.IsSuccessStatusCode) 
                return RedirectToAction("Login", "Home");
            else return RedirectToAction("Register", "Home");
        }
    }
}
