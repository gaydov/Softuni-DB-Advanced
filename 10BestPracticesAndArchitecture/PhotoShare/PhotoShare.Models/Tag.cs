using System.Collections.Generic;
using PhotoShare.Models.Validation;

namespace PhotoShare.Models
{
    public class Tag
    {
        public Tag()
        {
            this.AlbumTags = new HashSet<AlbumTag>();
        }

        public int Id { get; set; }

        [Tag]
        public string Name { get; set; }

        public ICollection<AlbumTag> AlbumTags { get; set; }

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
