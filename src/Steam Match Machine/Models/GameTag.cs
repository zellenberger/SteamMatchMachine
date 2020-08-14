using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Steam_Match_Machine.Models
{
    // The class which is used to represent a game GameTag.
    public class GameTag
    {
        // Gets or sets the video game GameTag id.
        public int GameTagId { get; set; }

        // Gets or sets the GameTag name.
        [Required]
        public string GameTagName {get; set;}

        // Gets or sets the game tag's video games.
        public List<GameTagVideoGame> GameTagVideoGames { get; set; }

        // Gets or sets the game tag's quiz answers.
        public List<QuizAnswerGameTag> QuizAnswerGameTags {get; set;}
    }
}