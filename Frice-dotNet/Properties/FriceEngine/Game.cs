using System;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace Frice_dotNet.Properties.FriceEngine
{
	public class Game : Form
	{
		public Game()
		{
			SetBounds(100, 100, 500, 500);
			OnInit();
			Show();
			new Thread(Run).Start();
		}

		private ArrayList _objects = new ArrayList();
		private ArrayList _objectsAddBuffer = new ArrayList();
		private ArrayList _objectsDeleteBuffer = new ArrayList();

		private ArrayList _texts = new ArrayList();
		private ArrayList _textsAddBuffer = new ArrayList();
		private ArrayList _textsDeleteBuffer = new ArrayList();

		public void OnInit()
		{
		}

		public void OnRefresh()
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			switch ()
			{
			}
		}

		private void Run()
		{
			while (true)
			{
				OnRefresh();
			}
			// ReSharper disable once FunctionNeverReturns
		}
	}
}
