using System;
using System.Runtime.CompilerServices;
using System.Timers;

namespace FriceEngine.Utils.Time
{
    public class FTimer
    {
        public readonly int Time;

        public int Times { get; protected set; }

        public int Start = DateTime.Now.Millisecond;

        public FTimer(int time, int times)
        {
            Time = time;
            Times = times;
        }

        public FTimer(int time)
        {
            Time = time;
            Times = -1;
        }

        public bool Ended()
        {
            if (DateTime.Now.Millisecond - Start <= Time || Times == 0) return false;
            Start = DateTime.Now.Millisecond;
            if (Times > 0) Times--;
            return true;
        }
    }

    public class FTimeListener : FTimer
    {
        public FTimeListener(int time, int times) : base(time, times)
        {
        }

        public FTimeListener(int time) : base(time)
        {
        }

        public void Check()
        {
            if (!Ended() || Times == 0) return;
            if (Times > 0) --Times;
            // TODO invoke the event
        }
    }

    /// <summary>
    /// timer using system API.
    /// </summary>
    /// <author>ifdog</author>
    public class FTimer2
    {
        public FTimer2(int milliSeconds)
        {
            _timer = new Timer
            {
                Interval = milliSeconds,
                AutoReset = true
            };
        }

        private readonly Timer _timer;

        public void Start(Action action)
        {
            this._timer.Start();
            this._timer.Elapsed += (sender, args) => action.Invoke();
        }
    }

    public class FTimeListener2
    {
        public FTimeListener2(int milliSeconds,bool autoReset = false)
        {
            _timer = new Timer()
            {
                Interval = milliSeconds,
                AutoReset = autoReset,
            };
            _timer.Elapsed += (sender, args) =>{OnTimeUp?.Invoke();};
        }
        private readonly Timer _timer;
        public event Action OnTimeUp;
        public void Start() => this._timer.Start();

    }
}