using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Steam_Match_Machine.Models;
using Steam_Match_Machine.Models.API;

namespace Steam_Match_Machine.Services
{
    public class DataService
    {

        private readonly DataContext _dataContext;

        private readonly SteamApi _steamApi;

        // Initializes a new instance of the DataService class.
        public DataService(DataContext dataContext, SteamApi steamApi)
        {
            _dataContext = dataContext;
            _steamApi = steamApi;
        }

        // The method that is used to add videogames to the application.
        public void AddVideoGame(VideoGame videoGame)
        {
            _dataContext.VideoGames.Add(videoGame);
            _dataContext.SaveChanges();
        }

        // The method used to archive a selected video game.
        public void DeleteVideoGame(int videoGameId)
        {
            // Get the specified video game to be deleted.
            VideoGame videoGame = GetVideoGame(videoGameId);

            // Set the game's is archived property to true.
            videoGame.IsArchived = true;

            // Update the database to reflect the changes.
            _dataContext.VideoGames.Update(videoGame);
            _dataContext.SaveChanges();
        }

        // The method that is used to get the list of video game recommendations for the specified user.
        public List<VideoGame> GetRecommendations(int userId)
        {
            List<GameTagVideoGame> allGtvg = new List<GameTagVideoGame>();
            List<VideoGame> videoGames = new List<VideoGame>();

            // Get the specified user.
            User user = GetUser(userId);

            //For every user quiz answer in the specified user's selected quiz answers...
            foreach (UserQuizAnswer userQuizAnswer in _dataContext.UserQuizAnswers
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToList())
            {

                // Get the game tags associated with the user's answers.
                List<GameTag> gameTags = GetRecommendationGameTags(userId);

                // For each game tag in the recommended game tags.
                foreach (GameTag gameTag in gameTags)
                {
                    // Get the game tag video games, with the matching game tag ids.
                    List<GameTagVideoGame> gtvg = _dataContext.GameTagVideoGames
                        .AsNoTracking()
                        .Where(x => x.GameTagId == gameTag.GameTagId)
                        .ToList();

                    // For every game tag video game in the game tag video game list for the specified game tag...
                    foreach (GameTagVideoGame gtagVg in gtvg)
                    {
                        // Set the the game tag property equal to the game tag with the matching id.
                        gtagVg.GameTag = gameTag;

                        // Add the gtvg to the all game tag video games list.
                        allGtvg.Add(gtagVg);
                    }
                }
            }

            // Foreach game tag video game in all of the game tag video games for the specified tags...
            foreach (GameTagVideoGame gt in allGtvg)
            {

                int videoGameId = gt.VideoGameId;
                VideoGame videoGame = _dataContext.VideoGames
                    .AsNoTracking()
                    .Where(x => x.steam_appid == videoGameId)
                    .FirstOrDefault();

                videoGame.GameTagVideoGames = new List<GameTagVideoGame>();
                videoGame.GameTagVideoGames = _dataContext.GameTagVideoGames
                    .AsNoTracking()
                    .Where(x => x.VideoGameId == videoGame.steam_appid)
                    .ToList();

                foreach (GameTagVideoGame g in videoGame.GameTagVideoGames)
                {
                    g.GameTag = _dataContext.GameTags
                        .AsNoTracking()
                        .Where(x => x.GameTagId == g.GameTagId)
                        .FirstOrDefault();
                }

                // Add the video game to the list of video games if there is no list of video game recommendations.
                if (videoGames.Count == 0)
                {
                    videoGames.Add(videoGame);
                }

                // Determine whether the video game already exists.
                bool exists = videoGames.Exists(x => x.steam_appid == videoGameId);

                // If exists is equal to false...
                if (exists == false)
                {
                    // Add the video game to the list of video games for the user's video game recommendations.
                    videoGames.Add(videoGame);
                }

            }

            // For every video game in the user's recommended video games list...
            foreach (VideoGame vg in videoGames)
            {
                // Create a new user video game relationship.
                UserVideoGame userVideoGame = new UserVideoGame();

                // Set the video game id to the current video game id.
                userVideoGame.VideoGameId = vg.steam_appid;

                // Set the user id to the current user's id.
                userVideoGame.UserId = userId;

                // Set the date added.
                userVideoGame.DateAdded = DateTime.Now;

                // Add the user video game relationship to the UserVideoGames table in the database.
                // This table can be used to store all user video game recommendations.
                AddUserVideoGame(userVideoGame);
            }

            return videoGames;
        }

