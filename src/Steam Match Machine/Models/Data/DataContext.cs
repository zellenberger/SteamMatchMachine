using Microsoft.EntityFrameworkCore;

namespace Steam_Match_Machine.Models {
    // The class which is used to represent the data context.
    public class DataContext : DbContext {

        // The database set of video game recommendations.
        public DbSet<VideoGame> VideoGames { get; set; }

        // Gets or sets the database's Users table.
        public DbSet<User> Users { get; set; }

        // Gets or sets the database's GameTags table.
        public DbSet<GameTag> GameTags { get; set; }

        // Gets or sets the database's GameTagVideoGames table.
        public DbSet<GameTagVideoGame> GameTagVideoGames { get; set; }

        public DbSet<QuizAnswerGameTag> QuizAnswerGameTags { get; set; }

        public DbSet<QuizAnswer> QuizAnswers { get; set; }

        // Gets or sets the database's user quiz.
        public DbSet<UserQuizAnswer> UserQuizAnswers { get; set; }

        // Gets or sets the database's user video game bridge table used for the user's recommendations.
        public DbSet<UserVideoGame> UserVideoGames { get; set; }

        // Gets or sets the databaase's user wish list.
        public DbSet<UserWishlist> UserWishlists { get; set; }

        // Initializes a new instance of the data context class.
        public DataContext (DbContextOptions<DataContext> options) : base (options) { }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            // Define the composite key for user quiz answer.
            modelBuilder.Entity<UserQuizAnswer> ()
                .HasKey (uq => new { uq.UserId, uq.QuizAnswerId });

            modelBuilder.Entity<VideoGame> ()
                .HasKey (p => new { p.steam_appid });

            // Define the relationship between the user quiz answer and user.
            modelBuilder.Entity<UserQuizAnswer> ()
                .HasOne (uq => uq.User)
                .WithMany (u => u.UserQuizAnswers)
                .HasForeignKey (uq => uq.UserId);

            // Define the relationship between the user quiz answer and the quiz answer.
            modelBuilder.Entity<UserQuizAnswer> ()
                .HasOne (uq => uq.QuizAnswer)
                .WithMany (qa => qa.UserQuizAnswers)
                .HasForeignKey (uq => uq.QuizAnswerId);

            // Define the composite key for GameTagVideoGames.
            modelBuilder.Entity<GameTagVideoGame> ()
                .HasKey (p => new { p.VideoGameId, p.GameTagId });

            // Define the relationship between the GameTagVideoGames and the Video Game.
            modelBuilder.Entity<GameTagVideoGame> ()
                .HasOne (gtv => gtv.VideoGame)
                .WithMany (v => v.GameTagVideoGames)
                .HasForeignKey (gtv => gtv.VideoGameId);

            // Define the relationship between the GameTagVideoGames and the Game Tags.
            modelBuilder.Entity<GameTagVideoGame> ()
                .HasOne (gtv => gtv.GameTag)
                .WithMany (gt => gt.GameTagVideoGames)
                .HasForeignKey (gtv => gtv.GameTagId);

            // Define the composite key for UserVideoGame.
            modelBuilder.Entity<UserVideoGame> ()
                .HasKey (p => new { p.VideoGameId, p.UserId });

            // Define the relationship between the UserVideoGame and the Video Game.
            modelBuilder.Entity<UserVideoGame> ()
                .HasOne (uvg => uvg.VideoGame)
                .WithMany (v => v.UserVideoGames)
                .HasForeignKey (uvg => uvg.VideoGameId);

            // Define the relationship between the UserVideoGame and the User.
            modelBuilder.Entity<UserVideoGame> ()
                .HasOne (uvg => uvg.User)
                .WithMany (u => u.UserVideoGames)
                .HasForeignKey (uvg => uvg.UserId);

            // Define the composite key for User Wishlist.
            modelBuilder.Entity<UserWishlist> ()
                .HasKey (p => new { p.VideoGameId, p.UserId });

