using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using FriceEngine.Object;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Message;
using FriceEngine.Utils.Time;

namespace FriceEngine
{
	public class Game : Form
	{
		// ReSharper disable once MemberCanBeProtected.Global
		public Game()
		{
			SetBounds(300, 300, 500, 500);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			// ReSharper disable once VirtualMemberCallInConstructor
			DoubleBuffered = true;
			MaximizeBox = false;

			Icon = (System.Drawing.Icon) new ComponentResourceManager(typeof(Icon)).GetObject("icon");

			_syncContext = SynchronizationContext.Current;
			_gamePanel = new AbstractGame
			{
				OnClickAction = e => OnClick(e, Mouse),
				OnCustomDraw = OnCustomDraw
			};

			FormClosing += OnExit;

			// ReSharper disable once VirtualMemberCallInConstructor
			OnInit();
			_gamePanel.SetBounds(0, 0, Width, Height);
			Controls.Add(_gamePanel);
			Show();
			Run();
			// ReSharper disable VirtualMemberCallInConstructor
		}

		private void OnCustomDraw(Graphics g)
		{
			CustomDraw(g);
		}

		private readonly SynchronizationContext _syncContext;
		private readonly AbstractGame _gamePanel;

		public Random Random = new Random();

//        private readonly Graphics _gameScene;
//        private readonly Bitmap _screenCut;

		public new FPoint MousePosition()
			=> new FPoint(Control.MousePosition.Y - Bounds.Y, Control.MousePosition.X - Bounds.X);

		public FPoint Mouse
		{
			get { return MousePosition(); }
			set
			{
				MousePosition().X = (int) value.X;
				MousePosition().Y = (int) value.Y;
			}
		}

		/// <summary>
		/// a java-like setTitle method.
		/// set the window title.
		/// </summary>
		/// <param name="title">text of the title</param>
		// ReSharper disable once MemberCanBeProtected.Global
		public void SetTitle(string title) => Text = title;

		/// <summary>
		/// show fps or not.
		/// </summary>
		/// <param name="_bool">show fps or not.</param>
		public void SetShowFps(bool _bool) => _gamePanel.ShowFps = _bool;

		/// <summary>
		/// hide the cursor.
		/// </summary>
		public void HideCursor() => Cursor.Hide();

		/// <summary>
		/// set the global text font.
		/// </summary>
		/// <param name="font">the new font.</param>
		public void SetTextFont(Font font) => _gamePanel.TextFont = font;

		/// <summary>
		/// set if the engine should collect garbages itself.
		/// </summary>
		/// <param name="_bool">collect or not.</param>
		// ReSharper disable once InconsistentNaming
		public void SetAutoGC(bool _bool) => _gamePanel.AutoGC = _bool;

		/// <summary>
		/// add an object or text to screen.
		/// </summary>
		/// <param name="o">the object or text to be added.</param>
		public void AddObject(IAbstractObject o) => _gamePanel.AddObject(o);

		public void AddObjects(params IAbstractObject[] objects) => _gamePanel.AddObjects(objects);

		public void RemoveObjects(params IAbstractObject[] objects) => _gamePanel.RemoveObjects(objects);

		/// <summary>
		/// remove an object or text from screen.
		/// </summary>
		/// <param name="o">the object or text to be removed.</param>
		public void RemoveObject(IAbstractObject o) => _gamePanel.RemoveObject(o);

		/// <summary>
		/// clear all objects and texts
		/// </summary>
		public void ClearObjects() => _gamePanel.ClearObjects();

		/// <summary>
		/// add a timerListener
		/// </summary>
		/// <param name="t">the timeListener to be added.</param>
		public void AddTimeListener(FTimeListener t) => _gamePanel.FTimeListenerAddBuffer.Add(t);

		/// <summary>
		/// remove a timeListener
		/// </summary>
		/// <param name="t">the timeListener to be removed.</param>
		public void RemoveTimeListener(FTimeListener t) => _gamePanel.FTimeListenerDeleteBuffer.Add(t);

		/// <summary>
		/// clear the timeListeners.
		/// </summary>
		public void ClearTimeListeners()
		{
			foreach (var l in _gamePanel.FTimeListeners) _gamePanel.FTimeListenerDeleteBuffer.Add(l);
		}

		public virtual void OnInit()
		{
		}


		public virtual void OnExit(object sender, FormClosingEventArgs args)
		{
		}

		public virtual void OnRefresh()
		{
		}

		/// <summary>
		/// will be called when the window is clicked.
		/// </summary>
		/// <param name="e">event args</param>
		/// <param name="mousePosition">the position of the mouse</param>
		public virtual void OnClick(EventArgs e, FPoint mousePosition)
		{
		}

		/// <summary>
		/// draw what you want with a graphics class
		/// </summary>
		/// <param name="g"></param>
		public virtual void CustomDraw(Graphics g)
		{
		}

		/// <summary>
		/// 感谢ifdog老司机帮我修改这个问题。。。
		/// 再次感谢ifdog老司机！
		/// </summary>
		private void Run()
		{
			var fTimer2 = new FTimer2(1);
			fTimer2.Start(() => _syncContext.Send(state =>
			{
				OnRefresh();
				_gamePanel.IncreaseFps();
				_gamePanel.Refresh();
			}, null));
			new FTimer2(1000).Start(_gamePanel.ChangeFps);
		}

