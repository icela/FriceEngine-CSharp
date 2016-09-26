using System;
using FriceEngine.Object;

namespace FriceEngine.Animation
{
    public abstract class FAnim
    {
        protected long Start = DateTime.Now.Ticks;

        protected long Now = DateTime.Now.Ticks;
    }
}