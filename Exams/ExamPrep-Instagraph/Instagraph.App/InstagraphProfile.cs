using System.Linq;
using AutoMapper;
using Instagraph.DataProcessor.DTOs;
using Instagraph.Models;

namespace Instagraph.App
{
    public class InstagraphProfile : Profile
    {
        public InstagraphProfile()
        {
            this.CreateMap<Post, UncommentedPostDto>()
                .ForMember(dto => dto.User,
                    opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dto => dto.Picture,
                    opt => opt.MapFrom(src => src.Picture.Path));

            this.CreateMap<User, PopularUserDto>()
                .ForMember(dto => dto.Followers,
                opt => opt.MapFrom(src => src.Followers.Count));

            this.CreateMap<User, UserWithComments>()
                .ForMember(dto => dto.Username,
                opt => opt.MapFrom(src => src.Username))
                .ForMember(dto => dto.PostsCommentsCount,
                opt => opt.MapFrom(src => src.Posts.Select(p => p.Comments.Count).ToArray()));
        }
    }
}
