using System;
using FriceEngine.Utils.Misc;

namespace FriceEngine.Animation
{
	/// <summary>
	/// base class of Animations
	/// </summary>
	public abstract class FAnim
	{
		public int Uid { get; } = StaticHelper.GetNewUid();

		protected readonly long Start = DateTime.Now.Ticks;

		protected long Last = DateTime.Now.Ticks;

		protected long Now = DateTime.Now.Ticks;
	}
}