using System;
using FriceEngine.Object;

namespace FriceEngine.Animation
{
	/// <summary>
	/// base class of Animations
	/// </summary>
	public abstract class FAnim
	{
		protected readonly long Start = DateTime.Now.Ticks;

		protected long Last = DateTime.Now.Ticks;

		protected long Now = DateTime.Now.Ticks;
	}
}