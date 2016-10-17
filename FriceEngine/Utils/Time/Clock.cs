using System;

namespace FriceEngine.Utils.Time
{
	public static class Clock
	{
		public static bool IsStarted { get; internal set; }
		public static long StartTicks { get; internal set; }
		public static long PauseTicks { get; internal set; }
		public static long Current => IsStarted ? DateTime.Now.Ticks - StartTicks : PauseTicks - StartTicks;

		public static void Init()
		{
			StartTicks = DateTime.Now.Ticks;
			IsStarted = true;
		}

		public static void Resume()
		{
			if (IsStarted) return;
			StartTicks += DateTime.Now.Ticks - PauseTicks;
			IsStarted = true;
		}

		public static void Pause()
		{
			if (!IsStarted) return;
			PauseTicks = DateTime.Now.Ticks;
			IsStarted = false;
		}
	}
}
