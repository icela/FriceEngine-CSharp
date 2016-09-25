using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FriceEngine.Object;
using FriceEngine.Utils.Graphics;
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

        internal AbstractGame()
        {
            SetBounds(100, 100, 500, 500);
            Objects = new List<IAbstractObject>();
            ObjectAddBuffer = new List<IAbstractObject>();
            ObjectDeleteBuffer = new List<IAbstractObject>();

            Texts = new List<FText>();
            TextAddBuffer = new List<FText>();
            TextDeleteBuffer = new List<FText>();

            FTimeListeners = new List<FTimeListener>();
            FTimeListenerAddBuffer = new List<FTimeListener>();
            FTimeListenerDeleteBuffer = new List<FTimeListener>();
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ProcessBuffer();
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
            }
            base.OnPaint(e);
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

    public class Game : Form
    {
        public Game()
        {
            //_timer = new FTimer(10);
            _syncContext = SynchronizationContext.Current;
            GamePanel = new AbstractGame();
            OnInit();
            GamePanel.SetBounds(0, 0, Width, Height);
            Controls.Add(GamePanel);
            Show();
            Run();
            // ReSharper disable VirtualMemberCallInConstructor
        }

        private SynchronizationContext _syncContext;
        protected readonly AbstractGame GamePanel;

//        private readonly FTimer _timer;

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
            if (o is FText) GamePanel.TextAddBuffer.Add((FText) o);
            else GamePanel.ObjectAddBuffer.Add(o);
        }

        /// <summary>
        /// remove an object or text from screen.
        /// </summary>
        /// <param name="o">the object or text to be removed.</param>
        public void RemoveObject(IAbstractObject o)
        {
            if (o == null) return;
            if (o is FText) GamePanel.TextDeleteBuffer.Add((FText) o);
            else GamePanel.ObjectDeleteBuffer.Add(o);
        }

        /// <summary>
        /// clear all objects and texts
        /// </summary>
        public void ClearObjects()
        {
            foreach (var o in GamePanel.Objects) GamePanel.ObjectDeleteBuffer.Add(o);
            foreach (var o in GamePanel.Texts) GamePanel.TextDeleteBuffer.Add(o);
        }

        /// <summary>
        /// add a timerListener
        /// </summary>
        /// <param name="t">the timeListener to be added.</param>
        public void AddTimeListener(FTimeListener t) => GamePanel.FTimeListenerAddBuffer.Add(t);

        /// <summary>
        /// remove a timeListener
        /// </summary>
        /// <param name="t">the timeListener to be removed.</param>
        public void RemoveTimeListener(FTimeListener t) => GamePanel.FTimeListenerDeleteBuffer.Add(t);

        /// <summary>
        /// clear the timeListeners.
        /// </summary>
        public void ClearTimeListeners()
        {
            foreach (var l in GamePanel.FTimeListeners) GamePanel.FTimeListenerDeleteBuffer.Add(l);
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

        private void Run()
        {
//            while (true)
//                if (_timer.Ended())
//            {
            FTimer2 fTimer2 = new FTimer2(50);
            fTimer2.Start(() =>
            {
                _syncContext.Send((state) =>
                {
                    OnRefresh();
                    GamePanel.Refresh();
                }, null);
            });

//           }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}