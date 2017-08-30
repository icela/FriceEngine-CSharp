using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FriceEngine.Object;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Misc;
using FriceEngine.Utils.Time;
using Brushes = System.Windows.Media.Brushes;
using Rectangle = System.Windows.Shapes.Rectangle;

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
		private readonly List<FTimeListener> _fTimeListeners = new List<FTimeListener>();

		public bool ShowFps
		{
			get => _window.ShowFps;
			set => _window.ShowFps = value;
		}

		public double Width { get; set; } = 1024;
		public double Height { get; set; } = 768;
		public bool LoseFocusChangeColor = false;
		public bool GameStarted { get; }

		public readonly Random Random;
		internal QuadTree Tree;
		internal IEnumerable<PhysicalObject> ExistingPhysicalObjects;

		protected WpfGame()
		{
			Random = new Random(DateTime.Now.Millisecond);
			_init();
			Tree = new QuadTree(new System.Drawing.Rectangle(0, 0, (int) Width, (int) Height));
			_window = new WpfWindow(Width, Height)
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
			_window.Activated += (s, e) => OnFocus();
			_window.Deactivated += (s, e) => OnLoseFocus();
			CompositionTarget.Rendering += (s, e) =>
			{
				OnRefresh();
				_window.Update(_buffer);
				CollisionDetection();
			};
			new Application().Run(_window);
		}

		private void _init() => OnInit();

		/// <summary>
		/// a collision detection system using quadThree.
		/// </summary>
		internal void CollisionDetection()
		{
			ExistingPhysicalObjects =
				_buffer.Where(i => _window.ObjectsDict.ContainsKey(i.Uid)).OfType<PhysicalObject>().ToArray();
			Tree.Clear();
			Tree.Insert(ExistingPhysicalObjects);
			var detect = new List<PhysicalObject>();
			ExistingPhysicalObjects.ForEach(i =>
			{
				Tree.Retrieve(detect, i);
				detect.ForEach(o =>
				{
					if (i.Uid != o.Uid &&
					    i.X + i.Width >= o.X && o.Y <= i.Y + i.Height && i.X <= o.X + o.Width && i.Y <= o.Y + o.Height)
						i.OnCollision(new OnCollosionEventArgs(i, o));
				});
			});
		}

		public void GameStart()
		{
			Clock.Resume();
			_fTimeListeners.ForEach(i => i.Start());
		}

		public void GamePause()
		{
			Clock.Pause();
			_fTimeListeners.ForEach(i => i.Stop());
		}

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

		public void AddObject(params IAbstractObject[] obj) => obj.ForEach(_buffer.Add);
		public void RemoveObject(params IAbstractObject[] obj) => obj.ForEach(i => _buffer.Remove(i));
		public void AddTimeListener(params FTimeListener[] obj) => obj.ForEach(_fTimeListeners.Add);
		public void RemoveTimeListener(params FTimeListener[] obj) => obj.ForEach(i => _fTimeListeners.Remove(i));

//		public void SetCursor(ImageResource img)
//		{
//		TODO
//		}

		public void SetSize(int width, int height)
		{
			Width = width;
			Height = height;
		}

