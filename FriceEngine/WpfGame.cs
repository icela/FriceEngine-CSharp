using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using FriceEngine.Object;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Time;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace FriceEngine
{
	/// <summary>
	/// A abstract Game Class based on WPF.
	/// WPF 太难写了 T_T.
	///
	/// ifdog同学加油喔		——ice1000
	/// </summary>
	///<author>ifdog</author>
	public abstract class WpfGame
	{
		private WpfWindow _window = new WpfWindow();
		private List<IAbstractObject> _buffer = new List<IAbstractObject>();
		protected WpfGame()
		{
			OnInit();
			Run();
			new Application().Run(_window);
		}

		public void Run()
		{
			CompositionTarget.Rendering += (sender, e) => { _window.Update(_buffer); };
		}

		public abstract void OnInit();
		

		public void AddObject(IAbstractObject obj)
		{
			_buffer.Add(obj);
		}
	}

	public class WpfWindow : Window
	{
		private readonly Canvas _canvas = new Canvas();
		private readonly Dictionary<int,FrameworkElement> _objectsDict = new Dictionary<int, FrameworkElement>();
		public WpfWindow()
		{
			this.Content = _canvas;
		}
		public void Update(List<IAbstractObject> objects)
		{
			_objectsDict.Keys.Where(o => !objects.Select(i => i.Uid).Contains(o)).ToList().ForEach(_onRemove);
			objects.ForEach(o =>
			{
				if (_objectsDict.ContainsKey(o.Uid))
				{
					_onChange(o);
				}
				else
				{
					_onAdd(o);
				}
			});
		}

		private void _onRemove(int uid)
		{
			_objectsDict.Remove(uid);
			_canvas.Children.Remove(_objectsDict[uid]);
		}

		private void _onAdd(IAbstractObject obj)
		{
			if (obj is ShapeObject)
			{
				var c = ((ShapeObject) obj).ColorResource.Color;
				var brush = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
				if (((ShapeObject) obj).Shape is FRectangle)
				{
					Rectangle rect = new Rectangle()
					{
						Fill = brush,
						Width = (float) ((ShapeObject) obj).Width,
						Height = (float) ((ShapeObject) obj).Height,
					};
					rect.SetValue(Canvas.TopProperty,  obj.X);
					rect.SetValue(Canvas.LeftProperty,  obj.Y);
					_objectsDict.Add(obj.Uid, rect);
					_canvas.Children.Add(rect);
				}
				else if (((ShapeObject) obj).Shape is FOval)
				{
					Ellipse epse = new Ellipse()
					{
						Fill = brush,
						Width = (float) ((ShapeObject) obj).Width,
						Height = (float) ((ShapeObject) obj).Height,
					};
					epse.SetValue(Canvas.TopProperty,  obj.X);
					epse.SetValue(Canvas.LeftProperty,  obj.Y);
					_objectsDict.Add(obj.Uid, epse);
					_canvas.Children.Add(epse);
				}
			}
			else if (obj is ImageObject)
			{
				var bmp = obj as ImageObject;

				using (MemoryStream ms = new MemoryStream())
				{
					bmp.Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					BitmapImage bImage = new BitmapImage();
					bImage.BeginInit();
					bImage.StreamSource = new MemoryStream(ms.ToArray());
					bImage.EndInit();
					bmp.Bitmap.Dispose();
					Image img = new Image {Source = bImage};
					int x = obj.Uid;
					_objectsDict.Add(obj.Uid, img);
					_canvas.Children.Add(img);
					
				}
			}
		}

		private void _onChange(IAbstractObject o)
		{
			var element = _objectsDict[o.Uid];
			(o as FObject)?.RunAnims();
			//Canvas.SetLeft(element,o.X);
			//Canvas.SetTop(element,o.Y);
			element.SetValue(Canvas.TopProperty, o.X);
			element.SetValue(Canvas.LeftProperty, o.Y);
			
		}
		
	}
}
