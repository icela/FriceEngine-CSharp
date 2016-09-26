using System;
using FriceEngine.Object;

namespace FriceEngine.Animation
{
    public abstract class FAnim
    {
        protected long Start = DateTime.Now.Ticks;

        protected long Now = DateTime.Now.Ticks;
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

        public int X { get; }
        public int Y { get; }

        public override DoublePair GetDelta()
        {
            Now = DateTime.Now.Ticks;
            DoublePair d = DoublePair.FromTicks((Now - Start)*X, (Now - Start)*Y);
            Start = DateTime.Now.Ticks;
            ;
            return d;
        }
    }

    /// <summary>
    /// a move class with more accurate parameters.
    /// </summary>
    public class AccurateMove : MoveAnim
    {
        public AccurateMove(int y, int x)
        {
            Y = y;
            X = x;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public override DoublePair GetDelta() => DoublePair.From1000((Now - Start)*X, (Now - Start)*Y);
    }

//    public class AccelerateMove : MoveAnim
//    {
//        public AccelerateMove(double x, double y)
//        {
//            X = x;
//            Y = y;
//        }

//        public double X { get; set; }
//        public double Y { get; set; }

//        public override DoublePair GetDelta()
//        {
//            return DoublePair.From1000();
//        }
//    }
}