using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class ModelActionRequestResult<T> where T: class
	{
		public readonly bool successful;
		public readonly T requestedModel;

		public ModelActionRequestResult(bool successful, T requestedModel = null)
		{
			this.successful = successful;
			this.requestedModel = requestedModel;
		}
	}
}
