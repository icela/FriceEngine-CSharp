using Frice_dotNet.Properties.FriceEngine;
using Frice_dotNet.Properties.FriceEngine.Object;
using Frice_dotNet.Properties.FriceEngine.Resource;
using Frice_dotNet.Properties.FriceEngine.Utils.Graphics;

namespace Frice_dotNet
{
	public class Test : Game
	{
		public override void OnInit()
		{
			base.OnInit();
			AddObject(new ShapeObject(ColorResource.茅野枫, new FCircle(50), 100, 100));
		}

		public static void Main(string[] args)
		{
			// ReSharper disable once ObjectCreationAsStatement
			new Test();
		}
	}
}
