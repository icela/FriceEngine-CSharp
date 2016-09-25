using FriceEngine.Resource;

namespace FriceEngine.Object
{
    public abstract class FText : IAbstractObject
    {
        public double Rotate { get; set; } = 0;

        public double X { get; set; }
        public double Y { get; set; }
        public string Text;

        public abstract ColorResource GetColor();
    }

    public class SimpleText : FText
    {
        public ColorResource Color;
        public override ColorResource GetColor() => Color;

        public SimpleText(ColorResource color, string text, double x, double y)
        {
            Color = color;
            X = x;
            Y = y;
            Text = text;
        }

        public SimpleText(string text, double x, double y)
        {
            Color = ColorResource.DrakGray;
            X = x;
            Y = y;
            Text = text;
        }
    }
}