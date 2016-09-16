namespace Frice_dotNet.Properties.FriceEngine.Object
{
	public interface IAbstractObject
	{
		double X { get; set; }
		double Y { get; set; }

		double Rotate { get; set; }
	}

	public interface IFContainer
	{
		double Width { get; set; }
		double Height { get; set; }
	}

	public abstract class FObject : IAbstractObject, IFContainer
	{
		public abstract double X { get; set; }
		public abstract double Y { get; set; }
		public double Rotate { get; set; } = 0.0;
		public double Width { get; set; }
		public double Height { get; set; }

		public bool ContainsPoint(double px, double py) => px >= X && px <= X + Width && py >= Y && py <= Y + Height;
		public bool ContainsPoint(int px, int py) => px >= X && px <= X + Width && py >= Y && py <= Y + Height;
	}

	public class ShapeObject : FObject
	{
		public override double X { get; set; }
		public override double Y { get; set; }
		public
	}
}
