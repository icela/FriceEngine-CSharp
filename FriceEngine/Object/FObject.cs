﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using FriceEngine.Animation;
using FriceEngine.Resource;
using FriceEngine.Utils.Graphics;
using FriceEngine.Utils.Misc;

namespace FriceEngine.Object
{
	public interface IAbstractObject
	{
		double X { get; set; }
		double Y { get; set; }

		double Rotate { get; set; }
	}

	public class AbstractObject : IAbstractObject
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Rotate { get; set; }

		public AbstractObject(double x, double y)
		{
			X = x;
			Y = y;
		}
	}

	public interface IFContainer
	{
		double Width { get; set; }
		double Height { get; set; }
	}

	public interface ICollideBox
	{
		bool IsCollide(ICollideBox other);
	}

	public abstract class PhysicalObject : IAbstractObject, IFContainer
	{
		public virtual double X { get; set; }
		public virtual double Y { get; set; }

		public virtual double Width { get; set; }
		public virtual double Height { get; set; }

		public double Rotate { get; set; } = 0;

		public bool Died { get; set; } = false;

		private double _mass = 1;

		public double Mass
		{
			get { return _mass; }
			set { _mass = value <= 0 ? 0.001 : value; }
		}
	}

	public abstract class FObject : PhysicalObject, ICollideBox
	{
		protected FObject()
		{
			MoveList = new List<MoveAnim>();
		}

		public List<MoveAnim> MoveList { get; }
		public List<Pair<ICollideBox, Action>> TargetList { get; }

		public void Move(double x, double y)
		{
			X += x;
			Y += y;
		}

		public void Move(DoublePair p) => Move(p.X, p.Y);

		public void RunAnims()
		{
			foreach (var anim in MoveList) Move(anim.Delta);
		}

		public void CheckCollitions()
		{
			foreach (var p in TargetList)
			{
				if (IsCollide(p.First))
				{
					p.Second.Invoke();
				}
			}
		}

		/// <summary>
		/// check if two collideBoxes are collided.
		/// if 'other' isn't a PhysicalObject, it will always return false;
		/// </summary>
		/// <param name="other">the other collide box</param>
		/// <returns>collided or not.</returns>
		public bool IsCollide(ICollideBox other)
		{
			if (other is PhysicalObject)
				return X + Width >= ((PhysicalObject) other).X && ((PhysicalObject) other).Y <= Y + Height &&
						X <= ((PhysicalObject) other).X + ((PhysicalObject) other).Width &&
						Y <= ((PhysicalObject) other).Y + ((PhysicalObject) other).Height;
			return false;
		}

		public bool ContainsPoint(double px, double py) => px >= X && px <= X + Width && py >= Y && py <= Y + Height;
		public bool ContainsPoint(int px, int py) => px >= X && px <= X + Width && py >= Y && py <= Y + Height;
	}

	public sealed class ShapeObject : FObject
	{
		public IFShape Shape;
		public ColorResource ColorResource;

		public override double Width
		{
			get { return Shape.Width; }
			set { Shape.Width = value; }
		}

		public override double Height
		{
			get { return Shape.Height; }
			set { Shape.Height = value; }
		}

		public ShapeObject(ColorResource colorResource, IFShape shape, double x, double y)
		{
			ColorResource = colorResource;
			Shape = shape;
			X = x;
			Y = y;
		}
	}

	public sealed class ImageObject : FObject
	{
		public ImageResource Res { get; set; }

		public Bitmap Bitmap
		{
			get { return Res.Bmp; }
			set { Res.Bmp = value; }
		}

		public Point Point { get; set; }
		private double _x;
		private double _y;

		public override double X
		{
			get { return _x; }
			set
			{
				_x = value;
				Point = new Point(Convert.ToInt32(_x), Convert.ToInt32(_y));
			}
		}

		public override double Y
		{
			get { return _y; }
			set
			{
				_y = value;
				Point = new Point(Convert.ToInt32(_x), Convert.ToInt32(_y));
			}
		}

		public override double Height
		{
			get { return Bitmap.Height; }
			set { Bitmap = _resize(Bitmap, Convert.ToInt32(Width), Convert.ToInt32(value)); }
		}

		public override double Width
		{
			get { return Bitmap.Width; }
			set { Bitmap = _resize(Bitmap, Convert.ToInt32(value), Convert.ToInt32(Height)); }
		}


		public ImageObject(ImageResource img, double x, double y)
		{
			Res = img;
			_x = x;
			_y = y;
			Point = new Point(Convert.ToInt32(_x), Convert.ToInt32(_y));
		}

		public ImageObject(Bitmap img, double x, double y)
		{
			Res = new ImageResource(img);
			_x = x;
			_y = y;
			Point = new Point(Convert.ToInt32(_x), Convert.ToInt32(_y));
		}

		public static ImageObject FromWeb(string url, double x, double y, int width = -1, int height = -1)
		{
			var r = WebRequest.Create(url).GetResponse() as HttpWebResponse;
			using (var imageStream = r?.GetResponseStream())
			{
				var img = imageStream == null ? null : new ImageObject(new Bitmap(imageStream, true), x, y);
				if (width > 0 && img != null) img.Width = width;
				if (height > 0 && img != null) img.Height = height;
				return img;
			}
		}

		/// <summary>
		/// 感谢ifdog老司机！
		/// </summary>
		/// <param name="oldBitmap">the original bitmap</param>
		/// <param name="newW">new bitmap width</param>
		/// <param name="newH">new bitmap height</param>
		/// <returns>scaled bitmap</returns>
		/// <author>ifdog</author>
		private static Bitmap _resize(Image oldBitmap, int newW, int newH)
		{
			var b = new Bitmap(newW, newH);
			using (var g = Graphics.FromImage(b))
			{
				g.Clear(Color.Transparent);
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.DrawImage(oldBitmap, new Rectangle(0, 0, newW, newH), 0, 0,
					oldBitmap.Width,
					oldBitmap.Height,
					GraphicsUnit.Pixel);
				return b;
			}
		}

		/// <summary>
		/// 感谢ifdog老司机！
		/// </summary>
		/// <param name="path">image path</param>
		/// <param name="x">position x</param>
		/// <param name="y">position y</param>
		/// <param name="width">image width, defaultly original size.</param>
		/// <param name="height">image height, defaultly original size.</param>
		/// <returns></returns>
		public static ImageObject FromFile(string path, double x, double y, int width = -1, int height = -1)
		{
			var img = new ImageObject(new Bitmap(path, true), x, y);
			if (width > 0) img.Width = width;
			if (height > 0) img.Height = height;
			return img;
		}
	}

	public class DoublePair
	{
		public double X;
		public double Y;

		public DoublePair(double y, double x)
		{
			Y = y;
			X = x;
		}

		public static DoublePair From1000(double x, double y) => new DoublePair(x / 1000.0, y / 1000.0);

		public static DoublePair FromKilo(double x, double y) => From1000(x, y);

		public static DoublePair FromTicks(long x, long y) => new DoublePair(x / 1e7, y / 1e7);
		public static DoublePair FromTicks(double x, double y) => new DoublePair(x / 1e7, y / 1e7);
	}
}
