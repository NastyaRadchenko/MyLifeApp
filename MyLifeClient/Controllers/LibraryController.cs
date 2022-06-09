using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLifeClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Controllers
{
    public class LibraryController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var result = await RequestsToServer<IEnumerable<Book>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/library");
            return View(result);
        }

        public async Task<IActionResult> Book(Guid inputId)
        {
            var book = await RequestsToServer<Book>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/library/{inputId}");
            return View(book);
        }

        public async Task<IActionResult> CreateBook()
        {
            var result = await RequestsToServer<IEnumerable<Book>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/library/categories");
            ViewBag.Category = result;
            return View();
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
                return RedirectToAction("Index", "Library");
            }
            else return RedirectToAction("CreateBook", "Library");
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
        public async Task<IActionResult> UpdateBook(IFormFile imgFile, Book inputBook, bool shouldDelete)
        {
            if (shouldDelete)
                inputBook.Picture = null;
            if (imgFile != null)
                inputBook.Picture = ByteConvert.GetBytesFromFile(imgFile);
            var responce = await RequestsToServer<Book>.SendPut(inputBook, $"https://localhost:5001/api/users/{MainUser.GetId()}/library/{inputBook.Id}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Library");
            }
            else return RedirectToAction("UpdateBook", "Library");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(Guid inputId)
        {
            var responce = await RequestsToServer<Book>.SendDelete($"https://localhost:5001/api/users/{MainUser.GetId()}/library/{inputId}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Library");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}