            // Define the relationship between the UserWishlist and the Video Game.
            modelBuilder.Entity<UserWishlist> ()
                .HasOne (uw => uw.VideoGame)
                .WithMany (v => v.UserWishList)
                .HasForeignKey (uw => uw.VideoGameId);

            // Define the relationship between the UserWishlist and the User.
            modelBuilder.Entity<UserWishlist> ()
                .HasOne (uw => uw.User)
                .WithMany (u => u.UserWishlist)
                .HasForeignKey (uw => uw.UserId);

            // Define the composite key for QuizAnswerGameTag.
            modelBuilder.Entity<QuizAnswer> ()
                .HasKey (p => new { p.AnswerId });

            // Define the composite key for QuizAnswerGameTag.
            modelBuilder.Entity<QuizAnswerGameTag> ()
                .HasKey (p => new { p.AnswerId, p.GameTagId });

            // Define the relationship between the Quiz Answer Game Tags and the Quiz Answer.
            modelBuilder.Entity<QuizAnswerGameTag> ()
                .HasOne (qag => qag.QuizAnswer)
                .WithMany (q => q.QuizAnswerGameTags)
                .HasForeignKey (qag => qag.AnswerId);

            // Define the relationship between the Quiz Answer Game Tags and the Game Tags.
            modelBuilder.Entity<QuizAnswerGameTag> ()
                .HasOne (qag => qag.GameTag)
                .WithMany (gt => gt.QuizAnswerGameTags)
                .HasForeignKey (qag => qag.GameTagId);

            modelBuilder.Entity<VideoGame> ().HasData (
                new VideoGame { steam_appid = 8980 },
                new VideoGame { steam_appid = 377160 },
                new VideoGame { steam_appid = 489830 },
                new VideoGame { steam_appid = 1174180 },
                new VideoGame { steam_appid = 271590 },
                new VideoGame { steam_appid = 470220 },
                new VideoGame { steam_appid = 374320 },
                new VideoGame { steam_appid = 812140 },
                new VideoGame { steam_appid = 292030 },
                new VideoGame { steam_appid = 242760 },
                new VideoGame { steam_appid = 1245950 },
                new VideoGame { steam_appid = 275850 },
                new VideoGame { steam_appid = 431240 },
                new VideoGame { steam_appid = 105600 }
            );

            // Specify the product category seed data.
            modelBuilder.Entity<GameTag> ().HasData (
                new GameTag { GameTagId = 1, GameTagName = "RPG" },
                new GameTag { GameTagId = 2, GameTagName = "Adventure" },
                new GameTag { GameTagId = 3, GameTagName = "Casual" },
                new GameTag { GameTagId = 4, GameTagName = "Action" },
                new GameTag { GameTagId = 5, GameTagName = "Open World" },
                new GameTag { GameTagId = 6, GameTagName = "Free to Play" },
                new GameTag { GameTagId = 7, GameTagName = "Multiplayer" },
                new GameTag { GameTagId = 8, GameTagName = "Survival" },
                new GameTag { GameTagId = 9, GameTagName = "Open World Survival Craft" },
                new GameTag { GameTagId = 10, GameTagName = "Space" },
                new GameTag { GameTagId = 11, GameTagName = "Singleplayer" },
                new GameTag { GameTagId = 12, GameTagName = "Co-Op" },
                new GameTag { GameTagId = 13, GameTagName = "Strategy" },
                new GameTag { GameTagId = 14, GameTagName = "Sports" },
                new GameTag { GameTagId = 15, GameTagName = "Violence" },
                new GameTag { GameTagId = 16, GameTagName = "Family Friendly" },
                new GameTag { GameTagId = 17, GameTagName = "Sci-fi" },
                new GameTag { GameTagId = 18, GameTagName = "Fantasy" },
                new GameTag { GameTagId = 19, GameTagName = "Medieval" },
                new GameTag { GameTagId = 20, GameTagName = "Historical" },
                new GameTag { GameTagId = 21, GameTagName = "Horror" },
                new GameTag { GameTagId = 22, GameTagName = "Controller" },
                new GameTag { GameTagId = 23, GameTagName = "VR" },
                new GameTag { GameTagId = 24, GameTagName = "Looter Shooter" },
                new GameTag { GameTagId = 25, GameTagName = "FPS" },
                new GameTag { GameTagId = 26, GameTagName = "Post-apocalyptic" },
                new GameTag { GameTagId = 27, GameTagName = "Exploration" },
                new GameTag { GameTagId = 28, GameTagName = "No Support" },
                new GameTag { GameTagId = 29, GameTagName = "Mature" }
            );

