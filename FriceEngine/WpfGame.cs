using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FriceEngine.Object;
using FriceEngine.Resource;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Misc;
using FriceEngine.Utils.Time;

namespace FriceEngine
{
	/// <summary>
	/// A abstract Game Class based on WPF.
	/// ifdog同学加油喔，不是很懂WPF的API所以帮不上什么忙啦。。。只能根据Google和栈溢出加一些自己能加的东西。		——ice1000
	/// 对，我现在也在面向Stackoverflow编程。 -ifdog
	/// </summary>
	///<author>ifdog</author>
	public class WpfGame
	{
		private readonly WpfWindow _window;
		private readonly List<IAbstractObject> _buffer = new List<IAbstractObject>();
		public bool ShowFps { get; set; } = true;
		public double Width { get; set; } = 1024;
		public double Height { get; set; } = 768;
		public bool LoseFocusChangeColor = false;

		protected WpfGame()
		{
			_init();
			_window = new WpfWindow(Width, Height, ShowFps)
			{
				CustomDrawAction = CustomDraw
			};
			_window.Closing += (s, e) => { OnExit(); };
			_window.MouseDown += (s, e) =>
			{
				var p = e.GetPosition(_window.Canvas);
				OnClick(p.X, p.Y, (int) e.ChangedButton);
			};
			_window.MouseMove += (s, e) =>
			{
				var p = e.GetPosition(_window.Canvas);
				OnMouseMove(p.X, p.Y);
			};
			_window.KeyDown += (s, e) => { OnKeyDown(e.Key.ToString()); };
			_window.DragOver += (s, e) =>
			{
				var p = e.GetPosition(_window.Canvas);
				OnDragOver(p.X, p.Y);
			};
			_window.Drop += (s, e) =>
			{
				var p = e.GetPosition(_window.Canvas);
				OnDrop(p.X, p.Y);
			};
			_window.GotFocus += (s, e) => OnFocus();
			_window.LostFocus += (s, e) => OnLoseFocus();
			CompositionTarget.Rendering += (sender, e) =>
			{
				OnRefresh();
				_window.Update(_buffer);
			};
			new Application().Run(_window);
		}

		private void _init() => OnInit();

		public virtual void OnInit()
		{
		}

		public virtual void OnRefresh()
		{
		}

		public virtual void OnExit()
		{
		}

		public virtual void CustomDraw(Canvas canvas)
		{
		}

		public virtual void OnClick(double x, double y, int button)
		{
		}

		public virtual void OnMouseMove(double x, double y)
		{
		}

		public virtual void OnKeyDown(string key)
		{
		}

		public virtual void OnFocus()
		{
		}

		public virtual void OnLoseFocus()
		{
		}

		public virtual void OnDragOver(double x, double y)
		{
		}

		public virtual void OnDrop(double x, double y)
		{
		}

		public void AddObjects(params IAbstractObject[] obj) => obj.ToList().ForEach(_buffer.Add);
		public void RemoveObjects(params IAbstractObject[] obj) => obj.ToList().ForEach(i => { _buffer.Remove(i); });

		public void SetBack(ImageResource img)
		{
		}

		public void SetCursor(ImageResource img)
		{
		}

		public ImageResource GetScreenCut() => null;

		public void EndGameWithADialog(string title, string content)
		{
			if (MessageBox.Show(content, title, MessageBoxButton.OK) == MessageBoxResult.OK)
			{
				_window.Close();
			}
		}
	}

	public class WpfWindow : Window
	{
		public readonly Canvas Canvas = new Canvas();
		public bool _showFps;
		public Action<Canvas> CustomDrawAction;
		private readonly Dictionary<int, FrameworkElement> _objectsDict = new Dictionary<int, FrameworkElement>();
		private readonly TextBlock _fpsTextBlock;
		private int _fps;
		private readonly List<IAbstractObject> _removing = new List<IAbstractObject>();

