using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Media.Imaging;
using JetBrains.Annotations;

namespace FriceEngine.Resource
{
	public abstract class FManager<T>
	{
		[NotNull] public readonly IDictionary Res = new Dictionary<string, T>();

		[NotNull]
		public abstract T Create([NotNull] string path);

		[NotNull]
		public virtual T this[string path]
		{
			[NotNull]
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
		[NotNull]	public static ImageManger Instance { get; } = new ImageManger();

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
				return new Bitmap(imageStream, true);
		}

		public override Bitmap this[string path] => (Bitmap) base[path].Clone();
	}
}