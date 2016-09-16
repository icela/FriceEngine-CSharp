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
	}
}
