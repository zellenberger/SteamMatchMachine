using System;

namespace Steam_Match_Machine.Models
{
    // The class which is used to represent the user's video game recommendations.
    public class UserVideoGame
    {
        // Gets or sets the video game.
        public VideoGame VideoGame { get; set; }

        // Gets or sets the video game id.
        public int VideoGameId { get; set; }

        // Gets or sets the user.
        public User User { get; set; }

        // Gets or sets the user id of the user.
        public int UserId {get; set;}

        // Gets or sets the date the user video game was added.
        public DateTime DateAdded {get; set;}
    }
}