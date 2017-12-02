using System;
using System.Collections.Generic;
using PhotoShare.Models.Validation;

namespace PhotoShare.Models
{
    public class User
    {
        public User()
        {
            this.FriendsAdded = new HashSet<Friendship>();
            this.AddedAsFriendBy = new HashSet<Friendship>();
            this.AlbumRoles = new HashSet<AlbumRole>();
        }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        
        public string Salt { get; set; }

        [Email]
        public string Email { get; set; }

        public int? ProfilePictureId { get; set; }

        public Picture ProfilePicture { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public int? BornTownId { get; set; }

        public Town BornTown { get; set; }

        public int? CurrentTownId { get; set; }

        public Town CurrentTown { get; set; }

        public DateTime? RegisteredOn { get; set; }

        public DateTime? LastTimeLoggedIn { get; set; }

        [Age]
        public int? Age { get; set; }

        public bool? IsDeleted { get; set; }

        public ICollection<Friendship> FriendsAdded { get; set; }

        public ICollection<Friendship> AddedAsFriendBy { get; set; }

        public ICollection<AlbumRole> AlbumRoles { get; set; }

        public override string ToString()
        {
            return $"{this.Username} {this.Email} {this.Age} {this.FullName}";
        }
    }
}
