using Microsoft.AspNetCore.Mvc;
using MyLifeClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Controllers
{
    public class StateController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var result = await RequestsToServer<IEnumerable<State>>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl");
            return View(result);
        }

        public IActionResult CreateState()
        {
            return View(new InputState(Guid.NewGuid(), DateTime.Today, 60, 0, 3));
        }

        [HttpPost]
        public async Task<IActionResult> CreateState(InputState inputState)
        {
            var weigth = Convert.ToDouble(inputState.MainWeigth + "," + inputState.PartialWeight);
            var state = new State(inputState.Id, MainUser.GetId(), inputState.Date, weigth, inputState.Mood);
            var responce = await RequestsToServer<State>.SendPost(state, $"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "State");
            }
            else return RedirectToAction("CreateState", "State");
        }

        public async Task<IActionResult> UpdateState(Guid inputId)
        {
            var state = await RequestsToServer<State>.SendGet($"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl/{inputId}");
            var mainWeigth = Convert.ToInt32(Math.Truncate(state.Weigth));
            var partialWeigth = Convert.ToInt32((state.Weigth - mainWeigth) * 10);
            var modelState = new InputState(state.Id, state.Date, mainWeigth, partialWeigth, state.Mood);
            return View(modelState);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateState(InputState inputState)
        {
            var weigth = Convert.ToDouble(inputState.MainWeigth + "," + inputState.PartialWeight);
            var state = new State(inputState.Id, MainUser.GetId(), inputState.Date, weigth, inputState.Mood);
            var responce = await RequestsToServer<State>.SendPut(state, $"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl/{state.Id}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "State");
            }
            else return RedirectToAction("UpdateState", "State");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteState(Guid inputId)
        {
            var responce = await RequestsToServer<State>.SendDelete($"https://localhost:5001/api/users/{MainUser.GetId()}/weigthControl/{inputId}");
            if (responce.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "State");
            }
            else return RedirectToAction("Index", "Home");
        }
    }
}
