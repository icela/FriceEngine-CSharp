using Frice_dotNet.Properties.FriceEngine;
using Frice_dotNet.Properties.FriceEngine.Animation;
using Frice_dotNet.Properties.FriceEngine.Object;
using Frice_dotNet.Properties.FriceEngine.Resource;
using Frice_dotNet.Properties.FriceEngine.Utils.Graphics;

namespace Frice_dotNet
{
    public class Test : Game
    {
        public override void OnInit()
        {
            var a = new ShapeObject(ColorResource.茅野枫, new FCircle(50), 100, 100);
            a.MoveList.Add(new SimpleMove(10, 0));
            base.OnInit();
            AddObject(a);
        }

        public static void Main(string[] args)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Test();
        }
    }
}