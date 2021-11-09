using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public enum AccessErrors { None, NoAccess, NotPermitted, Banned, NotFound }
	public interface IRequestResult
	{
		public bool Success { get; }
	}

	public class ModelAccessResult<ModelT, ErrorModelT, ErrorT> : IRequestResult
		where ModelT: class
		where ErrorModelT : class
		where ErrorT : Enum
	{
		private bool success;
		public ModelT requestedModel;
		public ErrorModelT errorModel;
		public ErrorT error;

		public bool Success => success;

		ModelAccessResult(bool success, ModelT model = null, ErrorModelT errorModel = null)
		{
			this.success = success;
			requestedModel = model;
			this.errorModel = errorModel;
		}

		public ModelAccessResult(ModelT model) : this(true, model) { }
		public ModelAccessResult(ErrorT error) : this(false)
		{
			this.error = error;
		}
		public ModelAccessResult(ErrorModelT errorModel, ErrorT error) : this(false, errorModel: errorModel) 
		{
			this.error = error;
		}

		/// <summary>
		/// Set error type and remove requestedModel
		/// </summary>
		/// <returns></returns>
		public ModelAccessResult<ModelT, ErrorModelT, ErrorT> SetError(ErrorT error, ErrorModelT errorModel = null)
		{
			this.error = error;
			this.errorModel = errorModel;
			requestedModel = null;
			success = false;
			return this;
		}

		public ModelAccessResult<NewModelT, NewErrorModelT, ErrorT> Map<NewModelT, NewErrorModelT>(IMapper mapper)
			where NewErrorModelT: class
			where NewModelT: class
		{
			var result = new ModelAccessResult<NewModelT, NewErrorModelT, ErrorT>(
				success, mapper.Map<NewModelT>(requestedModel), mapper.Map<NewErrorModelT>(errorModel));
			result.error = error;
			return result;
		}
	}
}
