using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using FriceEngine.Object;
using FriceEngine.Utils.Graphics;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace FriceEngine
{
	public class CanvasPainter : Canvas
	{
		private FBuffers _buffers;

		public CanvasPainter(FBuffers buffers)
		{
			_buffers = buffers;
		}

		protected override void OnRender(DrawingContext dc)
		{
			_buffers.ObjectAddBuffer.ToList().ForEach(o =>
			{
				if (o is ShapeObject)
				{
					var x = ((ShapeObject) o).ColorResource.Color;
					Brush brush = new SolidColorBrush(Color.FromArgb(x.A, x.R, x.G, x.B));

					if (((ShapeObject) o).Shape is FRectangle)
					{
						dc.DrawRectangle(brush, null,
							new Rect((float) o.X,
								(float) o.Y,
								(float) ((ShapeObject) o).Width,
								(float) ((ShapeObject) o).Height));
					}
					else if (((ShapeObject) o).Shape is FOval)
						dc.DrawEllipse(brush, null,
							new Point(o.X, o.Y),
							(float) ((ShapeObject) o).Width,
							(float) ((ShapeObject) o).Height);
				}
				else if (o is ImageObject)
				{
				}
			});
			base.OnRender(dc);
		}
	}

	/// <summary>
	/// engine class working on WPF.
	/// </summary>
	/// <author>ifdog</author>
	public abstract partial class GameWindow : Form
	{
		private Canvas _canvas = new Canvas();

		public GameWindow(FBuffers buffers)
		{
			InitializeComponent();
			elementHost1.Child = _canvas;
		}
	}
	
	public class FBuffers
	{
		public BlockingCollection<IAbstractObject> ObjectAddBuffer = new BlockingCollection<IAbstractObject>();
	}
}