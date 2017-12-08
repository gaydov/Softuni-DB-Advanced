using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Instagraph.Models
{
    public class Post
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        [Required]
        public string Caption { get; set; }

        public int UserId { get; set; }

        [Required]
        public User User { get; set; }

        public int PictureId { get; set; }

        [Required]
        public Picture Picture { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}