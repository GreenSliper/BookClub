using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
	public interface IRepository<ModelT>
	{
		Task<IEnumerable<ModelT>> GetAll();
		Task<ModelT> Get(int id);
		Task Insert(ModelT model);
		Task Update(ModelT model);
		Task Delete(ModelT model);
		Task SaveChanges();
	}
}
