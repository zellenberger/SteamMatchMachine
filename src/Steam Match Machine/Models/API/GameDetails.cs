using System.Collections.Generic;
using Steam_Match_Machine.Models.API;

namespace Steam_Match_Machine.Models {
    public class GameDetails {
        public int steam_appid { get; set; }

        public string name { get; set; }

        public string required_age { get; set; }

        public bool is_free { get; set; }

        public string about_the_game { get; set; }

        public string short_description { get; set; }

        public string header_image { get; set; }

        public GamePriceOverview price_overview { get; set; }

        public GamePlatforms platforms { get; set; }

        public List<GameCategories> categories { get; set; }

        public List<GameGenres> genres { get; set; }

        public List<GameScreenshots> screenshots { get; set; }

        public string background { get; set; }
    }
}