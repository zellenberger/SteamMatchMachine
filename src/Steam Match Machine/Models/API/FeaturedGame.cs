using System;
namespace Steam_Match_Machine.Models {
    public class FeaturedGame {
        private decimal finalPrice;

        public int id { get; set; }

        public string name { get; set; }

        public string large_capsule_image { get; set; }

        public string small_capsule_image { get; set; }

        public decimal final_price {

            get {
                return this.finalPrice;
            }
            set {
                if (value > 0) {
                    this.finalPrice = Math.Round (value / 100m, 2);
                } else {
                    this.finalPrice = 0.00m;
                }
            }
        }

        public string header_image { get; set; }
    }
}