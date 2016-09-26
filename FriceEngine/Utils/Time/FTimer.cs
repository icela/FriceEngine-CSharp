using System;
using System.Timers;

namespace FriceEngine.Utils.Time
{
	/// <summary>
	/// inherited from JVM edition.
	/// </summary>
	public class FTimer
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public readonly int Time;

		// ReSharper disable once MemberCanBeProtected.Global
		public int Times { get; protected set; }

		// ReSharper disable once MemberCanBePrivate.Global
		public int Start = DateTime.Now.Millisecond;

		// ReSharper disable once MemberCanBeProtected.Global
		public FTimer(int time, int times)
		{
			Time = time;
			Times = times;
		}

		// ReSharper disable once MemberCanBeProtected.Global
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

	/// <summary>
	/// inherited from JVM edition.
	/// </summary>
	public sealed class FTimeListener : FTimer
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
	/// reciving and avtion.
	/// </summary>
	/// <author>ifdog</author>
	public sealed class FTimer2
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
			_timer.Start();
			_timer.Elapsed += (sender, args) => action.Invoke();
		}
	}

	public class FTimeListener2
	{
		public FTimeListener2(int milliSeconds, bool autoReset = false)
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