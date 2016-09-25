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
            var a = new FriceEngine.Object.ShapeObject(FriceEngine.Resource.ColorResource.吾王蓝,new FCircle(40),100,100 );
            var b = ImageResource.FromFile(@"C:\1.bmp",250,100);
            var c = ImageResource.FromWeb("https://avatars1.githubusercontent.com/u/21008243", 100, 100);
            //可缩放：
            a.Height = 50;
            a.Width = 60;
            c.Height = 50;
            c.Width = 50;
            a.MoveList.Add(new SimpleMove(-5,-2));
            b.MoveList.Add(new SimpleMove(-10,4));
            c.MoveList.Add(new SimpleMove(-10, -4));
            AddObject(a);
            AddObject(b);
            AddObject(c);
     
        }

    }
}
