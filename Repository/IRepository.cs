using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
	public interface IRepository<ModelT, KeyT>
	{
		Task<IEnumerable<ModelT>> GetAll();
		Task Insert(ModelT model);
		Task Update(ModelT model);
		Task Delete(ModelT model);
		Task<ModelT> Get(KeyT id);
		Task SaveChanges();
	}
}
