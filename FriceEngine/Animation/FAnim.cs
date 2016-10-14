using System;
using FriceEngine.Utils.Misc;
using FriceEngine.Utils.Time;

namespace FriceEngine.Animation
{
	/// <summary>
	/// base class of Animations
	/// </summary>
	public abstract class FAnim
	{
		public int Uid { get; } = StaticHelper.GetNewUid();

		protected readonly long Start = Clock.Current;

		protected long Last = Clock.Current;

		protected long Now = Clock.Current;
	}
}