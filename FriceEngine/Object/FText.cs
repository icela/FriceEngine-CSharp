﻿using FriceEngine.Resource;

namespace FriceEngine.Object
{

	public class TextObject : FObject
	{
		public string Text;
		public double Size;
		public ColorResource ColorResource;
		public override double X { get; set; }
		public override double Y { get; set; }
		public TextObject(ColorResource colorResource, string text, double size, double x, double y)
		{
			this.ColorResource = colorResource;
			this.Text = text;
			this.X = x;
			this.Y = y;
			this.Size = size;
		}

		public ColorResource GetColor() => this.ColorResource;

		public override string ToString()
		{
			return Text;
		}
	}
}
