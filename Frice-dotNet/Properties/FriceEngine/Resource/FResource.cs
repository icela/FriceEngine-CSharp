using System.Drawing;

namespace Frice_dotNet.Properties.FriceEngine.Resource
{
	public interface IFResource
	{
	}

	public class ColorResource
	{
		public Color Color;

		public ColorResource(Color color)
		{
			Color = color;
		}

		public static readonly ColorResource Blue = new ColorResource(Color.Blue);
		public static readonly ColorResource Red = new ColorResource(Color.Red);
		public static readonly ColorResource Green = new ColorResource(Color.Green);
	}
}
