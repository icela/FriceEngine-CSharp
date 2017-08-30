using System;
using System.Drawing;
using JetBrains.Annotations;

namespace FriceEngine.Resource
{
	public interface IFResource
	{
	}

	public sealed class ColorResource
	{
		public Color Color;

		public ColorResource(Color color)
		{
			Color = color;
		}

		public ColorResource(int argb) : this(Color.FromArgb(argb))
		{
		}

		public ColorResource([NotNull] string name) : this(Color.FromName(name))
		{
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ColorResource)) return false;
			return ((ColorResource) obj).Color.Equals(Color);
		}

		public bool Equals(ColorResource other) => Color.Equals(other.Color);

		// ReSharper disable once NonReadonlyMemberInGetHashCode
		public override int GetHashCode() => Color.GetHashCode();

		[NotNull]
		public static ColorResource From(int argb) => new ColorResource(argb);

		[NotNull]
		public static ColorResource From(Color color) => new ColorResource(color);

		[NotNull] public static readonly ColorResource Blue = new ColorResource(Color.Blue);
		[NotNull] public static readonly ColorResource Red = new ColorResource(Color.Red);
		[NotNull] public static readonly ColorResource Green = new ColorResource(Color.Green);
		[NotNull] public static readonly ColorResource Pink = new ColorResource(Color.Pink);
		[NotNull] public static readonly ColorResource Yellow = new ColorResource(Color.Yellow);
		[NotNull] public static readonly ColorResource Black = new ColorResource(Color.Black);
		[NotNull] public static readonly ColorResource White = new ColorResource(Color.White);
		[NotNull] public static readonly ColorResource Wheat = new ColorResource(Color.Wheat);
		[NotNull] public static readonly ColorResource Orange = new ColorResource(Color.Orange);
		[NotNull] public static readonly ColorResource Gray = new ColorResource(Color.Gray);
		[NotNull] public static readonly ColorResource DrakGray = new ColorResource(Color.DarkGray);
		[NotNull] public static readonly ColorResource ShitYellow = new ColorResource(Color.Gray);
		[NotNull] public static readonly ColorResource IntelliJIdea黑 = new ColorResource(0x2B2B2B);
		[NotNull] public static readonly ColorResource Colorless = new ColorResource(0);
		[NotNull] public static readonly ColorResource 小埋色 = new ColorResource(0xFFAC2B);
		[NotNull] public static readonly ColorResource 基佬紫 = new ColorResource(0x781895);
		[NotNull] public static readonly ColorResource 吾王蓝 = Blue;
		[NotNull] public static readonly ColorResource 教主黄 = Yellow;
		[NotNull] public static readonly ColorResource 宝强绿 = Green;
		[NotNull] public static readonly ColorResource 冰封绿 = 宝强绿;
		[NotNull] public static readonly ColorResource 高坂穗乃果 = Orange;
		[NotNull] public static readonly ColorResource 如果奇迹有颜色那么一定是橙色 = 高坂穗乃果;
		[NotNull] public static readonly ColorResource 南小鸟 = Gray;
		[NotNull] public static readonly ColorResource 园田海未 = 吾王蓝;
		[NotNull] public static readonly ColorResource 洵濑绘理 = new ColorResource(0x0FFFFF);
		[NotNull] public static readonly ColorResource 星空凛 = 教主黄;
		[NotNull] public static readonly ColorResource 西木野真姬 = Red;
		[NotNull] public static readonly ColorResource 东条希 = 基佬紫;
		[NotNull] public static readonly ColorResource 小泉花阳 = new ColorResource(0x1BA61C);
		[NotNull] public static readonly ColorResource 矢泽妮可 = Pink;
		[NotNull] public static readonly ColorResource 屎黄色 = ShitYellow;
		[NotNull] public static readonly ColorResource 天依蓝 = new ColorResource(0x66CCFF);
		[NotNull] public static readonly ColorResource 清真绿 = new ColorResource(0x038B43);
		[NotNull] public static readonly ColorResource 如果真爱有颜色那么一定是黄色 = 教主黄;
		[NotNull] public static readonly ColorResource 杀老师 = 如果真爱有颜色那么一定是黄色;
		[NotNull] public static readonly ColorResource 潮田渚 = 园田海未;
		[NotNull] public static readonly ColorResource 茅野枫 = 冰封绿;
		[NotNull] public static readonly ColorResource 赤羽业 = 西木野真姬;
	}

	public class ImageResource
	{
		[NotNull] public Bitmap Bitmap;

		public ImageResource([NotNull] Bitmap bitmap)
		{
			Bitmap = bitmap;
		}

		public ImageResource([NotNull] string path)
		{
			Bitmap = Uri.IsWellFormedUriString(path, UriKind.Absolute)
				? WebImageManger.Instance[path]
				: ImageManger.Instance[path];
		}

		[NotNull]
		public static ImageResource FromFile([NotNull] string path) => new ImageResource(path);
	}

	public sealed class WebImageResource : ImageResource
	{
		public WebImageResource(string path) : base(WebImageManger.Instance[path])
		{
		}
	}
}