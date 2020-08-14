namespace Steam_Match_Machine.Models
{
    // The class which is used to represent the user answer's associated game tags
    public class QuizAnswerGameTag
    {
        // Gets or sets the quiz answer id.
        public int AnswerId { get; set; }

        // Gets or sets the quiz answers.
        public  QuizAnswer QuizAnswer {get; set;}

        // Gets or sets the game tag id.
        public int GameTagId { get; set; }

        // Gets or sets the game tag.
        public GameTag GameTag { get; set; }
    }
}