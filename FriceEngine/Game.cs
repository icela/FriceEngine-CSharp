﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FriceEngine.Object;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Message;
using FriceEngine.Utils.Time;
using JetBrains.Annotations;

namespace FriceEngine
{
	public class WinFormGame : Form
	{
		// ReSharper disable once MemberCanBeProtected.Global
		public WinFormGame()
		{
			Random = new Random(DateTime.Now.Millisecond);
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

		private void OnCustomDraw([NotNull] Graphics g) => CustomDraw(g);

		[NotNull] private readonly SynchronizationContext _syncContext;
		[NotNull] private readonly AbstractGame _gamePanel;

		[NotNull] public Random Random;

//		private readonly Graphics _gameScene;
//		private readonly Bitmap _screenCut;

		[NotNull]
		public new FPoint MousePosition()
			=> new FPoint(Control.MousePosition.Y - Bounds.Y, Control.MousePosition.X - Bounds.X);

		[NotNull]
		public FPoint Mouse
		{
			[NotNull] get => MousePosition();
			[NotNull]
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
		public void SetTitle([NotNull] string title) => Text = title;

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
		public void SetTextFont([NotNull] Font font) => _gamePanel.TextFont = font;

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
		public void AddObject([NotNull] IAbstractObject o) => _gamePanel.AddObject(o);

		public void AddObjects([NotNull] params IAbstractObject[] objects) => _gamePanel.AddObjects(objects);

		public void RemoveObjects([NotNull] params IAbstractObject[] objects) => _gamePanel.RemoveObjects(objects);

		/// <summary>
		/// remove an object or text from screen.
		/// </summary>
		/// <param name="o">the object or text to be removed.</param>
		public void RemoveObject([NotNull] IAbstractObject o) => _gamePanel.RemoveObject(o);

		/// <summary>
		/// clear all objects and texts
		/// </summary>
		public void ClearObjects() => _gamePanel.ClearObjects();

		/// <summary>
		/// add a timerListener
		/// </summary>
		/// <param name="t">the timeListener to be added.</param>
		public void AddTimeListener([NotNull] FTimeListener t) => _gamePanel.FTimeListenerAddBuffer.Add(t);

		/// <summary>
		/// remove a timeListener
		/// </summary>
		/// <param name="t">the timeListener to be removed.</param>
		public void RemoveTimeListener([NotNull] FTimeListener t) => _gamePanel.FTimeListenerDeleteBuffer.Add(t);

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


		public virtual void OnExit(
			[NotNull] object sender,
			[NotNull] FormClosingEventArgs args)
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
		public virtual void OnClick(
			[NotNull] EventArgs e,
			[NotNull] FPoint mousePosition)
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
			var fTimer2 = new FTimer(1);
			fTimer2.Start(() => _syncContext.Send(state =>
			{
				OnRefresh();
				_gamePanel.IncreaseFps();
				_gamePanel.Refresh();
			}, null));
			new FTimer(1000).Start(_gamePanel.ChangeFps);
		}

		private class AbstractGame : Panel
		{
			[NotNull] private readonly IList<IAbstractObject> _objects;
			[NotNull] private readonly IList<IAbstractObject> _objectAddBuffer;
			[NotNull] private readonly IList<IAbstractObject> _objectDeleteBuffer;

			[NotNull] private readonly IList<TextObject> _texts;
			[NotNull] private readonly IList<TextObject> _textAddBuffer;
			[NotNull] private readonly IList<TextObject> _textDeleteBuffer;

			[NotNull] internal readonly IList<FTimeListener> FTimeListeners;
			[NotNull] internal readonly IList<FTimeListener> FTimeListenerAddBuffer;
			[NotNull] internal readonly IList<FTimeListener> FTimeListenerDeleteBuffer;

			internal bool ShowFps = true;

			private long _fpsCounter;
			private long _fpsDisplay;

			[NotNull] internal Font TextFont = new Font(FontFamily.GenericSansSerif, 14);
			[CanBeNull] internal Action<EventArgs> OnClickAction;
			[CanBeNull] internal Action<Graphics> OnCustomDraw;

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
						if (o is PhysicalObject po) po.Died = true;
						RemoveObject(o);
					}
				}

				ProcessBuffer();
				foreach (var o in _objects)
				{
					if (!(o is FObject f)) continue;
					f.RunAnims();
					f.CheckCollitions();
				}

				var g = e.Graphics;
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				foreach (var o in _objects)
				{
					if (o is ShapeObject shape)
					{
						var brush = new SolidBrush(shape.ColorResource.Color);
						if (shape.Shape is FRectangle)
							g.FillRectangle(brush,
								(float) o.X,
								(float) o.Y,
								(float) shape.Width,
								(float) shape.Height
							);
						else if (shape.Shape is FOval)
							g.FillEllipse(brush,
								(float) o.X,
								(float) o.Y,
								(float) shape.Width,
								(float) shape.Height
							);
					}
					else if (o is ImageObject image)
						g.DrawImage(image.Bitmap, image.Point);
				}
				foreach (var t in _texts)
				{
					var brush = new SolidBrush(t.GetColor().Color);
					g.DrawString(t.Text, TextFont, brush, (float) t.X, (float) t.Y);
				}
				if (ShowFps)
					g.DrawString("fps: " + _fpsDisplay, TextFont, new SolidBrush(Color.Black), 20, Height - 80);

				OnCustomDraw?.Invoke(g);
				base.OnPaint(e);
			}

			internal void ChangeFps()
			{
				FLog.I("Refreshed");
				_fpsDisplay = _fpsCounter;
				_fpsCounter = 0;
			}

			protected override void OnClick([NotNull] EventArgs e)
			{
				OnClickAction?.Invoke(e);
				base.OnClick(e);
			}

			internal void IncreaseFps() => ++_fpsCounter;

			internal void RemoveObject([NotNull] IAbstractObject o)
			{
				if (o is TextObject text) _textDeleteBuffer.Add(text);
				else _objectDeleteBuffer.Add(o);
			}

			internal void ClearObjects()
			{
				foreach (var o in _objects) _objectDeleteBuffer.Add(o);
				foreach (var o in _texts) _textDeleteBuffer.Add(o);
			}

			internal void AddObject([NotNull] IAbstractObject o)
			{
				if (o is TextObject text) _textAddBuffer.Add(text);
				else _objectAddBuffer.Add(o);
			}

			internal void AddObjects([NotNull] params IAbstractObject[] objects) => objects.ToList().ForEach(AddObject);

			internal void RemoveObjects([NotNull] params IAbstractObject[] objects) => objects.ToList().ForEach(RemoveObject);

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