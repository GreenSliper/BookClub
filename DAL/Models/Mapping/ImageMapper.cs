using DAL.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Mapping
{
	public interface IImageMapper
	{
		DBImage ToDBImage(Image image);
		Image ToImage(DBImage image);
	}
	public class ImageMapper : IImageMapper
	{
		public DBImage ToDBImage(Image image)
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

		public Image ToImage(DBImage image)
		{
			if (image == null)
				return null;
			if (image.ImageData != null)
				return new Image() { URL = $"data:image / jpg; base64,{ Convert.ToBase64String(image.ImageData)}" };
			return new Image();
		}
	}
}
