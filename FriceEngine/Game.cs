﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using FriceEngine.Object;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Message;
using FriceEngine.Utils.Time;

namespace FriceEngine
{
	public class AbstractGame : Panel
	{
		internal readonly IList<IAbstractObject> Objects;
		internal readonly IList<IAbstractObject> ObjectAddBuffer;
		internal readonly IList<IAbstractObject> ObjectDeleteBuffer;

		internal readonly IList<FText> Texts;
		internal readonly IList<FText> TextAddBuffer;
		internal readonly IList<FText> TextDeleteBuffer;

		internal readonly IList<FTimeListener> FTimeListeners;
		internal readonly IList<FTimeListener> FTimeListenerAddBuffer;
		internal readonly IList<FTimeListener> FTimeListenerDeleteBuffer;

		internal bool ShowFps = true;

		private long _fpsCounter;
		private long _fpsDisplay;
		private Action _onClickAction;

		internal AbstractGame(Action onClick)
		{
			DoubleBuffered = true;
			_onClickAction = onClick;

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
			foreach (var t in Texts)
			{
				var brush = new SolidBrush(t.GetColor().Color);
				if (t is SimpleText)
					g.DrawString(t.Text, DefaultFont, brush, (float) t.X, (float) t.Y);
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
			_onClickAction.Invoke();
			base.OnClick(e);
		}

		internal void IncreaseFps() => ++_fpsCounter;

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

	public class Game : Form
	{
		// ReSharper disable once MemberCanBeProtected.Global
		public Game()
		{
			SetBounds(100, 100, 500, 500);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			// ReSharper disable once VirtualMemberCallInConstructor
			DoubleBuffered = true;
			MaximizeBox = false;

			Icon = (System.Drawing.Icon) new ComponentResourceManager(typeof(Icon)).GetObject("icon");

			_syncContext = SynchronizationContext.Current;
			_gamePanel = new AbstractGame(() => OnClick(new FPoint(MousePosition().X, MousePosition().Y)));
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

//        private readonly Graphics _gameScene;
//        private readonly Bitmap _screenCut;

		public new Point MousePosition() => Control.MousePosition;

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
		/// add an object or text to screen.
		/// </summary>
		/// <param name="o">the object or text to be added.</param>
		public void AddObject(IAbstractObject o)
		{
			if (o == null) return;
			if (o is FText) _gamePanel.TextAddBuffer.Add((FText) o);
			else _gamePanel.ObjectAddBuffer.Add(o);
		}

		/// <summary>
		/// remove an object or text from screen.
		/// </summary>
		/// <param name="o">the object or text to be removed.</param>
		public void RemoveObject(IAbstractObject o)
		{
			if (o == null) return;
			if (o is FText) _gamePanel.TextDeleteBuffer.Add((FText) o);
			else _gamePanel.ObjectDeleteBuffer.Add(o);
		}

		/// <summary>
		/// clear all objects and texts
		/// </summary>
		public void ClearObjects()
		{
			foreach (var o in _gamePanel.Objects) _gamePanel.ObjectDeleteBuffer.Add(o);
			foreach (var o in _gamePanel.Texts) _gamePanel.TextDeleteBuffer.Add(o);
		}

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
	}
}