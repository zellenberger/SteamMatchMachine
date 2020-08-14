using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Steam_Match_Machine.Models
{
    public class UserWishlist
    {
        [Required]
        public int UserId { get; set; }

        // Gets or sets the specified user.
        public User User { get; set; }

        // Gets or sets the video game id.
        [Required]
        public int VideoGameId {get; set;}

        // Gets or sets the specified video game.
        public VideoGame VideoGame { get; set; }
    }
}