using System;
using System.Runtime.CompilerServices;
using System.Timers;

namespace FriceEngine.Utils.Time
{
    public class FTimer2
    {
        public FTimer2(int time)
        {
            this._timer = new Timer()
            {
                Interval = time,
                AutoReset = true,
            };
        }
        private readonly Timer _timer;

        public void Start(Action action)
        {
            this._timer.Start();
            this._timer.Elapsed += (sender, args) => action.Invoke();
        }
    }
}