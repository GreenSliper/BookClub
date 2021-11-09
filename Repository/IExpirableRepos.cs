using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public interface IExpirableRepos<ModelT, KeyT> : IRepository<ModelT, KeyT> where ModelT : class, IExpirable
	{
		Task<IEnumerable<ModelT>> GetNotExpired(IEnumerable<ModelT> collection);
		Task<ModelT> GetNotExpired(ModelT entry);
	}
}