        // The method that is used to get the recommended video games game tags, based on the user's selcted answers.
        public List<GameTag> GetRecommendationGameTags(int userId)
        {
            List<int> userSelectedAnswers = new List<int>();
            List<int> gameTagIds = new List<int>();

            // For every user quiz answer record with the specified user id...
            foreach (UserQuizAnswer uqa in _dataContext.UserQuizAnswers.Where(x => x.UserId == userId).ToList())
            {
                // Get the current quiz answer id.
                int quizAnswerId = uqa.QuizAnswerId;

                // Add the answer id to the user's selcted answers list.
                userSelectedAnswers.Add(quizAnswerId);
            }

            List<GameTag> recommendedGameTags = new List<GameTag>();

            // For each answer id in the user's selected answers list.
            foreach (int quizAnswerId in userSelectedAnswers)
            {
                // Get the quiz answer game tag that is associated with the specified quiz answer.
                QuizAnswerGameTag qagt = _dataContext.QuizAnswerGameTags.Where(x => x.AnswerId == quizAnswerId).FirstOrDefault();

                // Add the quiz answer game tag to the recommended game tags list.
                gameTagIds.Add(qagt.GameTagId);
            }

            // For every game tag id in the game tag ids list.
            foreach (int gameTagId in gameTagIds)
            {
                // Get the actual game tag.
                GameTag gameTag = _dataContext.GameTags.Where(x => x.GameTagId == gameTagId).FirstOrDefault();

                recommendedGameTags.Add(gameTag);
            }

            return recommendedGameTags;
        }

        // The method that is used to add a new user video game record to the user video games table in the database.
        public void AddUserVideoGame(UserVideoGame userVideoGame)
        {
            // Get the current user's list of video games.
            List<UserVideoGame> userVideoGames = _dataContext.UserVideoGames.Where(x => x.UserId == userVideoGame.UserId).ToList();

            // Determine whether the video game already exists.
            bool exists = userVideoGames.Exists(x => x.VideoGameId == userVideoGame.VideoGameId);

            // If exists is equal to false...
            if (exists == false)
            {
                // Add the user video game to the users video games database table.
                _dataContext.UserVideoGames.Add(userVideoGame);
                _dataContext.SaveChanges();
            }
        }

        // Gets the previously recommended video games for the user.
        public List<UserVideoGame> GetUserVideoGames(int userId)
        {
            return _dataContext.UserVideoGames
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.DateAdded < DateTime.Now)
            .ToList();
        }

        // Adds the user wish list to the user's current wishlist.
        public void AddUserWishlist(int userId, int videoGameId)
        {
            // Get the current user's list of video games in the wish list.
            List<UserWishlist> userWishlists = _dataContext.UserWishlists
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToList();

            // Determine whether the video game already exists.
            bool exists = userWishlists.Exists(x => x.VideoGameId == videoGameId);

            // If the video game does not exist in the specified user's wish listg... 
            if (exists == false)
            {
                // Create a new userWishlist record.
                UserWishlist userWishlist = new UserWishlist();

                // Set the user wish list record's user id to the current user.
                userWishlist.UserId = userId;

                // Set the user wish list record's video game id, to the specified video game.
                userWishlist.VideoGameId = videoGameId;

                List<VideoGame> videoGames = GetVideoGames();

                // If the video game does not already exist in the video game table...
                bool existing = videoGames.Exists(x => x.steam_appid == videoGameId);
                if (existing == false) {
                    VideoGame vg = new VideoGame();
                    vg.steam_appid = videoGameId;
                    _dataContext.VideoGames.Add(vg);
                }

                // Add the user wish list to the userWishists database table.
                _dataContext.UserWishlists.Add(userWishlist);
                _dataContext.SaveChanges();
            }
        }

        public bool IsGameInWishlist(int userId, int gameId)
        {
            return _dataContext.UserWishlists.Any(x => x.UserId == userId & x.VideoGameId == gameId);
        }

