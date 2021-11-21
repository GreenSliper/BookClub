using System.Linq;
using AutoMapper;

namespace DAL.Models.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile(IModelMapper<DTO.DBImage, Image> imageMapper)
		{
			CreateMap<DTO.DBImage, Image>().ConvertUsing(x => imageMapper.ToVM(x));
			CreateMap<Image, DTO.DBImage>().ConvertUsing(x => imageMapper.ToDTO(x));

			CreateMap<Club, DTO.Club>();
			CreateMap<DTO.Club, Club>();

			CreateMap<Book, DTO.Book>();
			CreateMap<DTO.Book, Book>().ForMember(x => x.AverageRating, res=>res.MapFrom<BookRatingResolver>());

			CreateMap<ReadBook, DTO.ReadBook>();
			CreateMap<DTO.ReadBook, ReadBook>().ForMember(x => x.BookName, y => y.MapFrom(z => z.Book.Name));

			CreateMap<ClubBook, DTO.ClubBook>();
			CreateMap<DTO.ClubBook, ClubBook>();

			CreateMap<ClubMember, DTO.ClubMember>();
			CreateMap<DTO.ClubMember, ClubMember>();

			CreateMap<ClubDiscussion, DTO.ClubDiscussion>();
			CreateMap<DTO.ClubDiscussion, ClubDiscussion>();

			CreateMap<ClubDiscussionBook, DTO.ClubDiscussionBook>();
			CreateMap<DTO.ClubDiscussionBook, ClubDiscussionBook>();

			CreateMap<ClubInvite, DTO.ClubInvite>();
			CreateMap<DTO.ClubInvite, ClubInvite>();

			CreateMap<Ban, DTO.Ban>();
			CreateMap<DTO.Ban, Ban>();
		}
	}
}
