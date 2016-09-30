using System;
using FriceEngine;
using FriceEngine.Animation;
using FriceEngine.Object;
using FriceEngine.Resource;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Message;
using FriceEngine.Utils.Misc;
using FriceEngine.Utils.Time;
using System.Windows.Input;

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
//			_b.MoveList.Add(new SimpleMove(-10, -10));
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
	//		a.TargetList.Add(new Pair<PhysicalObject, Action>(_b, () => a.MoveList.Add(new SimpleMove(0, -400))));
//			a.MoveList.Add(new SimpleMove(0, -400));
//			a.MoveList.Add(new AccelerateMove(0, 1000));
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
		SimpleText _t = new SimpleText("Click",300,200);
		Random r = new Random();

		public override void OnInit()
		{
			TextObject t = new TextObject(ColorResource.Black, "Press any Key (keyboard or mouse)",30,400,200);
			AddObject(t);
		}


		public override void OnClick(double x,double y,int b)
		{
			FObject a;
			switch (b)
			{
				case 0:
					a = _x.Clone();
					break;
				case 2:
					a = ImageObject.FromFile(@"C:\frice.png", 0, 0, 50, 50);
					break;
				default:
					a = new ShapeObject(ColorResource.Black, new FCircle(50.0),0,0 );
					break;
			}
			a.X = x;
			a.Y = y;
			a.AddAnims(new SimpleMove(30, -500));
			a.AddAnims(new AccelerateMove(0, 800));
			AddObject(a);
		}

		public override void OnMouseMove(double x, double y)
		{
			_t.X = x+30;
			_t.Y = y;
			_t.Text = $"点击：, {x}, {y}";
			AddObject(_t);
		}

		public override void OnKeyDown(string key)
		{
			TextObject _to = new TextObject(ColorResource.赤羽业, "",50, r.Next(0,1000), 20);
			_to.Text = key;
			_to.AddAnims(new AccelerateMove(0,300));
			AddObject(_to);
		}

		
	}
}
