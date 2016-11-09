using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;

namespace FriceEngine.Resource
{
	public abstract class FManager<T>
	{
		public readonly IDictionary Res = new Dictionary<string, T>();

		public abstract T Create(string path);

		public virtual T this[string path]
		{
			get
			{
				if (Res.Contains(path)) return (T) Res[path];
				var ret = Create(path);
				Res.Add(path, ret);
				return ret;
			}
		}
	}

	/// <summary>
	/// 我只能这样实现单例了，感觉好丑陋
	/// </summary>
	public class ImageManger : FManager<Bitmap>
	{
		private static ImageManger _instance;

		public static ImageManger Instance
		{
			get { return _instance ?? (_instance = new ImageManger()); }
			set { _instance = value; }
		}

		public override Bitmap Create(string path) => (Bitmap) Image.FromFile(path);

		public override Bitmap this[string path] => (Bitmap) base[path].Clone();
	}


	/// <summary>
	/// 我只能这样实现单例了，感觉好丑陋
	/// </summary>
	public class WebImageManger : FManager<Bitmap>
	{
		private static WebImageManger _instance;

		public static WebImageManger Instance
		{
			get { return _instance ?? (_instance = new WebImageManger()); }
			set { _instance = value; }
		}

		public override Bitmap Create(string path)
		{
			var r = WebRequest.Create(path).GetResponse() as HttpWebResponse;
			using (var imageStream = r?.GetResponseStream())
			{
				var img = imageStream == null ? null : new Bitmap(imageStream, true);
				return img;
			}
		}

		public override Bitmap this[string path] => (Bitmap) base[path].Clone();
	}
}