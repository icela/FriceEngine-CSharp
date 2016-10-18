using System;
using System.Timers;

namespace FriceEngine.Utils.Time
{
	/// <summary>
	/// timer using system API.
	/// reciving and avtion.
	/// </summary>
	/// <author>ifdog</author>
	public sealed class FTimer
	{
		public FTimer(long milliSeconds)
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

	public class FTimeListener
	{
		public FTimeListener(long milliSeconds, Action onTimeUp, bool autoReset = false)
		{
			_timer = new Timer
			{
				Interval = milliSeconds,
				AutoReset = autoReset
			};
			_timer.Elapsed += (sender, args) => { onTimeUp?.Invoke(); };
		}

		private readonly Timer _timer;

		public void Start()
		{
			if (!_timer.Enabled) _timer.Start();
		}

		public void Stop()
		{
			if (_timer.Enabled) _timer.Stop();
		}
	}
}