		private class AbstractGame : Panel
		{
			private readonly IList<IAbstractObject> _objects;
			private readonly IList<IAbstractObject> _objectAddBuffer;
			private readonly IList<IAbstractObject> _objectDeleteBuffer;

			private readonly IList<TextObject> _texts;
			private readonly IList<TextObject> _textAddBuffer;
			private readonly IList<TextObject> _textDeleteBuffer;

			internal readonly IList<FTimeListener> FTimeListeners;
			internal readonly IList<FTimeListener> FTimeListenerAddBuffer;
			internal readonly IList<FTimeListener> FTimeListenerDeleteBuffer;

			internal bool ShowFps = true;

			private long _fpsCounter;
			private long _fpsDisplay;

			internal Font TextFont = new Font(FontFamily.GenericSansSerif, 14);
			internal Action<EventArgs> OnClickAction;
			internal Action<Graphics> OnCustomDraw;

			// ReSharper disable once InconsistentNaming
			internal bool AutoGC = true;

			internal AbstractGame()
			{
				DoubleBuffered = true;

				_fpsCounter = 0;
				_objects = new List<IAbstractObject>(50);
				_objectAddBuffer = new List<IAbstractObject>(10);
				_objectDeleteBuffer = new List<IAbstractObject>(10);

				_texts = new List<TextObject>(20);
				_textAddBuffer = new List<TextObject>(10);
				_textDeleteBuffer = new List<TextObject>(10);

				FTimeListeners = new List<FTimeListener>(10);
				FTimeListenerAddBuffer = new List<FTimeListener>(10);
				FTimeListenerDeleteBuffer = new List<FTimeListener>(10);
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				// garbage collection
				if (AutoGC)
				{
					foreach (var o in _objects.Where(o =>
						o.X < -Width ||
						o.Y < -Height ||
						o.X > Width + Width ||
						o.Y > Height + Height))
					{
						if (o is PhysicalObject) ((PhysicalObject) o).Died = true;
						RemoveObject(o);
					}
				}

				ProcessBuffer();
				foreach (var l in FTimeListeners) l.Check();
				foreach (var o in _objects)
				{
					(o as FObject)?.RunAnims();
					(o as FObject)?.CheckCollitions();
				}

				var g = e.Graphics;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
				foreach (var o in _objects)
				{
					if (o is ShapeObject)
					{
						var brush = new SolidBrush((o as ShapeObject).ColorResource.Color);
						if ((o as ShapeObject).Shape is FRectangle)
							g.FillRectangle(brush,
								(float) o.X,
								(float) o.Y,
								(float) (o as ShapeObject).Width,
								(float) (o as ShapeObject).Height
							);
						else if ((o as ShapeObject).Shape is FOval)
							g.FillEllipse(brush,
								(float) o.X,
								(float) o.Y,
								(float) (o as ShapeObject).Width,
								(float) (o as ShapeObject).Height
							);
					}
					else if (o is ImageObject)
					{
						g.DrawImage((o as ImageObject).Bitmap, (o as ImageObject).Point);
					}
				}
				foreach (var t in _texts)
				{
					var brush = new SolidBrush(t.GetColor().Color);
					if (t is TextObject)
						g.DrawString(t.Text, TextFont, brush, (float) t.X, (float) t.Y);
				}
				if (ShowFps)
					g.DrawString("fps: " + _fpsDisplay, TextFont, new SolidBrush(Color.Black), 20, Height - 80);

				OnCustomDraw.Invoke(g);
				base.OnPaint(e);
			}

			internal void ChangeFps()
			{
				FLog.I("Refreshed");
				_fpsDisplay = _fpsCounter;
				_fpsCounter = 0;
			}

			protected override void OnClick(EventArgs e)
			{
				OnClickAction?.Invoke(e);
				base.OnClick(e);
			}

			internal void IncreaseFps() => ++_fpsCounter;

			internal void RemoveObject(IAbstractObject o)
			{
				if (o == null) return;
				if (o is TextObject) _textDeleteBuffer.Add((TextObject) o);
				else _objectDeleteBuffer.Add(o);
			}

			internal void ClearObjects()
			{
				foreach (var o in _objects) _objectDeleteBuffer.Add(o);
				foreach (var o in _texts) _textDeleteBuffer.Add(o);
			}

			internal void AddObject(IAbstractObject o)
			{
				if (o == null) return;
				if (o is TextObject) _textAddBuffer.Add((TextObject) o);
				else _objectAddBuffer.Add(o);
			}

			internal void AddObjects(params IAbstractObject[] objects) => objects.ToList().ForEach(AddObject);

			internal void RemoveObjects(params IAbstractObject[] objects) => objects.ToList().ForEach(RemoveObject);

			private void ProcessBuffer()
			{
				foreach (var o in _objectAddBuffer) _objects.Add(o);
				foreach (var o in _objectDeleteBuffer) _objects.Remove(o);
				_objectAddBuffer.Clear();
				_objectDeleteBuffer.Clear();

				foreach (var t in _textAddBuffer) _texts.Add(t);
				foreach (var t in _textDeleteBuffer) _texts.Remove(t);
				_textAddBuffer.Clear();
				_textDeleteBuffer.Clear();

				foreach (var t in FTimeListenerAddBuffer) FTimeListeners.Add(t);
				foreach (var t in FTimeListenerDeleteBuffer) FTimeListeners.Remove(t);
				FTimeListenerAddBuffer.Clear();
				FTimeListenerDeleteBuffer.Clear();
			}
		}
	}
}
