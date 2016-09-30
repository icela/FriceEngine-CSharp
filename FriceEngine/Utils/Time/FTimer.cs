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
		public readonly long Time;

		// ReSharper disable once MemberCanBeProtected.Global
		public long Times { get; protected set; }

		// ReSharper disable once MemberCanBePrivate.Global
		public long Start = DateTime.Now.Millisecond;

		// ReSharper disable once MemberCanBeProtected.Global
		public FTimer(long time, long times)
		{
			Time = time;
			Times = times;
		}

		// ReSharper disable once MemberCanBeProtected.Global
		public FTimer(long time)
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
		public Action OnTimeEnded;

		public FTimeListener(long time, long times, Action action) : base(time, times)
		{
			OnTimeEnded = action;
		}

		public FTimeListener(long time, Action action) : base(time)
		{
			OnTimeEnded = action;
		}

		public void Check()
		{
			if (!Ended() || Times == 0) return;
			if (Times > 0) --Times;
			OnTimeEnded.Invoke();
		}
	}

	/// <summary>
	/// timer using system API.
	/// reciving and avtion.
	/// </summary>
	/// <author>ifdog</author>
	public sealed class FTimer2
	{
		public FTimer2(long milliSeconds)
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
		public FTimeListener2(long milliSeconds, bool autoReset = false)
		{
			_timer = new Timer
			{
				Interval = milliSeconds,
				AutoReset = autoReset
			};
			_timer.Elapsed += (sender, args) => { OnTimeUp?.Invoke(); };
		}

		private readonly Timer _timer;
		public event Action OnTimeUp;
		public void Start() => _timer.Start();
	}
}
