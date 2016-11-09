using FriceEngine.Object;
using FriceEngine.Utils.Time;

namespace FriceEngine.Animation
{
	public abstract class MoveAnim : FAnim
	{
		public abstract DoublePair Delta { get; }
	}

	public class SimpleMove : MoveAnim
	{
		public SimpleMove(int y, int x)
		{
			Y = y;
			X = x;
		}

		public SimpleMove(double y, double x)
		{
			Y = y;
			X = x;
		}

		public double X { get; }
		public double Y { get; }

		/// <summary>
		/// 感谢ifdog老司机帮我修改这个问题。。。
		/// </summary>
		/// <returns>the distance you should move.</returns>
		public override DoublePair Delta
		{
			get
			{
				Now = Clock.Current;
				var d = DoublePair.FromTicks((Now - Last) * X, (Now - Last) * Y);
				Last = Clock.Current;
				return d;
			}
		}
	}

	/// <summary>
	/// a move class with more accurate parameters.
	/// </summary>
	public class AccurateMove : MoveAnim
	{
		public AccurateMove(int y, int x)
		{
			Y = y;
			X = x;
		}

		public double X { get; set; }
		public double Y { get; set; }

		public override DoublePair Delta
		{
			get
			{
				Now = Clock.Current;
				var d = DoublePair.FromTicks((Now - Last) * X, (Now - Last) * Y);
				Last = Clock.Current;
				return d;
			}
		}
	}

	/// <summary>
	/// full of bugs. Orz
	/// </summary>
	public class AccelerateMove : MoveAnim
	{
		public AccelerateMove(double y, double x)
		{
			X = x;
			Y = y;
		}

		public double X { get; set; }
		public double Y { get; set; }

		private double _mx;
		private double _my;

		/// <summary>
		/// same algorithm as JVM Frice.
		/// </summary>
		public override DoublePair Delta
		{
			get
			{
				_mx = (Now - Start) * X / 2;
				_my = (Now - Start) * Y / 2;
				Now = Clock.Current;
				// not sure
				var d = DoublePair.FromTicks((Now - Last) * _mx / 1e7, (Now - Last) * _my / 1e7);
				Last = Clock.Current;
				return d;
			}
		}
	}
}