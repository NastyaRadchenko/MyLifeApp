using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLifeClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Controllers
{
    public class DiaryController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var result = await RequestsToServer<IEnumerable<DiaryEntry>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/diary");
            return View(result);
        }

        public async Task<IActionResult> DiaryEntry(Guid inputId)
        {
            var entry = await RequestsToServer<DiaryEntry>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/diary/{inputId}");
            return View(entry);
        }

        public IActionResult CreateDiaryEntry()
        {
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
                return RedirectToAction("Index", "Diary");
            }
            else return RedirectToAction("CreateDiaryEntry", "Diary");
        }

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
                return RedirectToAction("Index", "Diary");
            }
            else return RedirectToAction("UpdateDiaryEntry", "Diary");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDiaryEntry(Guid inputId)
        {
            var responce = await RequestsToServer<DiaryEntry>.SendDelete($"https://localhost:5001/api/users/{MainUser.GetId()}/diary/{inputId}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Diary");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}
