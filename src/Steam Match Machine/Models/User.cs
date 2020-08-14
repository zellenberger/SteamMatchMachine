using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Steam_Match_Machine.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        // Gets or sets the list of user video games.
        public List<UserVideoGame> UserVideoGames { get; set; }

        // Gets or sets the user's selected quiz answers.
        public List<UserQuizAnswer> UserQuizAnswers { get; set; }

        // Gets or sets the user's wish list.
        public List<UserWishlist> UserWishlist { get; set; }
    }
}