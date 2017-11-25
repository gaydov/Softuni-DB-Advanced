using System.Collections.Generic;

namespace PhotoShare.Models
{
    public class Album
    {
        public Album()
        {
            this.Pictures = new HashSet<Picture>();
            this.AlbumTags = new HashSet<AlbumTag>();
            this.AlbumRoles = new HashSet<AlbumRole>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public Color? BackgroundColor { get; set; }

        public bool IsPublic { get; set; }

        public ICollection<AlbumRole> AlbumRoles { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        public ICollection<AlbumTag> AlbumTags { get; set; }

        public override string ToString()
        {
            return $"{this.Name} has {this.Pictures.Count} pictures";
        }
    }
}
