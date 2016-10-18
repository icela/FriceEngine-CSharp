using FriceEngine;
using FriceEngine.Animation;
using FriceEngine.Object;
using FriceEngine.Utils.Time;

namespace FriceEngineTest
{
	public class Demo : WpfGame
	{
		private ImageObject _bird;

		private ImageObject[] _lo =
		{
			ImageObject.FromFile("lo1.png", 600, 400),
			ImageObject.FromFile("lo2.png", 600, 400),
			ImageObject.FromFile("lo3.png", 600, 400),
			ImageObject.FromFile("lo4.png", 600, 400),
			ImageObject.FromFile("lo5.png", 600, 400)
		};

		private ImageObject[] _lou =
		{
			ImageObject.FromFile("lo1u.png", 600, 0),
			ImageObject.FromFile("lo2u.png", 600, 0),
			ImageObject.FromFile("lo3u.png", 600, 0),
			ImageObject.FromFile("lo4u.png", 600, 0),
			ImageObject.FromFile("lo5u.png", 600, 0)
		};

		private int _loLast;
		private int _louLast;

		public override void OnInit()
		{
			Height = 640;
			Width = 480;
			_bird = ImageObject.FromFile("an.png", 20, 300);
			ResetGravity();
			AddObject(_bird);
			AddTimelistener(new FTimeListener(1000, () =>
			{
				_lou[_louLast].ClearAnims();
				_lo[_loLast].ClearAnims();
				RemoveObject(_lo[_loLast], _lou[_louLast]);
				_loLast = Random.Next(_lo.Length);
				_louLast = Random.Next(_lou.Length);
				_lou[_louLast].X = 600;
				_lo[_loLast].X = 600;
				_lou[_louLast].AddAnims(new SimpleMove(-100, 0));
				_lo[_loLast].AddAnims(new SimpleMove(-100, 0));
				AddObject(_lo[_loLast], _lou[_louLast]);
			}));
			base.OnInit();
		}

		public override void OnClick(double x, double y, int button)
		{
			_bird.ClearAnims();
			ResetGravity();
			base.OnClick(x, y, button);
		}

		public override void OnLoseFocus()
		{
			GamePause();
			base.OnLoseFocus();
		}

		public override void OnFocus()
		{
			GameStart();
			base.OnFocus();
		}

		private void ResetGravity() => _bird.AddAnims(new AccelerateMove(0, 1800), new SimpleMove(0, -500));
	}
}
