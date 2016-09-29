using System;
using FriceEngine;
using FriceEngine.Animation;
using FriceEngine.Object;
using FriceEngine.Resource;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Message;
using FriceEngine.Utils.Misc;
using FriceEngine.Utils.Time;

namespace FriceEngineTest
{
	public static class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			// ReSharper disable once ObjectCreationAsStatement
//			Application.Run(new Test2());
			//Application.Run(new Test());
			new Test2();
		}
	}

	public class Test : Game
	{
		private ImageResource _file;
		private FTimer _timer;
		private ImageObject _b;

		public override void OnInit()
		{
			_timer = new FTimer(10);
			_file = ImageResource.FromFile(@"C:\frice.png");
			SetBounds(300, 300, 800, 600);

			SetTitle("Fuck the world");

			//replace with a file path in desk
			_b = ImageObject.FromFile(@"C:\frice.png", 300, 400, 50, 50);
//			var c = ImageObject.FromWeb("https://avatars1.githubusercontent.com/u/21008243", 400, 300);

			//can resize：
//			c.Height = 100;
//			c.Width = 100;
			_b.MoveList.Add(new SimpleMove(-10, -10));
//			c.MoveList.Add(new SimpleMove(-10, 10));
			AddObject(_b);
			AddObject(new SimpleText(ColorResource.高坂穗乃果, "Hello World", 10, 10));
//			AddObject(c);
		}

		private void Add()
		{
			var a = new ImageObject(_file.Bmp, Mouse.X - 50, Mouse.Y - 50)
			{
				Width = 100,
				Height = 100
			};
			a.TargetList.Add(new Pair<PhysicalObject, Action>(_b, () => a.MoveList.Add(new SimpleMove(0, -400))));
			a.MoveList.Add(new SimpleMove(0, -400));
			a.MoveList.Add(new AccelerateMove(0, 1000));
			AddObject(a);
		}

		public override void OnClick(EventArgs eventArgs, FPoint mousePosition)
		{
			FLog.D("On click called.");
			Add();
			base.OnClick(eventArgs, mousePosition);
		}

		public override void OnRefresh()
		{
			if (_timer.Ended())
				Add();
			base.OnRefresh();
		}
	}

	public class Test2 : WpfGame
	{
		ImageObject _x = ImageObject.FromWeb("https://avatars1.githubusercontent.com/u/21008243", 0, 0, 50, 50);

		public override void OnInit()
		{
			AddObject(new SimpleText(ColorResource.赤羽业, "蛤蛤蛤", 10, 10));
			for (var i = 0; i < 1000; i++)
			{
				var o = _x.Clone();
				o.X = 400;
				o.Y = 300;
				RandomMove(o, 500);
				AddObject(o);
			}
		}

		private static void RandomMove(FObject obj, int time)
		{
			var ft2 = new FTimeListener2(time, true);
			ft2.OnTimeUp += () =>
			{
				obj.MoveList.Clear();
				obj.MoveList.Add(GetRandomMove());
			};
			ft2.Start();
		}

		private static int _seed;

		private static SimpleMove GetRandomMove()
		{
			var r = new Random(_seed);
			_seed = r.Next();
			var x = r.Next(-100, 100);
			var y = r.Next(-100, 100);
			return new SimpleMove(x, y);
		}
	}
}