		public WpfWindow(double width = 1024.0, double height = 768.0, bool showFps = true)
		{
			_showFps = showFps;
			Content = Canvas;
			Width = width;
			Height = height;
			Canvas.Width = width;
			Canvas.Height = height;
			SizeChanged += (sender, args) =>
			{
				Canvas.Height = args.NewSize.Height;
				Canvas.Width = args.NewSize.Width;
			};
			if (_showFps)
			{
				_fpsTextBlock = new TextBlock
				{
					Foreground = Brushes.Red,
					Background = Brushes.White
				};
				_fpsTextBlock.SetValue(Canvas.LeftProperty, Canvas.Width - 65.0);
				_fpsTextBlock.SetValue(Canvas.TopProperty, Canvas.Height - 60.0);
				Canvas.Children.Add(_fpsTextBlock);
				new FTimer2(1000).Start(() =>
				{
					Dispatcher.Invoke(() => { _fpsTextBlock.Text = $"FPS:{_fps}"; });
					_fps = 0;
				});
			}
		}

		public void Update(List<IAbstractObject> objects)
		{
			_objectsDict.Keys.Where(o =>
						!objects.Select(i => i.Uid).Contains(o)
			).ToList().ForEach(_onRemove);
			objects.ForEach(o =>
			{
				if (0 - Canvas.Width < o.X && o.X < Canvas.Width && 0 - Canvas.Height < o.Y && o.Y < Canvas.Height)
				{
					if (_objectsDict.ContainsKey(o.Uid)) _onChange(o);
					else _onAdd(o);
				}
				else if (-10 * Canvas.Width > o.X || o.X > 10 * Canvas.Width || -10 * Canvas.Height > o.Y || o.Y > 10 * Canvas.Height)
				{
					_removing.Add(o);
				}
				else
				{
					if (_objectsDict.Keys.Contains(o.Uid))
					{
						Canvas.Children.Remove(_objectsDict[o.Uid]);
						_objectsDict.Remove(o.Uid);
					}
					(o as FObject)?.RunAnims();
				}
			});
			_removing.ForEach(o =>
			{
				if (_objectsDict.ContainsKey(o.Uid)) _objectsDict.Remove(o.Uid);
				objects.Remove(o);
			});
			_removing.Clear();
			CustomDrawAction?.Invoke(Canvas);
			if (_showFps) _fps++;
		}

		private void _onRemove(int uid)
		{
			_objectsDict.Remove(uid);
			Canvas.Children.Remove(_objectsDict[uid]);
		}

		private void _onAdd(IAbstractObject obj)
		{
			FrameworkElement element = null;
			if (obj is ShapeObject)
			{
				var c = ((ShapeObject) obj).ColorResource.Color;
				if (((ShapeObject) obj).Shape is FRectangle)
				{
					element = new Rectangle
					{
						Fill = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B)),
						Width = (float) ((ShapeObject) obj).Width,
						Height = (float) ((ShapeObject) obj).Height
					};
				}
				else if (((ShapeObject) obj).Shape is FOval)
				{
					element = new Ellipse
					{
						Fill = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B)),
						Width = (float) ((ShapeObject) obj).Width,
						Height = (float) ((ShapeObject) obj).Height
					};
				}
			}
			else if (obj is ImageObject)
			{
				element = StaticHelper.BitmapToImage(((ImageObject) obj).Bitmap);
			}
			else if (obj is TextObject)
			{
				var o = (TextObject) obj;
				element = new TextBlock
				{
					Foreground = new SolidColorBrush(ColorUtils.ToMediaColor(o.ColorResource.Color)),
					Text = o.Text
				};
			}
			if (element != null)
			{
				element.SetValue(Canvas.LeftProperty, obj.X);
				element.SetValue(Canvas.TopProperty, obj.Y);
				_objectsDict.Add(obj.Uid, element);
				Canvas.Children.Add(element);
			}
		}

		private void _onChange(IAbstractObject o)
		{
			var element = _objectsDict[o.Uid];
			if (o is TextObject)
			{
				((TextBlock) element).Text = ((TextObject) o).Text;
				((TextBlock) element).FontSize = ((TextObject) o).Size;
			}
			(o as FObject)?.RunAnims();
			element.SetValue(Canvas.LeftProperty, o.X);
			element.SetValue(Canvas.TopProperty, o.Y);
		}


		private static Image BitmapToImage(System.Drawing.Image bmp)
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
	}
}
