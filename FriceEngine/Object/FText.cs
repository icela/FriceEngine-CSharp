using FriceEngine.Resource;
using JetBrains.Annotations;

namespace FriceEngine.Object
{
	public class TextObject : FObject
	{
		public string Text;
		public double Size;
		[NotNull] public ColorResource ColorResource;
		public override double X { get; set; }
		public override double Y { get; set; }

		public TextObject(
			[NotNull] ColorResource colorResource,
			[NotNull] string text,
			double size,
			double x,
			double y)
		{
			ColorResource = colorResource;
			Text = text;
			X = x;
			Y = y;
			Size = size;
		}

		[NotNull]
		public ColorResource GetColor() => ColorResource;

		public override string ToString() => Text;
	}
}