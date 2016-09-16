using System;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using Frice_dotNet.Properties.FriceEngine.Object;

namespace Frice_dotNet.Properties.FriceEngine
{
	public class Game : Form
	{
		public Game()
		{
			SetBounds(100, 100, 500, 500);
			OnInit(this, EventArgs.Empty);
			Show();
			new Thread(Run).Start();
		}

		private ArrayList _objects = new ArrayList();
		private ArrayList _objectsAddBuffer = new ArrayList();
		private ArrayList _objectsDeleteBuffer = new ArrayList();

		private ArrayList _texts = new ArrayList();
		private ArrayList _textsAddBuffer = new ArrayList();
		private ArrayList _textsDeleteBuffer = new ArrayList();

		public void AddObject(IAbstractObject o) => _objectsAddBuffer.Add(o);

		public void RemoveObject(IAbstractObject o) => _objectsDeleteBuffer.Add(o);

		public event EventHandler OnInit = delegate {  };

		public event EventHandler OnRefresh = delegate { };

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			foreach (var o in _objects)
			{
				if (o is ShapeObject)
				{
				}
			}
		}

		private void Run()
		{
			while (true)
			{
				OnRefresh(this, EventArgs.Empty);
			}
			// ReSharper disable once FunctionNeverReturns
		}
	}
}