            // Specify the product category product seed data.
            modelBuilder.Entity<GameTagVideoGame> ().HasData (
                new GameTagVideoGame { VideoGameId = 8980, GameTagId = 1 },
                new GameTagVideoGame { VideoGameId = 8980, GameTagId = 12 },
                new GameTagVideoGame { VideoGameId = 8980, GameTagId = 4 },
                new GameTagVideoGame { VideoGameId = 8980, GameTagId = 24 },
                new GameTagVideoGame { VideoGameId = 8980, GameTagId = 25 },
                new GameTagVideoGame { VideoGameId = 377160, GameTagId = 5 },
                new GameTagVideoGame { VideoGameId = 377160, GameTagId = 1 },
                new GameTagVideoGame { VideoGameId = 377160, GameTagId = 26 },
                new GameTagVideoGame { VideoGameId = 377160, GameTagId = 27 },
                new GameTagVideoGame { VideoGameId = 489830, GameTagId = 5 },
                new GameTagVideoGame { VideoGameId = 489830, GameTagId = 1 },
                new GameTagVideoGame { VideoGameId = 489830, GameTagId = 2 },
                new GameTagVideoGame { VideoGameId = 489830, GameTagId = 11 },
                new GameTagVideoGame { VideoGameId = 242760, GameTagId = 8 },
                new GameTagVideoGame { VideoGameId = 242760, GameTagId = 9 },
                new GameTagVideoGame { VideoGameId = 242760, GameTagId = 5 },
                new GameTagVideoGame { VideoGameId = 275850, GameTagId = 5 },
                new GameTagVideoGame { VideoGameId = 275850, GameTagId = 10 },
                new GameTagVideoGame { VideoGameId = 275850, GameTagId = 9 },
                new GameTagVideoGame { VideoGameId = 275850, GameTagId = 23 },
                new GameTagVideoGame { VideoGameId = 292030, GameTagId = 5 },
                new GameTagVideoGame { VideoGameId = 292030, GameTagId = 1 },
                new GameTagVideoGame { VideoGameId = 292030, GameTagId = 11 },
                new GameTagVideoGame { VideoGameId = 292030, GameTagId = 2 },
                new GameTagVideoGame { VideoGameId = 292030, GameTagId = 29 },
                new GameTagVideoGame { VideoGameId = 105600, GameTagId = 9 },
                new GameTagVideoGame { VideoGameId = 105600, GameTagId = 8 },
                new GameTagVideoGame { VideoGameId = 105600, GameTagId = 12 },
                new GameTagVideoGame { VideoGameId = 431240, GameTagId = 7 },
                new GameTagVideoGame { VideoGameId = 431240, GameTagId = 14 },
                new GameTagVideoGame { VideoGameId = 431240, GameTagId = 16 },
                new GameTagVideoGame { VideoGameId = 1174180, GameTagId = 2 },
                new GameTagVideoGame { VideoGameId = 1174180, GameTagId = 4 },
                new GameTagVideoGame { VideoGameId = 1174180, GameTagId = 29 },
                new GameTagVideoGame { VideoGameId = 1174180, GameTagId = 11 },
                new GameTagVideoGame { VideoGameId = 271590, GameTagId = 5 },
                new GameTagVideoGame { VideoGameId = 271590, GameTagId = 7 },
                new GameTagVideoGame { VideoGameId = 271590, GameTagId = 29 },
                new GameTagVideoGame { VideoGameId = 271590, GameTagId = 4 },
                new GameTagVideoGame { VideoGameId = 470220, GameTagId = 3 },
                new GameTagVideoGame { VideoGameId = 470220, GameTagId = 7 },
                new GameTagVideoGame { VideoGameId = 470220, GameTagId = 16 },
                new GameTagVideoGame { VideoGameId = 374320, GameTagId = 7 },
                new GameTagVideoGame { VideoGameId = 374320, GameTagId = 1 },
                new GameTagVideoGame { VideoGameId = 1245950, GameTagId = 23 },
                new GameTagVideoGame { VideoGameId = 1245950, GameTagId = 19 },
                new GameTagVideoGame { VideoGameId = 812140, GameTagId = 20 },
                new GameTagVideoGame { VideoGameId = 812140, GameTagId = 11 },
                new GameTagVideoGame { VideoGameId = 812140, GameTagId = 22 }
            );

