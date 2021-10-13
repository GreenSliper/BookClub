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

			CreateMap<ClubMember, DTO.ClubMember>();
			CreateMap<DTO.ClubMember, ClubMember>();
		}
	}
}
