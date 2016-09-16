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

		public void OnInit()
		{
		}

		public void OnRefresh()
		{
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
