using System.Collections.Generic;

namespace Steam_Match_Machine.Models.API
{
    public class GameDetailsResponse
    {
        public GameDetails data { get; set; }

        public bool InWishlist { get; set;  } 
    }
}