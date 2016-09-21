using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frice_dotNet.Properties.FriceEngine.Utils.Time
{
    public class FTimer
    {
        protected int Time;

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

    public class FTimeListener:FTimer
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
}