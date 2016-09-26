using System;
using System.Net.Mime;
using System.Threading;
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
			// Application.Run(new Test2());
		}
	}

	public class Test : Game
	{
		public override void OnInit()
		{
			SetBounds(300, 300, 800, 600);

			SetTitle("Fuck the world");

			//replace with a file path in desk
			var b = ImageObject.FromFile(@"C:\frice.png", 300, 400, 50, 50);
//			var c = ImageObject.FromWeb("https://avatars1.githubusercontent.com/u/21008243", 400, 300);

			//can resize：
//			c.Height = 100;
//			c.Width = 100;
			b.MoveList.Add(new SimpleMove(-10, -10));
//			c.MoveList.Add(new SimpleMove(-10, 10));
			AddObject(b);
			AddObject(new SimpleText(ColorResource.高坂穗乃果, "Hello World", 10, 10));
//			AddObject(c);
		}

		private void Add()
		{
			var a = ImageObject.FromFile(@"C:\frice.png", Mouse.X - 50, Mouse.Y - 50, 100, 100);
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

		public override void OnRefresh()
		{
			Add();
			base.OnRefresh();
		}
	}

	public class Test2 : Game
	{
		public override void OnInit()
		{
			Width = 800;
			Height = 600;

			var a = new ShapeObject(ColorResource.吾王蓝, new FCircle(40), 300, 200);
			//replace with a file path in disk
			var b = ImageObject.FromFile(@"C:\frice.png", 300, 200, 100, 100);
			var c = ImageObject.FromWeb("https://avatars1.githubusercontent.com/u/21008243", 300, 200, 100, 100);
			AddObjects(a,b,c);
			RandomMove(a,1000);
			RandomMove(b,1500);
			RandomMove(c,750);

		}

		public void RandomMove(FObject obj,int time)
		{
			FTimeListener2 ft2 = new FTimeListener2(time, true);
			ft2.OnTimeUp += () =>
			{
				obj.MoveList.Clear();
				obj.MoveList.Add(GetRandomMove());
			};
			ft2.Start();
		}

		public static SimpleMove GetRandomMove()
		{
			Random r = new Random();
			int x = r.Next(-100,100);
			Random r2 = new Random(x);
			int y = r.Next(-100, 100);

			return new SimpleMove(x,y);
		}
	}
}