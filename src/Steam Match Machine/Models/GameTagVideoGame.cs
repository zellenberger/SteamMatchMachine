namespace Steam_Match_Machine.Models
{
    // The class which is used to represent the join table between the game and game tags.
    public class GameTagVideoGame
    {
        // Gets or sets the video game id.
        public int VideoGameId { get; set; }

        // Gets or sets the video game.
        public VideoGame VideoGame { get; set; }

        public int GameTagId { get; set; }

        // Gets or sets the video game tag.
        public GameTag GameTag { get; set; }
    }
}