using System;
using Frice_dotNet.Properties.FriceEngine.Object;

namespace Frice_dotNet.Properties.FriceEngine.Animation
{
    public abstract class FAnim
    {
        protected readonly double Start = DateTime.Now.Millisecond;

        protected double Now => DateTime.Now.Millisecond;
    }

    public abstract class MoveAnim : FAnim
    {
        public abstract DoublePair GetDelta();
    }

    public class SimpleMove : MoveAnim
    {
        public int X { get; set; }
        public int Y { get; set; }

        public SimpleMove(int y, int x)
        {
            Y = y;
            X = x;
        }

        public override DoublePair GetDelta()
        {
            return DoublePair.From1000((Now - Start)*X, (Now - Start)*Y);
        }
    }

    public class AccurateMove : MoveAnim
    {
        public double X { get; set; }
        public double Y { get; set; }

        public AccurateMove(int y, int x)
        {
            Y = y;
            X = x;
        }

        public override DoublePair GetDelta()
        {
            return DoublePair.From1000((Now - Start)*X, (Now - Start)*Y);
        }
    }
}