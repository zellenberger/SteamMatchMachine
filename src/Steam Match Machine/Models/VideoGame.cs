using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace Steam_Match_Machine.Models
{
    // The class which is used to represent a video game.
    public class VideoGame
    {
        // The field that is used to represent the primary key of the video game.
        public int steam_appid { get; set; }


        // Gets or sets the video game's cover image.
        [NotMapped]
        public string header_image { get; set; }

        // Gets or sets the video game's small image.
        [NotMapped]
        public string small_capsule_image { get; set; }

        [Required]
        public bool IsArchived { get; set; }

        //Set a thumnnail image property, not mapped.
        [NotMapped]
        public string thumbnail_img { get; set; }

        [NotMapped]
        // The field that is used to represent the video game's name.
        public string name { get; set; }

        // Gets or sets the video games game tags.
        public List<GameTagVideoGame> GameTagVideoGames { get; set; }

        // Gets or sets the list user video games.
        public List<UserVideoGame> UserVideoGames { get; set; }

        // Gets or sets the user wish list video games.
        public List<UserWishlist> UserWishList { get; set; }

        [MinLength(20, ErrorMessage = "The description must be longer than 20 characters.")]
        [DataType(DataType.MultilineText)]
        [NotMapped]
        // The field that is used to represent the video game's description.
        public string short_description { get; set; }

        // The field that is used to represent the video game's product link to Steam.
        public string ProductLink { get; set; }

        // The field that is used to represent the video game's price.
        [NotMapped]
        [Range(.01, 1000000)]
        [DataType(DataType.Currency)]
        public string final_formatted { get; set; }

        // Initializes a new instance of the video game class.
        public VideoGame()
        {

        }
    }
}