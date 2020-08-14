using System.Collections.Generic;

namespace Steam_Match_Machine.Models
{
    // The class which is used to represent the quiz answers.
    public class QuizAnswer
    {
        // Gets or sets the quiz answer id.
        public int AnswerId { get; set; }

        // Gets or sets the quiz answers.
        public string Answer { get; set; }

        // Gets or sets the answer's related game tags.
        public List<QuizAnswerGameTag> QuizAnswerGameTags { get; set; }

        // Gets or sets the user's quiz answers.
        public List<UserQuizAnswer> UserQuizAnswers { get; set; }
    }
}