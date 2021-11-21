using AutoMapper;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Mapping
{
	class BookRatingResolver : IValueResolver<DTO.Book, Models.Book, float>
	{
		public float Resolve(DTO.Book source, Book destination, float destMember, ResolutionContext context)
		{
			var ratings = source.ReadBy?.Where(x => x.Rating.HasValue)?.Select(x => x.Rating.Value);
			destination.RatingCount = ratings.Count();
			return (float)(destination.RatingCount == 0 ? 0 : ratings.Average());
		}
	}
}
