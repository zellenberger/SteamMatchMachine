using System.Collections.Generic;

namespace Steam_Match_Machine.Models
{
    public class UserProfileViewModel
    {
        public List<VideoGame> Recs { get; set; }

        public List<VideoGame> Wishlist { get; set; }
    }
}