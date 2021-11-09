using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	/// <summary>
	/// Get and GetAll operations are guaranteed to return only non-expired entries. 
	/// All expired entries are removed from db asyncronously.
	/// Use GetNotExpired to check entries from related props (in other DTO-s)
	/// </summary>
	/// <typeparam name="ModelT">IExpirable model</typeparam>
	public abstract class ExpirableCrudRepos<ContextT, ModelT, KeyT> : CrudRepository<ContextT, ModelT, KeyT>,
		IExpirableRepos<ModelT, KeyT>
		where ContextT : Microsoft.EntityFrameworkCore.DbContext
		where ModelT : class, IExpirable
	{
		public ExpirableCrudRepos(ContextT context) : base(context) { }

		protected async Task<ModelT> GetOrRemoveExpired(ModelT entry)
		{
			if (entry!=null && entry.IsExpired)
			{
				await Delete(entry);
				return null;
			}
			return entry;
		}
		protected async IAsyncEnumerable<ModelT> GetCollectionRemoveExpired(IEnumerable<ModelT> source)
		{
			foreach (var inv in source)
			{
				var processed = await GetOrRemoveExpired(inv);
				if (processed != null)
					yield return processed;
			}
		}
		public override async Task<IEnumerable<ModelT>> GetAll()
		{
			return await GetCollectionRemoveExpired(await base.GetAll()).ToListAsync();
		}

		public async Task<IEnumerable<ModelT>> GetNotExpired(IEnumerable<ModelT> collection)
		{
			return await GetCollectionRemoveExpired(collection).ToListAsync();
		}

		public async Task<ModelT> GetNotExpired(ModelT entry)
		{
			return await GetOrRemoveExpired(entry);
		}
	}
}