            // Specify the quiz answer seed data.
            modelBuilder.Entity<QuizAnswer> ().HasData (
                new QuizAnswer { AnswerId = 1, Answer = "Singleplayer" },
                new QuizAnswer { AnswerId = 2, Answer = "Co-op" },
                new QuizAnswer { AnswerId = 3, Answer = "Multiplayer" },
                new QuizAnswer { AnswerId = 4, Answer = "Action" },
                new QuizAnswer { AnswerId = 5, Answer = "Adventure" },
                new QuizAnswer { AnswerId = 6, Answer = "RPG" },
                new QuizAnswer { AnswerId = 7, Answer = "Strategy" },
                new QuizAnswer { AnswerId = 8, Answer = "Sports" },
                new QuizAnswer { AnswerId = 9, Answer = "Yes" },
                new QuizAnswer { AnswerId = 10, Answer = "No" },
                new QuizAnswer { AnswerId = 11, Answer = "Sci-Fi" },
                new QuizAnswer { AnswerId = 12, Answer = "Fantasy" },
                new QuizAnswer { AnswerId = 13, Answer = "Medieval" },
                new QuizAnswer { AnswerId = 14, Answer = "Historical" },
                new QuizAnswer { AnswerId = 15, Answer = "Horror" },
                new QuizAnswer { AnswerId = 16, Answer = "No Support" },
                new QuizAnswer { AnswerId = 17, Answer = "Controller Support" },
                new QuizAnswer { AnswerId = 18, Answer = "VR Headset" }
            );

            // Specify the product category product seed data.
            modelBuilder.Entity<QuizAnswerGameTag> ().HasData (
                new QuizAnswerGameTag { AnswerId = 1, GameTagId = 11 },
                new QuizAnswerGameTag { AnswerId = 2, GameTagId = 12 },
                new QuizAnswerGameTag { AnswerId = 5, GameTagId = 2 },
                new QuizAnswerGameTag { AnswerId = 4, GameTagId = 4 },
                new QuizAnswerGameTag { AnswerId = 6, GameTagId = 1 },
                new QuizAnswerGameTag { AnswerId = 3, GameTagId = 7 },
                new QuizAnswerGameTag { AnswerId = 7, GameTagId = 13 },
                new QuizAnswerGameTag { AnswerId = 8, GameTagId = 14 },
                new QuizAnswerGameTag { AnswerId = 9, GameTagId = 15 },
                new QuizAnswerGameTag { AnswerId = 9, GameTagId = 29 },
                new QuizAnswerGameTag { AnswerId = 10, GameTagId = 16 },
                new QuizAnswerGameTag { AnswerId = 11, GameTagId = 17 },
                new QuizAnswerGameTag { AnswerId = 12, GameTagId = 18 },
                new QuizAnswerGameTag { AnswerId = 13, GameTagId = 19 },
                new QuizAnswerGameTag { AnswerId = 14, GameTagId = 20 },
                new QuizAnswerGameTag { AnswerId = 15, GameTagId = 21 },
                new QuizAnswerGameTag { AnswerId = 17, GameTagId = 22 },
                new QuizAnswerGameTag { AnswerId = 18, GameTagId = 23 },
                new QuizAnswerGameTag { AnswerId = 16, GameTagId = 28 }
            );

            base.OnModelCreating (modelBuilder);
        }
    }
}