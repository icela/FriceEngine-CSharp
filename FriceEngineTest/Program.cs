using System;
using System.Windows.Forms;
using FriceEngine;
using FriceEngine.Animation;
using FriceEngine.Object;
using FriceEngine.Resource;
using FriceEngine.Utils.Graphics;


namespace FriceEngineTest
{
	public static class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			// ReSharper disable once ObjectCreationAsStatement
			//new Demo();
			new Test2();
		}
	}

	public class Test2 : WpfGame
	{
		private ImageObject _x = ImageObject.FromWeb(
			"http://codevs.cn/accounts/avatar/d5dcee637319a7623c61aaa6eae5c45d-80.png"
			, 0, 0, 100, 100);

		private TextObject _t = new TextObject(ColorResource.Black, "Click", 30, 300, 200);
		private ButtonObject _b;

		public override void OnInit()
		{
			Height = 500;
			Width = 1280;
			var t = new TextObject(ColorResource.Black, "Press any Key (keyboard or mouse)", 30, 400, 200);
			AddObject(t, _t);
			_b = new ButtonObject(null, "Button1", 100, 100, 50, 50,
				ColorResource.Black, ColorResource.Red,
				new ImageResource(_x.Bitmap), i =>
				{
					MessageBox.Show($"{i} clicked!");
					_b.Width *= 1.5;
					_b.Height *= 1.5;
				});
			var pause = new ButtonObject("Pause", "pause", 200, 200, 50, 25, ColorResource.Blue, ColorResource.Black,
				onClick: s => { GamePause(); });
			var start = new ButtonObject("Resume", "resume", 200, 250, 50, 25, ColorResource.Blue, ColorResource.Black,
				onClick: s => { GameStart(); });
			AddObject(_b, pause, start);
		}

		public override void OnClick(double x, double y, int b)
		{
			FObject a;
			switch (b)
			{
				case 0:
					a = _x.Clone();
					a.Collision += A_Collision;
					break;
				case 2:
					a = ImageObject.FromFile(@"C:\frice.png", 0, 0, 100, 100);
					break;
				default:
					a = new ShapeObject(ColorResource.Black, new FCircle(50.0), 0, 0);
					break;
			}
			a.SetCentre(x, y);
			a.AddAnims(new SimpleMove(30, -500));
			a.AddAnims(new AccelerateMove(0, 800));
			AddObject(a);
		}

		private void A_Collision(object sender, FriceEngine.Utils.Misc.OnCollosionEventArgs e)
		{
			var oc = new TextObject(ColorResource.Black, "", 50, 200, 20) {Text = e.CollideObject.Uid.ToString()};
			oc.AddAnims(new AccelerateMove(0,500));
			AddObject(oc);
		}

		public override void OnMouseMove(double x, double y)
		{
			_t.X = x + 30;
			_t.Y = y;
			_t.Text = $"位置： {x}, {y}";
		}

		public override void OnKeyDown(string key)
		{
			var t = new TextObject(ColorResource.赤羽业, "", 50, Random.Next(0, 1000), 20) {Text = key};
			t.AddAnims(new AccelerateMove(0, 300));
			AddObject(t);
			if (key.ToLower().Contains("x"))
			{
				var s = GetScreenCut();
				var o = new ImageObject(s, 200, 200);
				AddObject(o);
			}
		}

		public override void OnLoseFocus()
		{
			GamePause();
		}

		public override void OnFocus()
		{
			GameStart();
		}
	}
}
