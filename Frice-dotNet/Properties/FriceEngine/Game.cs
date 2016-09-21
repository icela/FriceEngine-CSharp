using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Frice_dotNet.Properties.FriceEngine.Object;
using Frice_dotNet.Properties.FriceEngine.Utils.Graphics;
using Frice_dotNet.Properties.FriceEngine.Utils.Message;

namespace Frice_dotNet.Properties.FriceEngine
{
    public class Game : Form
    {
        public Game()
        {
            _objects = new List<IAbstractObject>();
            _objectsAddBuffer = new List<IAbstractObject>();
            _objectsDeleteBuffer = new List<IAbstractObject>();
            _texts = new List<IAbstractObject>();
            _textsAddBuffer = new List<IAbstractObject>();
            _textsDeleteBuffer = new List<IAbstractObject>();
            SetBounds(100, 100, 500, 500);
            OnInit();
            OnClick();
            ShowDialog();
            new Thread(Run).Start();
        }

        private readonly IList<IAbstractObject> _objects;
        private readonly IList<IAbstractObject> _objectsAddBuffer;
        private readonly IList<IAbstractObject> _objectsDeleteBuffer;

        private readonly IList<IAbstractObject> _texts;
        private readonly IList<IAbstractObject> _textsAddBuffer;
        private readonly IList<IAbstractObject> _textsDeleteBuffer;

        public new Point MousePosition() => Control.MousePosition;

        public void AddObject(IAbstractObject o) => _objectsAddBuffer.Add(o);

        public void RemoveObject(IAbstractObject o) => _objectsDeleteBuffer.Add(o);

        public virtual void OnInit()
        {
        }

        public virtual void OnRefresh()
        {
        }

//        public virtual void OnClick()
//        {
//        }

        protected override void OnPaint(PaintEventArgs e)
        {
            HandleBuffer();

            var g = e.Graphics;
            foreach (var o in _objects)
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
            foreach (var t in _texts)
            {
            }

            base.OnPaint(e);
        }

        private void Run()
        {
            while (true)
            {
                Thread.Sleep(10);
                OnRefresh();
                Refresh();
                FLog.Info("repaint");
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void HandleBuffer()
        {
            foreach (var o in _objectsAddBuffer) _objects.Add(o);
            _objectsAddBuffer.Clear();
            foreach (var o in _objectsDeleteBuffer) _objects.Remove(o);
            _objectsDeleteBuffer.Clear();

            foreach (var o in _textsAddBuffer) _texts.Add(o);
            _textsAddBuffer.Clear();
            foreach (var o in _textsDeleteBuffer) _texts.Remove(o);
            _textsDeleteBuffer.Clear();
        }
    }
}