        // Gets the user's wishlist.
        public List<VideoGame> GetUserWishlist(int userId)
        {
            List<UserWishlist> userWishlists = _dataContext.UserWishlists
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToList();

            List<VideoGame> videoGames = new List<VideoGame>();

            userWishlists?.ForEach(userWishlistItem =>
            {
                videoGames.Add(new VideoGame()
                {
                    steam_appid = userWishlistItem.VideoGameId
                });
            });
            return videoGames;
        }

        // The method that is used to get the specified video game.
        public VideoGame GetVideoGame(int id)
        {
            return _dataContext.VideoGames
                .AsNoTracking()
                .Where(x => x.steam_appid == id)
                .FirstOrDefault();
        }

        // Gets the generic list of video games in the database.
        public List<VideoGame> GetVideoGames()
        {
            // Get the list of video games.
            List<VideoGame> videoGames = _dataContext.VideoGames
                .AsNoTracking()
                .Where(x => x.IsArchived == false)
                .ToList();

            // Initialize a new instance of a list of game tags.
            List<GameTag> gameTags = new List<GameTag>();

            // For each video game in the video games list...
            foreach (VideoGame vg in videoGames)
            {
                // Get the game tag video game relationship for the specified video game.
                vg.GameTagVideoGames = _dataContext.GameTagVideoGames
                    .AsNoTracking()
                    .Where(x => x.VideoGameId == vg.steam_appid)
                    .ToList();

                // For every game tag video game relationship...
                foreach (GameTagVideoGame gtv in vg.GameTagVideoGames)
                {
                    // Get the game tags where that match the game tag id.
                    gtv.GameTag = _dataContext.GameTags
                        .AsNoTracking()
                        .Where(x => x.GameTagId == gtv.GameTagId)
                        .FirstOrDefault();
                }
            }

            return videoGames;
        }

        // The method which is used to update the specified video game.
        public void UpdateVideoGame(VideoGame videoGame)
        {
            _dataContext.VideoGames.Update(videoGame);
            _dataContext.SaveChanges();
        }

        // The method that is used to add a user to the user database.
        public void AddUser(User user)
        {
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();
        }

        // The method that is used to get the specified user.
        public User GetUser(string email)
        {
            return _dataContext.Users
                .AsNoTracking()
                .Where(x => x.EmailAddress.ToLower() == email.ToLower())
                .FirstOrDefault();
        }

        // Gets the user by id.
        public User GetUser(int id)
        {
            return _dataContext.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);
        }

        // Updates the specified user information.
        public void UpdateUser(User u)
        {
            _dataContext.Users.Update(u);
            _dataContext.SaveChanges();
        }

        // Gets the game tag video games.
        public List<GameTagVideoGame> GetGameTagVideoGames(int gameTagId)
        {
            return _dataContext.GameTagVideoGames
                .Where(x => x.GameTagId == gameTagId)
                .ToList();
        }

        // Gets the stored quiz answer.
        public QuizAnswer GetQuizAnswer(string answer)
        {
            return _dataContext.QuizAnswers
                .Where(x => x.Answer == answer).FirstOrDefault();
        }

        // Gets the list of the user's specified quiz answers.
        public List<UserQuizAnswer> GetUserQuizAnswers(int userId)
        {
            return _dataContext.UserQuizAnswers
                .Where(x => x.UserId == userId).ToList();
        }

        // Adds the user's quiz answers to the database.
        public void AddUserQuizAnswer(UserQuizAnswer userQuizAnswer)
        {
            _dataContext.UserQuizAnswers.Add(userQuizAnswer);
            _dataContext.SaveChanges();
        }

        // Deletes the specified user quiz answers.
        public void DeleteUserQuizAnswer(int userId)
        {
            // For every record in the UserQuizAnswers database set. 
            foreach (var q in _dataContext.UserQuizAnswers)
            {
                // If the existing user exists...
                if (q.UserId == userId)
                {
                    // Remove them from UserQuizAnswers database set.
                    _dataContext.UserQuizAnswers.Remove(q);
                    _dataContext.SaveChanges();
                }
            }
        }

        // Gets the associated game tags associated with the selected answers.
        public List<QuizAnswerGameTag> GetQuizAnswerGameTags(int quizAnswerId)
        {
            return _dataContext.QuizAnswerGameTags
                .Where(x => x.AnswerId == quizAnswerId).ToList();
        }
    }
}