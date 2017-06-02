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
	/// 觉得丑陋就写简单一点啊
	/// </summary>
	public class ImageManger : FManager<Bitmap>
	{
	    public static ImageManger Instance { get; } = new ImageManger();

	    public override Bitmap Create(string path) => (Bitmap) Image.FromFile(path);

		public override Bitmap this[string path] => (Bitmap) base[path].Clone();
	}


	/// <summary>
	/// 我随便写了个能用的，应该比复制代码要好
	/// </summary>
	public class WebImageManger : FManager<Bitmap>
	{
	    public static WebImageManger Instance { get; } = new WebImageManger();

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