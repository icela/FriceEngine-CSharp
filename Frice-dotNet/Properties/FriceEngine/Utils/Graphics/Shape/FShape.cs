namespace Frice_dotNet.Properties.FriceEngine.Utils.Graphics.Shape
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

	public class FOval : FRectangle
	{
		/// <summary> this class represents the shape oval. </summary>
		/// <param name="rh"> radius horizontal. </param>
		/// <param name="rv"> radius vertical. </param>
		public FOval(double rh, double rv) : base(rh + rh, rv + rv)
		{
		}
	}

	public class FCircle : FOval
	{
		/// <summary> this class represents the shape circle. </summary>
		/// <param name="r"> radius. </param>
		public FCircle(double r) : base(r, r)
		{
		}
	}
}
