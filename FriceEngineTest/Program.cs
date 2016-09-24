using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriceEngine.Animation;
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
            var a = new FriceEngine.Object.ShapeObject(FriceEngine.Resource.ColorResource.吾王蓝,new FCircle(40),100,100 );
            a.MoveList.Add(new SimpleMove(5,2));
            AddObject(a);
     
        }

    }
}
