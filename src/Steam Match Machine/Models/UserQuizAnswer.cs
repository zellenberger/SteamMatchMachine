namespace Steam_Match_Machine.Models
{
    public class UserQuizAnswer
    {
        // Gets or sets the specified user.
        public int UserId { get; set; }

        // Gets or sets the user.
        public User User { get; set; }

        // Gets or sets the quiz answer id.
        public int QuizAnswerId { get; set; }

        // Gets or sets the quiz answer.
        public QuizAnswer QuizAnswer { get; set; }
    }
}