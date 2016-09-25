using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriceEngine.Animation;
using FriceEngine.Resource;
using FriceEngine.Utils.Graphics;

namespace FriceEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            new Test();
        }
    }

    public class Test : FriceEngine.Game
    {
        
        public override void OnInit()
        {
            this.Width = 800;
            this.Height = 600;
            var a = new FriceEngine.Object.ShapeObject(FriceEngine.Resource.ColorResource.吾王蓝,new FCircle(40),400,300 );
            //replace with a file path in desk
            var b = ImageResource.FromFile(@"C:\1.bmp",400,300);
            var c = ImageResource.FromWeb("https://avatars1.githubusercontent.com/u/21008243", 400, 300);
            //can resize：
            a.Height = 100;
            a.Width = 100;
            c.Height = 100;
            c.Width = 100;
            a.MoveList.Add(new SimpleMove(10,10));
            b.MoveList.Add(new SimpleMove(10,-10));
            c.MoveList.Add(new SimpleMove(-10,10));
            AddObject(a);
            AddObject(b);
            AddObject(c);
     
        }

    }
}
