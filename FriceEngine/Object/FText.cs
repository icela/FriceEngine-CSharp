using FriceEngine.Resource;

namespace FriceEngine.Object
{
	public class TextObject : FObject
	{
		public string Text;
		public double Size;
		public ColorResource ColorResource;
		public override double X { get; set; }
		public override double Y { get; set; }

		public TextObject(ColorResource colorResource, string text, double size, double x, double y)
		{
			ColorResource = colorResource;
			Text = text;
			X = x;
			Y = y;
			Size = size;
		}

		public ColorResource GetColor() => ColorResource;

		public override string ToString()
		{
			return Text;
		}
	}
}