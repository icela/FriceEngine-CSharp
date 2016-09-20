using Frice_dotNet.Properties.FriceEngine.Resource;
using Frice_dotNet.Properties.FriceEngine.Utils.Graphics;

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

	public interface ICollideBox
	{
		bool IsCollide(ICollideBox other);
	}

	public abstract class PhysicalObject : IAbstractObject, IFContainer
	{
		public virtual double X { get; set; }
		public virtual double Y { get; set; }

		public virtual double Width { get; set; }
		public virtual double Height { get; set; }

		public double Rotate { get; set; } = 0;

		public bool Died { get; set; } = false;

		private double _mass = 1;

		public double Mass
		{
			get { return _mass; }
			set { _mass = value <= 0 ? 0.001 : value; }
		}
	}

	public abstract class FObject : PhysicalObject
	{
		public bool ContainsPoint(double px, double py) => px >= X && px <= X + Width && py >= Y && py <= Y + Height;
		public bool ContainsPoint(int px, int py) => px >= X && px <= X + Width && py >= Y && py <= Y + Height;
	}

	public sealed class ShapeObject : FObject
	{
		public IFShape Shape;
		public ColorResource ColorResource;

		public override double Width
		{
			get { return Shape.Width; }
			set { Shape.Width = value; }
		}

		public override double Height
		{
			get { return Shape.Height; }
			set { Shape.Height = value; }
		}

		public ShapeObject(ColorResource colorResource, IFShape shape, double x, double y)
		{
			ColorResource = colorResource;
			Shape = shape;
			X = x;
			Y = y;
		}
	}

	public class ImageObject : FObject
	{
	}

	public class DoublePair
	{
		public double X;
		public double Y;

		public DoublePair(double y, double x)
		{
			Y = y;
			X = x;
		}
	}
}
