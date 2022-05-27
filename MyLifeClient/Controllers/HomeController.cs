using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyLifeClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        //VALIDATION

        [AcceptVerbs("GET", "POST")]
        public async Task<JsonResult> ValidateLoginPassword(string email, string password)
        {
            var hasher = new PasswordHasher<User>();
            var user = await RequestsToServer<User>.SendGet($"https://localhost:5001/api/users/emails/{email}");
            var verificationResult = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verificationResult.Equals(PasswordVerificationResult.Success))
            {
                return Json(true);
            }
            return Json("Неверный пароль");
        }
        public async Task<JsonResult> ValidateLoginEmail(string email)
        {
            var user = await RequestsToServer<User>.SendGet($"https://localhost:5001/api/users/emails/{email}");
            if (user == default(User))
            {
                return Json("Пользователь с таким email не найден");
            }
            return Json(true);
        }
        public async Task<JsonResult> ValidateRegisterEmail(string email)
        {
            var user = await RequestsToServer<User>.SendGet($"https://localhost:5001/api/users/emails/{email}");
            if (user != default(User))
            {
                return Json("Пользователь с таким email уже существует");
            }
            return Json(true);
        }

        //AUTHORIZATION - REGISTRATION
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var hasher = new PasswordHasher<User>();
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
            var responce = await RequestsToServer<User>.SendPost(user, "https://localhost:5001/api/users");
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
            var user = await RequestsToServer<User>.SendGet($"https://localhost:5001/api/users/emails/{model.Email}");
            if (user != null)
            {
                MainUser.NewMain(user);
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("Login", "Home");
        }

        public IActionResult Quit()
        {
            MainUser.Quit();
            return RedirectToAction("Login", "Home");
        }

        //MAIN VIEWS

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //LIST PAGES

        public async Task<IActionResult> Diary()
        {
            var result = await RequestsToServer<IEnumerable<DiaryEntry>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/diary");
            return View(result);
        }

        public async Task<IActionResult> WeigthControl()
        {
            var result = await RequestsToServer<IEnumerable<State>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl");
            return View(result);
        }

        //ONE PAGE
        public async Task<IActionResult> DiaryEntry(Guid inputId)
        {
            var entry = await RequestsToServer<DiaryEntry>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/diary/{inputId}");
            return View(entry);
        }

        //CREATE PAGES

        public IActionResult CreateDiaryEntry()
        {
            return View();
        }

        public IActionResult CreateState()
        {
            return View(new InputState(DateTime.Today, 60, 0, 3));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiaryEntry(IFormFile imgFile, DiaryEntry inputEntry)
        {
            if (imgFile != null)
                inputEntry.Picture = ByteConvert.GetBytesFromFile(imgFile);
            var responce = await RequestsToServer<DiaryEntry>.SendPost(inputEntry, $"https://localhost:5001/api/users/{MainUser.GetId()}/diary");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Diary", "Home");
            }
            else return RedirectToAction("CreateDiaryEntry", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateState(InputState inputState)
        {
            var weigth = Convert.ToDouble(inputState.MainWeigth + "," + inputState.PartialWeight);
            var state = new State(Guid.NewGuid(), MainUser.GetId(), inputState.Date, weigth, inputState.Mood);
            var responce = await RequestsToServer<State>.SendPost(state, $"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else return RedirectToAction("CreateState", "Home");
        }

        //UPDATE PAGE

        public async Task<IActionResult> UpdateDiaryEntry(Guid inputId)
        {
            var entry = await RequestsToServer<DiaryEntry>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/diary/{inputId}");
            return View(entry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDiaryEntry(IFormFile imgFile, DiaryEntry inputEntry, bool shouldDelete)
        {
            if (shouldDelete)
                inputEntry.Picture = null;
            if (imgFile != null)
                inputEntry.Picture = ByteConvert.GetBytesFromFile(imgFile);
            var responce = await RequestsToServer<DiaryEntry>.SendPut(inputEntry, $"https://localhost:5001/api/users/{MainUser.GetId()}/diary/{inputEntry.Id}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Diary", "Home");
            }
            else return RedirectToAction("UpdateDiaryEntry", "Home");
        }

        //DELETE PAGE

        [HttpPost]
        public async Task<IActionResult> DeleteDiaryEntry(Guid inputId)
        {
            var responce = await RequestsToServer<DiaryEntry>.SendDelete($"https://localhost:5001/api/users/{MainUser.GetId()}/diary/{inputId}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Diary", "Home");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}