using System.Collections.Generic;
using System.Drawing;

namespace Frice_dotNet.Properties.FriceEngine.Utils.Graphics
{
	public static class ColorUtils
	{
		public static readonly IList<char> AsciiList = new List<char>
		{
			'#',
			'0',
			'X',
			'x',
			'+',
			'=',
			'-',
			';',
			',',
			'.',
			' '
		};

		public static char ToAscii(int rgb) => AsciiList[Gray(rgb) / (256 / AsciiList.Count + 1)];

		public static int Gray(int argb)
		{
			var color = Color.FromArgb(argb);
			var c = (color.R + color.G + color.B) / 3;
			return Color.FromArgb(color.A, c, c, c).ToArgb();
		}
	}
}
