using System;
using System.Net.Mime;
using System.Threading;
using System.Windows.Forms;
using FriceEngine;
using FriceEngine.Animation;
using FriceEngine.Object;
using FriceEngine.Resource;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Time;

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