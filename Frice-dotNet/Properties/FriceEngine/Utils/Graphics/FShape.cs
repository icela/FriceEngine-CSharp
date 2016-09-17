namespace Frice_dotNet.Properties.FriceEngine.Utils.Graphics
{
	public interface IFShape
	{
		double Width { get; set; }
		double Height { get; set; }
	}

	public class FRectangle : IFShape
	{
		public double Width { get; set; }
		public double Height { get; set; }

		public FRectangle(double width, double height)
		{
			Width = width;
			Height = height;
		}
	}

	public class FOval : IFShape
	{
		/// <summary> this class represents the shape oval. </summary>
		/// <param name="rh"> radius horizontal. </param>
		/// <param name="rv"> radius vertical. </param>
		public FOval(double rh, double rv)
		{
			Width = rh + rh;
			Height = rv + rv;
		}

		public double Width { get; set; }
		public double Height { get; set; }
	}

	public class FCircle : FOval
	{
		/// <summary> this class represents the shape circle. </summary>
		/// <param name="r"> radius. </param>
		public FCircle(double r) : base(r, r)
		{
		}
	}

	/// <summary> like a data class in Kotlin. </summary>
	public class FPoint
	{
		public double X;
		public double Y;

		public FPoint(double y, double x)
		{
			Y = y;
			X = x;
		}
	}
}
