using System.Windows.Forms;
using FriceEngine;
using FriceEngine.Animation;
using FriceEngine.Object;
using FriceEngine.Resource;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Message;

namespace FriceEngineTest
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			// ReSharper disable once ObjectCreationAsStatement
			Application.Run(new Test());
		}
	}

	public class Test : Game
	{
		public override void OnInit()
		{
			Width = 800;
			Height = 600;

			SetTitle("Fuck the world");

			//replace with a file path in desk
			var b = ImageObject.FromFile(@"C:\frice.png", 300, 400, 50, 50);
//            var c = ImageObject.FromWeb("https://avatars1.githubusercontent.com/u/21008243", 400, 300);

			//can resize：
//            c.Height = 100;
//            c.Width = 100;
			b.MoveList.Add(new SimpleMove(-10, -10));
//            c.MoveList.Add(new SimpleMove(-10, 10));
			AddObject(b);
			AddObject(new SimpleText(ColorResource.高坂穗乃果, "Hello World", 10, 10));
//            AddObject(c);
		}

		private void Add()
		{
			var a = new ShapeObject(ColorResource.吾王蓝, new FCircle(40), Mouse.X - 50, Mouse.Y - 50)
			{
				Height = 100,
				Width = 100
			};
			a.MoveList.Add(new SimpleMove(100, -400));
			a.MoveList.Add(new AccelerateMove(0, 1000));
			AddObject(a);
		}

		public override void OnClick(FPoint mousePosition)
		{
			FLog.D("On click called.");
			Add();
			base.OnClick(mousePosition);
		}
	}
}