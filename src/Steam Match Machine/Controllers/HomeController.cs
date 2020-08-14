using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
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

namespace SteamMatch.Controllers
{
    [Authorize]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DataContext _dataContext;

        // The field that is used to represent the app's data service.
        private readonly DataService _dataService;

        private readonly SteamApi _steamApi;

        // The field that is used to represent the user's selected quiz answers.
        private List<QuizAnswer> selectedAnswers;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext, SteamApi steamApi)
        {
            _logger = logger;
            _dataService = new DataService(dataContext, steamApi);
            _steamApi = steamApi;
        }

        [AllowAnonymous]
        [Route("")]
        public IActionResult Index()
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            User user = _dataService.GetUser(userId);
            dynamic gameModel = new ExpandoObject();
            FeaturedGameResponse featuredGameResponse = _steamApi.GetFeaturedGames();
            List<VideoGame> model = _dataService.GetRecommendations(userId);
            model = _steamApi.SetVideoGameDetails(model);
            FeaturedCategoriesResponseWrapper featuredCategories = _steamApi.GetFeaturedCategories();
            gameModel.RecGames = model;
            gameModel.FeaturedGames = featuredGameResponse.featured_win;
            gameModel.TopSellers = featuredCategories.top_sellers.items;
            return View(gameModel);
        }

        [AllowAnonymous]
        [Route("User")]
        public IActionResult MatchUser()
        {
            return View();
        }

        [Route("Quiz")]
        public IActionResult Quiz()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("Recommendations")]
        public IActionResult Recommendations(string sort, string filter)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            User user = _dataService.GetUser(userId);

            List<VideoGame> model = _dataService.GetRecommendations(userId);

            model = _steamApi.SetVideoGameDetails(model);


            if (!String.IsNullOrEmpty(filter))
            {
                model = model
                    .Where(p => p.name.ToLower().Contains(filter.ToLower()) ||
                       p.short_description.ToLower().Contains(filter.ToLower()) ||
                       p.GameTagVideoGames.Any(v => v.GameTag.GameTagName.ToLower().Contains(filter.ToLower())))
                    .ToList();
            }

            switch (sort)
            {
                case "title_desc":
                    model = model.OrderByDescending(x => x.name).ToList();
                    break;
                case "price_asc":
                    model = model.OrderBy(x => x.final_formatted).ToList();
                    break;
                case "price_desc":
                    model = model.OrderByDescending(x => x.final_formatted).ToList();
                    break;
                default:
                    model = model.OrderBy(x => x.name).ToList();
                    break;
            }

            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sort) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sort == "price_asc" ? "price_desc" : "price_asc";
            ViewData["Filter"] = filter;

            return View(model);
        }

        [AllowAnonymous]
        [Route("About")]
        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost, Route("SubmitQuiz")]
        public JsonResult SubmitQuiz([FromBody] List<string> selections)
        {
            // Get the current user's id.
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Get the current user by their id.
            User user = _dataService.GetUser(userId);

            // If the current user exists in the user quiz answer, delete the records.
            _dataService.DeleteUserQuizAnswer(userId);

            foreach (string answer in selections)
            {
                UserQuizAnswer userQuizAnswer = new UserQuizAnswer();

                QuizAnswer quizAnswer = _dataService.GetQuizAnswer(answer);

                userQuizAnswer.QuizAnswerId = quizAnswer.AnswerId;

                userQuizAnswer.UserId = user.Id;

                _dataService.AddUserQuizAnswer(userQuizAnswer);
            }

            user.UserQuizAnswers = _dataService.GetUserQuizAnswers(userId);

            return Json(null);
        }

        [AllowAnonymous]
        [HttpGet, Route("game/{id:int}")]
        public IActionResult Game(int id)
        {
            // Get the current user's id.
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            GameDetailsResponse gameDetailsResponse = _steamApi.GetGameDetails(id);

            if (User.Identity.IsAuthenticated)
            {
                //call dataservice to determine if game is in user wishlist
                gameDetailsResponse.InWishlist = _dataService.IsGameInWishlist(userId, id);
            }

            return View(gameDetailsResponse);
        }

        [HttpPost, Route("add-to-wishlist/{id:int}")]
        public IActionResult AddToWishlist(int id)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            _dataService.AddUserWishlist(userId, id);

            return RedirectToAction("Game", new { id = id });
        }
    }
}