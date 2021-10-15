using AutoMapper;

namespace DAL.Models.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Club, DTO.Club>();
			CreateMap<DTO.Club, Club>();

			CreateMap<Book, DTO.Book>();
			CreateMap<DTO.Book, Book>();

			CreateMap<ReadBook, DTO.ReadBook>();
			CreateMap<DTO.ReadBook, ReadBook>();

			CreateMap<ClubBook, DTO.ClubBook>();
			CreateMap<DTO.ClubBook, ClubBook>();

			CreateMap<ClubMember, DTO.ClubMember>();
			CreateMap<DTO.ClubMember, ClubMember>();

			CreateMap<ClubDiscussion, DTO.ClubDiscussion>();
			CreateMap<DTO.ClubDiscussion, ClubDiscussion>();

			CreateMap<ClubDiscussionBook, DTO.ClubDiscussionBook>();
			CreateMap<DTO.ClubDiscussionBook, ClubDiscussionBook>();
		}
	}
}
