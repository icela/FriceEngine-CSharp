using System;
using FriceEngine.Object;

namespace FriceEngine.Animation
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
        public SimpleMove(int y, int x)
        {
            Y = y;
            X = x;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override DoublePair GetDelta()
        {
            return DoublePair.From1000((Now - Start) * X, (Now - Start) * Y);
        }
    }

    public class AccurateMove : MoveAnim
    {
        public AccurateMove(int y, int x)
        {
            Y = y;
            X = x;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public override DoublePair GetDelta()
        {
            return DoublePair.From1000((Now - Start)*X, (Now - Start)*Y);
        }
    }
}