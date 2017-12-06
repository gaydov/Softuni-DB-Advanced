using System.Collections.Generic;

namespace Instagraph.DataProcessor.DTOs
{
    public class UserWithComments
    {
        public string Username { get; set; }

        public ICollection<int> PostsCommentsCount { get; set; }
    }
}