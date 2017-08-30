using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using FriceEngine.Object;
using JetBrains.Annotations;
using Image = System.Windows.Controls.Image;

namespace FriceEngine.Utils.Misc
{
	/// <summary>
	/// TF 是 Type first,别想成某青少年明星组合了
	/// 我不是四叶草		——ice1000
	/// </summary>
	/// <typeparam name="TF">第一个参数的类型</typeparam>
	/// <typeparam name="TS">第二个参数的类型</typeparam>
	public class Pair<TF, TS>
	{
		[NotNull]
		public TF First { get; set; }

		[NotNull]
		public TS Second { get; set; }

		public Pair([NotNull] TF first, [NotNull] TS second)
		{
			Second = second;
			First = first;
		}
	}

	public static class StaticHelper
	{
		private static int _base;
		public static int GetNewUid() => _base++;

		[NotNull]
		public static Image ToImage([NotNull] this Bitmap bmp)
		{
			var bImage = new BitmapImage();
			using (var ms = new MemoryStream())
			{
				bmp.Save(ms, ImageFormat.Png);
				bImage.BeginInit();
				bImage.StreamSource = new MemoryStream(ms.ToArray());
				bImage.EndInit();
			}
			return new Image {Source = bImage};
		}

		public static void ForEach<T>(
			[NotNull] this IEnumerable<T> ieEnumerable,
			[NotNull] Action<T> action)
		{
			foreach (var o in ieEnumerable)
				action.Invoke(o);
		}

		public static void ForEach(this int i, [NotNull] Action<int> action)
		{
			if (i >= 0)
				for (var x = 0; x < i; x++)
					action(i);
			else
				for (var x = i; x < 0; x++)
					action(i);
		}
	}

	public class Utils
	{
		public static void ForceRun([NotNull] Action action) => action();

		public static void Async([NotNull] Action action) => new Thread(action.Invoke).Start();
	}

	public class OnCollosionEventArgs : EventArgs
	{
		[NotNull] public PhysicalObject ThisObject;
		[NotNull] public PhysicalObject ThatObject;

		public OnCollosionEventArgs(
			[NotNull] PhysicalObject thisObj,
			[NotNull] PhysicalObject thatObj)
		{
			ThisObject = thisObj;
			ThatObject = thatObj;
		}
	}
}