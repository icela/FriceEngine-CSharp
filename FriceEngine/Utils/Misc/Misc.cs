using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using FriceEngine.Object;
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
		public TF First { get; set; }
		public TS Second { get; set; }

		public Pair(TF first, TS second)
		{
			Second = second;
			First = first;
		}
	}

	public static class StaticHelper
	{
		private static int _base;
		public static int GetNewUid() => _base++;

		public static Image ToImage(this Bitmap bmp)
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

		public static void ForEach<T>(this IEnumerable<T> ieEnumerable, Action<T> action)
		{
			foreach (var o in ieEnumerable)
			{
				action.Invoke(o);
			}
		}

		public static void ForEach(this int i, Action<int> action)
		{
			if (i >= 0)
			{
				for (int x = 0; x < i; x++)
				{
					action(i);
				}
			}
			else
			{
				for (int x = i; x < 0; x++)
				{
					action(i);
				}
			}
		}
	}

	public class Utils
	{
		public static void ForceRun(Action action)
		{
			try
			{
				action.Invoke();
			}
			catch (Exception)
			{
				// ignored
			}
		}

		public static void Async(Action action)
		{
			new Thread(action.Invoke).Start();
		}
	}

	public class OnCollosionEventArgs : EventArgs
	{
		public PhysicalObject ThisObject;
		public PhysicalObject ThatObject;

		public OnCollosionEventArgs(PhysicalObject thisObj, PhysicalObject thatObj)
		{
			ThisObject = thisObj;
			ThatObject = thatObj;
		}
	}
}