//		public void SetLocation(int x, int y)
//		{
//		TODO
//		}

		public Bitmap GetScreenCut()
		{
			var bmp = new RenderTargetBitmap((int) _window.Canvas.Width,
				(int) _window.Canvas.Height, 96, 96, PixelFormats.Default);
			bmp.Render(_window.Canvas);
			var encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(bmp));
			using (var ms = new MemoryStream())
			{
				encoder.Save(ms);
				return new Bitmap(ms);
			}
		}

		public void EndGameWithDialog(string title, string content)
		{
			if (MessageBox.Show(content, title, MessageBoxButton.OK) == MessageBoxResult.OK)
				_window.Close();
		}
	}

	/// <summary>
	/// base window class
	/// </summary>
	public class WpfWindow : Window
	{
		public Action<Canvas> CustomDrawAction;
		public Dictionary<int, FrameworkElement> ObjectsDict { get; } = new Dictionary<int, FrameworkElement>();

		internal bool ShowFps = true;
		internal readonly Canvas Canvas;

		private int _fps;
		private readonly List<IAbstractObject> _removing = new List<IAbstractObject>();

		public WpfWindow(double width = 1024.0, double height = 768.0)
		{
			Canvas = new Canvas();
			Content = Canvas;
			Width = width;
			Height = height;
			Canvas.Width = width;
			Canvas.Height = height;
			ResizeMode = ResizeMode.CanMinimize;
			SizeChanged += (sender, args) =>
			{
				Canvas.Height = args.NewSize.Height;
				Canvas.Width = args.NewSize.Width;
			};
			var fpsTextBlock = new TextBlock
			{
				Foreground = Brushes.Blue,
				Background = Brushes.White
			};
			fpsTextBlock.SetValue(Canvas.LeftProperty, Canvas.Width - 65.0);
			fpsTextBlock.SetValue(Canvas.TopProperty, Canvas.Height - 60.0);
			new FTimer(1000).Start(() =>
			{
				Dispatcher.Invoke(() => { fpsTextBlock.Text = $"fps:{_fps}"; });
				_fps = 0;
			});
			if (ShowFps)
				Canvas.Children.Add(fpsTextBlock);
		}

		public void Update(List<IAbstractObject> objects)
		{
			ObjectsDict.Keys.Where(o =>
				!objects.Select(i => i.Uid).Contains(o)
			).ToList().ForEach(_onRemove);
			objects.ForEach(o =>
			{
				if (0 - Canvas.Width < o.X &&
				    o.X < Canvas.Width &&
				    0 - Canvas.Height < o.Y &&
				    o.Y < Canvas.Height)
				{
					if (ObjectsDict.ContainsKey(o.Uid)) _onChange(o);
					else _onAdd(o);
				}
				else if (-10 * Canvas.Width > o.X ||
				         o.X > 10 * Canvas.Width ||
				         -10 * Canvas.Height > o.Y ||
				         o.Y > 10 * Canvas.Height)
					_removing.Add(o);
				else
				{
					if (ObjectsDict.Keys.Contains(o.Uid))
					{
						Canvas.Children.Remove(ObjectsDict[o.Uid]);
						ObjectsDict.Remove(o.Uid);
					}
				}
				if (o is FObject f)
				{
					f.RunAnims();
					f.CheckCollitions();
					if (f.Died) _removing.Add(o);
				}
			});
			_removing.ForEach(o =>
			{
				if (ObjectsDict.ContainsKey(o.Uid)) ObjectsDict.Remove(o.Uid);
				objects.Remove(o);
			});
			_removing.Clear();
			CustomDrawAction?.Invoke(Canvas);
			if (ShowFps) _fps++;
		}

		private void _onRemove(int uid)
		{
			ObjectsDict.Remove(uid);
			Canvas.Children.Remove(ObjectsDict[uid]);
		}

		private void _onAdd(IAbstractObject obj)
		{
			FrameworkElement element = null;
			if (obj is ShapeObject shape)
			{
				var brush = new SolidColorBrush(((ShapeObject) obj).ColorResource.Color.ToMediaColor());
				if (shape.Shape is FRectangle)
				{
					element = new Rectangle
					{
						Fill = brush,
						Width = (float) shape.Width,
						Height = (float) shape.Height
					};
				}
				else if (shape.Shape is FOval)
				{
					element = new Ellipse
					{
						Fill = brush,
						Width = (float) shape.Width,
						Height = (float) shape.Height
					};
				}
			}
			else if (obj is ImageObject image)
			{
				element = image.Bitmap.ToImage();
			}
			else if (obj is TextObject text)
			{
				element = new TextBlock
				{
					Foreground = new SolidColorBrush(text.ColorResource.Color.ToMediaColor()),
					Text = text.Text
				};
			}
			else if (obj is ButtonObject button)
			{
				element = new Button
				{
					Background = new SolidColorBrush(button.BackgroundColor.Color.ToMediaColor()),
					Foreground = new SolidColorBrush(button.ForegroundColor.Color.ToMediaColor()),
					Height = button.Height,
					Width = button.Width
				};
				var buttonElement = (Button) element;
				if (button.Image == null) buttonElement.Content = button.Text;
				else buttonElement.Content = button.Image.Bitmap.ToImage();

				if (button.OnClick != null) buttonElement.Click += (s, e) => button.OnClick(button.Name);
				if (button.OnMouseEnter != null) buttonElement.Click += (s, e) => button.OnMouseEnter(button.Name);
				if (button.OnMouseLeave != null) buttonElement.Click += (s, e) => button.OnMouseLeave(button.Name);
			}
			if (element != null)
			{
				element.SetValue(Canvas.LeftProperty, obj.X);
				element.SetValue(Canvas.TopProperty, obj.Y);
				ObjectsDict.Add(obj.Uid, element);
				Canvas.Children.Add(element);
			}
		}

		private void _onChange(IAbstractObject o)
		{
			var element = ObjectsDict[o.Uid];
			if (o is TextObject text)
			{
				var textblock = ((TextBlock) element);
				textblock.Text = text.Text;
				textblock.FontSize = text.Size;
			}
			if (o is ButtonObject button)
			{
				var oldButton = (Button) element;
				if (button.Image == null) oldButton.Content = button.Text;
				else oldButton.Content = button.Image.Bitmap.ToImage();
				oldButton.Width = button.Width;
				oldButton.Height = button.Height;
				oldButton.Background = new SolidColorBrush(button.BackgroundColor.ToMediaColor());
				oldButton.Foreground = new SolidColorBrush(button.ForegroundColor.ToMediaColor());
			}
			if (o is FObject f) f.RunAnims();
			element.SetValue(Canvas.LeftProperty, o.X);
			element.SetValue(Canvas.TopProperty, o.Y);
		}
	}
}