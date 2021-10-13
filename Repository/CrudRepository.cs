using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
	/// <summary>
	/// Generic CRUD repository base
	/// </summary>
	public abstract class CrudRepository<ContextT, ModelT> : IRepository<ModelT>
		where ModelT: class
		where ContextT : DbContext
	{
		private readonly ContextT context;
		protected DbSet<ModelT> entities;

		public CrudRepository(ContextT context)
		{
			this.context = context;
			entities = context.Set<ModelT>();
		}

		protected static bool CheckNull(ModelT input)
		{
			if (input == null)
				throw new ArgumentNullException("input entity was null");
			return true;
		}

		public virtual async Task Insert(ModelT entity)
		{
			CheckNull(entity);
			await entities.AddAsync(entity);
			await SaveChanges();
		}

		public virtual async Task Delete(ModelT entity)
		{
			CheckNull(entity);
			entities.Remove(entity);
			await SaveChanges();
		}

		public virtual async Task Update(ModelT entity)
		{
			CheckNull(entity);
			entities.Update(entity);
			await SaveChanges();
		}
		public virtual async Task<IEnumerable<ModelT>> GetAll()
		{
			return await entities.ToListAsync();
		}

		public async Task SaveChanges()
		{
			await context.SaveChangesAsync();
		}

		public abstract Task<ModelT> Get(int id);
	}
}
