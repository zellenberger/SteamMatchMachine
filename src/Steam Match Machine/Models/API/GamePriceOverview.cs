namespace Steam_Match_Machine.Models.API {
    public class GamePriceOverview {
        public string currency { get; set; }

        public int discount_percent { get; set; }

        public string initial_formatted { get; set; }

        public string final_formatted { get; set; }
    }
}