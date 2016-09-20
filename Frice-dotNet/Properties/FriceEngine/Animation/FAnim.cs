using Frice_dotNet.Properties.FriceEngine.Object;

namespace Frice_dotNet.Properties.FriceEngine.Animation
{
	public abstract class FAnim
	{
		protected readonly double Start = System.DateTime.Now.Millisecond;

		protected double Now => System.DateTime.Now.Millisecond;
	}

	public abstract class MoveAnim
	{
		public abstract DoublePair GetDelta();
	}

	public class SimpleMove : MoveAnim
	{
		public override DoublePair GetDelta()
		{
		}
	}
}
