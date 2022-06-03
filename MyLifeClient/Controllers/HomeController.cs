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

        public async Task<IActionResult> Library()
        {
            var result = await RequestsToServer<IEnumerable<Book>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/library");
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

        public async Task<IActionResult> Book(Guid inputId)
        {
            var book = await RequestsToServer<Book>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/library/{inputId}");
            return View(book);
        }

        //CREATE PAGES

        public IActionResult CreateDiaryEntry()
        {
            return View();
        }

        public IActionResult CreateState()
        {
            return View(new InputState(Guid.NewGuid(), DateTime.Today, 60, 0, 3));
        }

        public async Task<IActionResult> CreateBook()
        {
            var result = await RequestsToServer<IEnumerable<Book>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/library/categories");
            ViewBag.Category = result;
            return View();
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
            var state = new State(inputState.Id, MainUser.GetId(), inputState.Date, weigth, inputState.Mood);
            var responce = await RequestsToServer<State>.SendPost(state, $"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("WeigthControl", "Home");
            }
            else return RedirectToAction("CreateState", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(IFormFile imgFile, Book inputBook)
        {
            if (imgFile != null)
                inputBook.Picture = ByteConvert.GetBytesFromFile(imgFile);
            var responce = await RequestsToServer<Book>.SendPost(inputBook, $"https://localhost:5001/api/users/{MainUser.GetId()}/library");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Library", "Home");
            }
            else return RedirectToAction("CreateBook", "Home");
        }

        //UPDATE PAGE

        public async Task<IActionResult> UpdateDiaryEntry(Guid inputId)
        {
            var entry = await RequestsToServer<DiaryEntry>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/diary/{inputId}");
            return View(entry);
        }

        public async Task<IActionResult> UpdateState(Guid inputId)
        {
            var state = await RequestsToServer<State>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl/{inputId}");
            var mainWeigth = Convert.ToInt32(Math.Truncate(state.Weigth));
            var partialWeigth = Convert.ToInt32((state.Weigth - mainWeigth)*10);
            var modelState = new InputState(state.Id, state.Date, mainWeigth, partialWeigth, state.Mood);
            return View(modelState);
        }

        public async Task<IActionResult> UpdateBook(Guid inputId)
        {
            var book = await RequestsToServer<Book>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/library/{inputId}");
            var categories = await RequestsToServer<IEnumerable<Book>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/library/categories");
            ViewBag.Category = categories;
            return View(book);
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

        [HttpPost]
        public async Task<IActionResult> UpdateState(InputState inputState)
        {
            var weigth = Convert.ToDouble(inputState.MainWeigth + "," + inputState.PartialWeight);
            var state = new State(inputState.Id, MainUser.GetId(), inputState.Date, weigth, inputState.Mood);
            var responce = await RequestsToServer<State>.SendPut(state, $"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl/{state.Id}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("WeigthControl", "Home");
            }
            else return RedirectToAction("UpdateState", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBook(IFormFile imgFile, Book inputBook, bool shouldDelete)
        {
            if (shouldDelete)
                inputBook.Picture = null;
            if (imgFile != null)
                inputBook.Picture = ByteConvert.GetBytesFromFile(imgFile);
            var responce = await RequestsToServer<Book>.SendPut(inputBook, $"https://localhost:5001/api/users/{MainUser.GetId()}/library/{inputBook.Id}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Library", "Home");
            }
            else return RedirectToAction("UpdateBook", "Home");
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

        [HttpPost]
        public async Task<IActionResult> DeleteState(Guid inputId)
        {
            var responce = await RequestsToServer<State>.SendDelete($"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl/{inputId}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("weigthControl", "Home");
            }
            else return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(Guid inputId)
        {
            var responce = await RequestsToServer<Book>.SendDelete($"https://localhost:5001/api/users/{MainUser.GetId()}/library/{inputId}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Library", "Home");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}