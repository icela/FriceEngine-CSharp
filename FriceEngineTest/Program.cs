using System;
using System.Net.Mime;
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

			var a = new ShapeObject(ColorResource.吾王蓝, new FCircle(40), 400, 300);
			//replace with a file path in desk
			var b = ImageObject.FromFile(@"C:\frice.png", 300, 400, 50, 50);
//            var c = ImageObject.FromWeb("https://avatars1.githubusercontent.com/u/21008243", 400, 300);

			//can resize：
			a.Height = 100;
			a.Width = 100;
//            c.Height = 100;
//            c.Width = 100;
			a.MoveList.Add(new SimpleMove(10, 10));
			b.MoveList.Add(new SimpleMove(10, -10));
//            c.MoveList.Add(new SimpleMove(-10, 10));
			AddObject(a);
			AddObject(b);
//            AddObject(c);
		}
	}
}