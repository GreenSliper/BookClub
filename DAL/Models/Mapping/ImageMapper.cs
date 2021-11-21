using DAL.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Mapping
{
	public interface IModelMapper<DTOT, VMT>
		where DTOT : class
		where VMT : class
	{
		DTOT ToDTO(VMT model);
		VMT ToVM(DTOT model);
	}
	public class ImageMapper : IModelMapper<DTO.DBImage, Image>
	{
		public DBImage ToDTO(Image image)
		{
			if (image == null)
				return null;
			DBImage img = new DBImage();
			using (MemoryStream ms = new MemoryStream())
			{
				//TODO add async
				image.File.CopyTo(ms);
				img.ImageData = ms.ToArray();
				ms.Close();
			}
			return img;
		}

		public Image ToVM(DBImage image)
		{
			if (image == null)
				return null;
			if (image.ImageData != null)
				return new Image() { URL = $"data:image / jpg; base64,{ Convert.ToBase64String(image.ImageData)}" };
			return new Image();
		}
	}
}
