using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FriceEngine.Object;
using FriceEngine.Utils.Graphics;
using Image = System.Drawing.Image;

namespace FriceEngine
{
	/// <summary>
	/// A abstract Game Class based on WPF.
	/// WPF 太难写了 T_T.
	///
	/// ifdog同学加油喔		——ice1000
	/// </summary>
	///<author>ifdog</author>
	public class WpfGame
	{
		private readonly Window _gameWindow = new Window
		{
			ResizeMode = ResizeMode.NoResize
		};

		private readonly DrawingCanvas _canvas = new DrawingCanvas();
		private readonly List<IAbstractObject> _addBuffer = new List<IAbstractObject>();
		private readonly List<DrawingVisual> _drawBuffer = new List<DrawingVisual>();

		public double Width
		{
			get { return _gameWindow.Width; }
			set { _gameWindow.Width = value; }
		}

		public double Height
		{
			get { return _gameWindow.Height; }
			set { _gameWindow.Height = value; }
		}

		public Point Location
		{
			get { return new Point(_gameWindow.Left, _gameWindow.Top); }
			set
			{
				_gameWindow.Left = value.X;
				_gameWindow.Top = value.Y;
			}
		}

		public WpfGame()
		{
			_gameWindow.Content = _canvas;
		}

		public void AddVisuals()
		{
			_addBuffer.ForEach(o =>
			{
				var dv = new DrawingVisual();
				var dc = dv.RenderOpen();
				if (o is ShapeObject)
				{
					var c = ((ShapeObject) o).ColorResource.Color;
					var brush = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
					if (((ShapeObject) o).Shape is FRectangle)
					{
						dc.DrawRectangle(brush, null, new Rect(
							(float) o.X,
							(float) o.Y,
							(float) ((ShapeObject) o).Width,
							(float) ((ShapeObject) o).Height));
					}
					else if (((ShapeObject) o).Shape is FOval)
					{
						dc.DrawEllipse(brush, null, new Point(
								(float) o.X,
								(float) o.Y),
							(float) ((ShapeObject) o).Width,
							(float) ((ShapeObject) o).Height);
					}
				}
				else if (o is ImageObject)
				{
					var bmp = o as ImageObject;
					dc.DrawImage(BitmapToBitmapImage(bmp.Bitmap),
						new Rect(bmp.X, bmp.Y, bmp.Width, bmp.Height));
				}
				_drawBuffer.Add(dv);
			});
		}

		private static BitmapImage BitmapToBitmapImage(Image bitmap)
		{
			var bitmapImage = new BitmapImage();
			using (var ms = new System.IO.MemoryStream())
			{
				bitmap.Save(ms, bitmap.RawFormat);
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = ms;
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
				bitmapImage.Freeze();
			}
			return bitmapImage;
		}


		public void Run()
		{
			new Application().Run(_gameWindow);
		}
	}

	public class DrawingCanvas : Canvas
	{
		public List<Visual> Visuals = new List<Visual>();

		public DrawingVisual GetHitVisual(Point point) => VisualTreeHelper.HitTest(this, point).VisualHit as DrawingVisual;
	}
}
