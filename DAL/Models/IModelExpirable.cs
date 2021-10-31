using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public interface IModelExpirable
	{
		public DateTime ExpirationTime { get; }
	}
}
