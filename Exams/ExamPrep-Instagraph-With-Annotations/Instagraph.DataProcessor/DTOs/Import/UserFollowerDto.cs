using System.ComponentModel.DataAnnotations;

namespace Instagraph.DataProcessor.DTOs.Import
{
    public class UserFollowerDto
    {
        [Required]
        public string User { get; set; }

        [Required]
        public string Follower { get; set; }
    }
}