using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Frice_dotNet.Properties.FriceEngine.Object;
using Frice_dotNet.Properties.FriceEngine.Utils.Graphics;
using Frice_dotNet.Properties.FriceEngine.Utils.Message;
using Frice_dotNet.Properties.FriceEngine.Utils.Time;

namespace Frice_dotNet.Properties.FriceEngine
{
    public class Game : Form
    {
        public Game()
        {
            _timer = new FTimer(10);

            Objects = new List<IAbstractObject>();
            ObjectAddBuffer = new List<IAbstractObject>();
            ObjectDeleteBuffer = new List<IAbstractObject>();

            Texts = new List<FText>();
            TextAddBuffer = new List<FText>();
            TextDeleteBuffer = new List<FText>();

            FTimeListeners = new List<FTimeListener>();
            FTimeListenerAddBuffer = new List<FTimeListener>();
            FTimeListenerDeleteBuffer = new List<FTimeListener>();

            SetBounds(100, 100, 500, 500);
            OnInit();
//            _gameScene = CreateGraphics();
//            _screenCut = new Bitmap(Width, Height);
            ShowDialog();
            new Thread(Run).Start();
        }

        protected readonly IList<IAbstractObject> Objects;
        protected readonly IList<IAbstractObject> ObjectAddBuffer;
        protected readonly IList<IAbstractObject> ObjectDeleteBuffer;

        protected readonly IList<FText> Texts;
        protected readonly IList<FText> TextAddBuffer;
        protected readonly IList<FText> TextDeleteBuffer;

        protected readonly IList<FTimeListener> FTimeListeners;
        protected readonly IList<FTimeListener> FTimeListenerAddBuffer;
        protected readonly IList<FTimeListener> FTimeListenerDeleteBuffer;

        private readonly FTimer _timer;

//        private readonly Graphics _gameScene;
//        private readonly Bitmap _screenCut;

        public new Point MousePosition() => Control.MousePosition;

        /// <summary>
        /// add an object or text to screen.
        /// </summary>
        /// <param name="o">the object or text to be added.</param>
        public void AddObject(IAbstractObject o)
        {
            if (o == null) return;
            if (o is FText) TextAddBuffer.Add((FText) o);
            else ObjectAddBuffer.Add(o);
        }

        /// <summary>
        /// remove an object or text from screen.
        /// </summary>
        /// <param name="o">the object or text to be removed.</param>
        public void RemoveObject(IAbstractObject o)
        {
            if (o == null) return;
            if (o is FText) TextDeleteBuffer.Add((FText) o);
            else ObjectDeleteBuffer.Add(o);
        }

        /// <summary>
        /// clear all objects and texts
        /// </summary>
        public void ClearObjects()
        {
            foreach (var o in Objects) ObjectDeleteBuffer.Add(o);
            foreach (var o in Texts) TextDeleteBuffer.Add(o);
        }

        /// <summary>
        /// add a timerListener
        /// </summary>
        /// <param name="t">the timeListener to be added.</param>
        public void AddTimeListener(FTimeListener t) => FTimeListenerAddBuffer.Add(t);

        /// <summary>
        /// remove a timeListener
        /// </summary>
        /// <param name="t">the timeListener to be removed.</param>
        public void RemoveTimeListener(FTimeListener t) => FTimeListenerDeleteBuffer.Add(t);

        /// <summary>
        /// clear the timeListeners.
        /// </summary>
        public void ClearTimeListeners()
        {
            foreach (var l in FTimeListeners) FTimeListenerDeleteBuffer.Add(l);
        }

        public virtual void OnInit()
        {
        }

        public virtual void OnRefresh()
        {
        }

        public virtual void OnClick()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ProcessBuffer();
            foreach (var o in Objects) (o as FObject)?.HandleAnims();

            var g = e.Graphics;
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
                }
            foreach (var t in Texts)
            {
            }

            base.OnPaint(e);
        }

        private void Run()
        {
            while (true)
                if (_timer.Ended())
                {
                    OnRefresh();
                    Refresh();
                    FLog.Info("repaint");
                }
            // ReSharper disable once FunctionNeverReturns
        }

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