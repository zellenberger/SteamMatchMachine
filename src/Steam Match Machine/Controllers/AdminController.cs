using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steam_Match_Machine.Models;
using Steam_Match_Machine.Models.API;
using Steam_Match_Machine.Services;

namespace Steam_Match_Machine.Controllers {
    // The class which is used to represent the administrative controller
    [Authorize (Roles = "Admin")]
    public class AdminController : Controller {
        private readonly ILogger<AdminController> _logger;

        private readonly DataContext _dataContext;

        private readonly SteamApi _steamAPI;

        // The field that is used to represent the app's data service.
        private readonly DataService _dataService;

        public AdminController (ILogger<AdminController> logger, DataContext dataContext, SteamApi steamApi) {
            _logger = logger;
            _steamAPI = steamApi;
            _dataService = new DataService (dataContext, steamApi);
        }

        [Authorize (Roles = "Admin")]
        [HttpGet, Route ("admin/addvideogame")]
        public IActionResult AddVideoGame () {
            return View ();
        }

        [Authorize (Roles = "Admin")]
        [HttpPost, Route ("admin/addvideogame")]
        public IActionResult AddVideoGame (VideoGameViewModel VideoGameViewModel) {
            if (!ModelState.IsValid) {
                VideoGameViewModel model = new VideoGameViewModel ();

                return View (model);
            }

            _dataService.AddVideoGame (VideoGameViewModel.VideoGame);

            return RedirectToAction ("VideoGames");
        }

        [HttpGet, Route("delete-videogame/{steam_appid:int}")]
        public IActionResult DeleteVideoGameConfirm(int steam_appid) {
            VideoGame videoGame = _dataService.GetVideoGame(steam_appid);
            videoGame = _steamAPI.SetVideoGameDetails(videoGame);

            return View(videoGame);
        }

        [HttpPost, Route("delete-videogame/{steam_appid:int}")]
        public IActionResult DeleteVideoGame(int steam_appid) {
            // Delete the specified game.
            _dataService.DeleteVideoGame(steam_appid);

            return RedirectToAction("VideoGames");
        }

        [Authorize (Roles = "Admin")]
        [Route ("admin/videogames")]
        public IActionResult VideoGames () {
            // Need logic to return the list of brands.
            List<VideoGame> model = _dataService.GetVideoGames();

            model = _steamAPI.SetVideoGameDetails(model);

            return View (model);
        }
    }
}