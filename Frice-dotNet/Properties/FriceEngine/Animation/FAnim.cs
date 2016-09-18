namespace Frice_dotNet.Properties.FriceEngine.Animation
{
	public abstract class FAnim
	{
		protected readonly double Start = System.DateTime.Now.Millisecond;

		protected double Now => System.DateTime.Now.Millisecond;
	}
}
