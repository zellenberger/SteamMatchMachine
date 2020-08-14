using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SteamMatchMachine.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameTags",
                columns: table => new
                {
                    GameTagId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameTagName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTags", x => x.GameTagId);
                });

            migrationBuilder.CreateTable(
                name: "QuizAnswers",
                columns: table => new
                {
                    AnswerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAnswers", x => x.AnswerId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoGames",
                columns: table => new
                {
                    steam_appid = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsArchived = table.Column<bool>(nullable: false),
                    ProductLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoGames", x => x.steam_appid);
                });

            migrationBuilder.CreateTable(
                name: "QuizAnswerGameTags",
                columns: table => new
                {
                    AnswerId = table.Column<int>(nullable: false),
                    GameTagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAnswerGameTags", x => new { x.AnswerId, x.GameTagId });
                    table.ForeignKey(
                        name: "FK_QuizAnswerGameTags_QuizAnswers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "QuizAnswers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizAnswerGameTags_GameTags_GameTagId",
                        column: x => x.GameTagId,
                        principalTable: "GameTags",
                        principalColumn: "GameTagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQuizAnswers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    QuizAnswerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuizAnswers", x => new { x.UserId, x.QuizAnswerId });
                    table.ForeignKey(
                        name: "FK_UserQuizAnswers_QuizAnswers_QuizAnswerId",
                        column: x => x.QuizAnswerId,
                        principalTable: "QuizAnswers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuizAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameTagVideoGames",
                columns: table => new
                {
                    VideoGameId = table.Column<int>(nullable: false),
                    GameTagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTagVideoGames", x => new { x.VideoGameId, x.GameTagId });
                    table.ForeignKey(
                        name: "FK_GameTagVideoGames_GameTags_GameTagId",
                        column: x => x.GameTagId,
                        principalTable: "GameTags",
                        principalColumn: "GameTagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTagVideoGames_VideoGames_VideoGameId",
                        column: x => x.VideoGameId,
                        principalTable: "VideoGames",
                        principalColumn: "steam_appid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVideoGames",
                columns: table => new
                {
                    VideoGameId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVideoGames", x => new { x.VideoGameId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserVideoGames_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVideoGames_VideoGames_VideoGameId",
                        column: x => x.VideoGameId,
                        principalTable: "VideoGames",
                        principalColumn: "steam_appid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWishlists",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    VideoGameId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWishlists", x => new { x.VideoGameId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserWishlists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWishlists_VideoGames_VideoGameId",
                        column: x => x.VideoGameId,
                        principalTable: "VideoGames",
                        principalColumn: "steam_appid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 1, "RPG" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 29, "Mature" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 28, "No Support" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 27, "Exploration" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 26, "Post-apocalyptic" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 25, "FPS" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 24, "Looter Shooter" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 23, "VR" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 22, "Controller" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 21, "Horror" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 20, "Historical" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 18, "Fantasy" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 17, "Sci-fi" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 16, "Family Friendly" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 19, "Medieval" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 14, "Sports" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 2, "Adventure" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 3, "Casual" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 15, "Violence" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 5, "Open World" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 6, "Free to Play" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 7, "Multiplayer" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 4, "Action" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 9, "Open World Survival Craft" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 10, "Space" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 11, "Singleplayer" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 12, "Co-Op" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 13, "Strategy" });

            migrationBuilder.InsertData(
                table: "GameTags",
                columns: new[] { "GameTagId", "GameTagName" },
                values: new object[] { 8, "Survival" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 11, "Sci-Fi" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 18, "VR Headset" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 17, "Controller Support" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 16, "No Support" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 15, "Horror" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 14, "Historical" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 13, "Medieval" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 12, "Fantasy" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 10, "No" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 2, "Co-op" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 8, "Sports" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 7, "Strategy" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 6, "RPG" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 5, "Adventure" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 4, "Action" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 3, "Multiplayer" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 1, "Singleplayer" });

            migrationBuilder.InsertData(
                table: "QuizAnswers",
                columns: new[] { "AnswerId", "Answer" },
                values: new object[] { 9, "Yes" });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 275850, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 1245950, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 242760, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 292030, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 812140, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 374320, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 489830, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 271590, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 1174180, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 377160, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 8980, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 431240, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 470220, false, null });

            migrationBuilder.InsertData(
                table: "VideoGames",
                columns: new[] { "steam_appid", "IsArchived", "ProductLink" },
                values: new object[] { 105600, false, null });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 1174180, 4 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 271590, 5 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 271590, 7 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 271590, 29 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 271590, 4 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 470220, 3 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 470220, 7 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 470220, 16 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 374320, 7 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 374320, 1 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 812140, 20 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 812140, 11 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 812140, 22 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 292030, 5 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 292030, 1 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 1174180, 11 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 292030, 11 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 292030, 29 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 242760, 8 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 242760, 9 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 242760, 5 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 1245950, 23 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 1245950, 19 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 275850, 5 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 275850, 10 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 275850, 9 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 275850, 23 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 431240, 7 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 431240, 14 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 431240, 16 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 105600, 9 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 292030, 2 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 1174180, 29 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 105600, 12 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 1174180, 2 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 105600, 8 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 8980, 1 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 8980, 12 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 8980, 24 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 8980, 25 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 377160, 5 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 8980, 4 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 377160, 26 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 377160, 27 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 489830, 5 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 489830, 1 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 489830, 2 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 489830, 11 });

            migrationBuilder.InsertData(
                table: "GameTagVideoGames",
                columns: new[] { "VideoGameId", "GameTagId" },
                values: new object[] { 377160, 1 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 8, 14 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 2, 12 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 3, 7 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 4, 4 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 5, 2 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 6, 1 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 7, 13 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 9, 15 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 16, 28 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 10, 16 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 11, 17 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 12, 18 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 13, 19 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 14, 20 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 15, 21 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 17, 22 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 18, 23 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 9, 29 });

            migrationBuilder.InsertData(
                table: "QuizAnswerGameTags",
                columns: new[] { "AnswerId", "GameTagId" },
                values: new object[] { 1, 11 });

            migrationBuilder.CreateIndex(
                name: "IX_GameTagVideoGames_GameTagId",
                table: "GameTagVideoGames",
                column: "GameTagId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswerGameTags_GameTagId",
                table: "QuizAnswerGameTags",
                column: "GameTagId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAnswers_QuizAnswerId",
                table: "UserQuizAnswers",
                column: "QuizAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVideoGames_UserId",
                table: "UserVideoGames",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWishlists_UserId",
                table: "UserWishlists",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTagVideoGames");

            migrationBuilder.DropTable(
                name: "QuizAnswerGameTags");

            migrationBuilder.DropTable(
                name: "UserQuizAnswers");

            migrationBuilder.DropTable(
                name: "UserVideoGames");

            migrationBuilder.DropTable(
                name: "UserWishlists");

            migrationBuilder.DropTable(
                name: "GameTags");

            migrationBuilder.DropTable(
                name: "QuizAnswers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VideoGames");
        }
    }
}
