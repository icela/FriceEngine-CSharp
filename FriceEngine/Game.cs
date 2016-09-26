using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
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
				OnClickAction = () =>
						OnClick(new FPoint(MousePosition().X, MousePosition().Y))
			};

			// ReSharper disable once VirtualMemberCallInConstructor
			OnInit();
			_gamePanel.SetBounds(0, 0, Width, Height);
			Controls.Add(_gamePanel);
			Show();
			Run();
			// ReSharper disable VirtualMemberCallInConstructor
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

		public virtual void OnRefresh()
		{
		}

		/// <summary>
		/// will be called when the window is clicked.
		/// </summary>
		/// <param name="mousePosition">the position of the mouse</param>
		public virtual void OnClick(FPoint mousePosition)
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
			new FTimer2(1000).Start(() => _gamePanel.ChangeFps());
		}

		private class AbstractGame : Panel
		{
			private readonly IList<IAbstractObject> Objects;
			internal readonly IList<IAbstractObject> ObjectAddBuffer;
			internal readonly IList<IAbstractObject> ObjectDeleteBuffer;

			private readonly IList<FText> Texts;
			internal readonly IList<FText> TextAddBuffer;
			internal readonly IList<FText> TextDeleteBuffer;

			internal readonly IList<FTimeListener> FTimeListeners;
			internal readonly IList<FTimeListener> FTimeListenerAddBuffer;
			internal readonly IList<FTimeListener> FTimeListenerDeleteBuffer;

			internal bool ShowFps = true;

			private long _fpsCounter;
			private long _fpsDisplay;

			internal Font TextFont = new Font(FontFamily.GenericSansSerif, 14);
			internal Action OnClickAction;
			// ReSharper disable once InconsistentNaming
			internal bool AutoGC = true;

			internal AbstractGame()
			{
				DoubleBuffered = true;

				_fpsCounter = 0;
				Objects = new List<IAbstractObject>();
				ObjectAddBuffer = new List<IAbstractObject>();
				ObjectDeleteBuffer = new List<IAbstractObject>();

				Texts = new List<FText>();
				TextAddBuffer = new List<FText>();
				TextDeleteBuffer = new List<FText>();

				FTimeListeners = new List<FTimeListener>();
				FTimeListenerAddBuffer = new List<FTimeListener>();
				FTimeListenerDeleteBuffer = new List<FTimeListener>();
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				ProcessBuffer();
				foreach (var l in FTimeListeners) l.Check();
				foreach (var o in Objects) (o as FObject)?.HandleAnims();

				var g = e.Graphics;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
				foreach (var o in Objects)
				{
					// GC
					if (AutoGC && (o.X < -Width || o.Y < -Height || o.X > Width + Width || o.Y > Height + Height))
						RemoveObject(o);
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
						g.DrawImage((o as ImageObject).Bmp, (o as ImageObject).Point);
					}
				}
				foreach (var t in Texts)
				{
					var brush = new SolidBrush(t.GetColor().Color);
					if (t is SimpleText)
						g.DrawString(t.Text, TextFont, brush, (float) t.X, (float) t.Y);
				}
				if (ShowFps)
					g.DrawString("fps: " + _fpsDisplay, DefaultFont, new SolidBrush(Color.Black), 20, Height - 80);

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
				OnClickAction?.Invoke();
				base.OnClick(e);
			}

			internal void IncreaseFps() => ++_fpsCounter;

			internal void RemoveObject(IAbstractObject o)
			{
				if (o == null) return;
				if (o is FText) TextDeleteBuffer.Add((FText) o);
				else ObjectDeleteBuffer.Add(o);
			}

			internal void ClearObjects()
			{
				foreach (var o in Objects) ObjectDeleteBuffer.Add(o);
				foreach (var o in Texts) TextDeleteBuffer.Add(o);
			}

			internal void AddObject(IAbstractObject o)
			{
				if (o == null) return;
				if (o is FText) TextAddBuffer.Add((FText) o);
				else ObjectAddBuffer.Add(o);
			}

			internal void AddObjects(params IAbstractObject[] objects) => objects.ToList().ForEach(AddObject);

			internal void RemoveObjects(params IAbstractObject[] objects) => objects.ToList().ForEach(RemoveObject);

			private void ProcessBuffer()
			{
				foreach (var o in ObjectAddBuffer) Objects.Add(o);
				foreach (var o in ObjectDeleteBuffer) Objects.Remove(o);
				ObjectAddBuffer.Clear();
				ObjectDeleteBuffer.Clear();

				foreach (var t in TextAddBuffer) Texts.Add(t);
				foreach (var t in TextDeleteBuffer) Texts.Remove(t);
				TextAddBuffer.Clear();
				TextDeleteBuffer.Clear();

				foreach (var t in FTimeListenerAddBuffer) FTimeListeners.Add(t);
				foreach (var t in FTimeListenerDeleteBuffer) FTimeListeners.Remove(t);
				FTimeListenerAddBuffer.Clear();
				FTimeListenerDeleteBuffer.Clear();
			}
		}
	}
}