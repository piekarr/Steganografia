using System;

namespace Steganografia.Services.BitmapCoders
{
	public class BitmapCodeDataConfig
	{
		public int R { get; set; } = 1;
		public int G { get; set; } = 1;
		public int B { get; set; } = 1;

		public BitmapCodeDataConfig(int r, int g, int b)
		{
			Validate(r, g, b);
			R = r;
			G = g;
			B = b;
		}

		private void Validate(int r, int g, int b)
		{
			if (!IsValid(r) || !IsValid(g) || !IsValid(b))
			{
				throw new ArgumentException("Arguments should be in range 1-8");
			}
		}

		private bool IsValid(int r)
		{
			return r > 0 && r <= 8;
		}

		internal int GetBitsCount(PixelRGBValue selectedPixel)
		{
			switch (selectedPixel)
			{
				case PixelRGBValue.R:
					return R;
				case PixelRGBValue.G:
					return G;
				case PixelRGBValue.B:
					return B;
				default:
					throw new ArgumentException("Wrong selected Pixel value", "selectedPixel");
			}
		}
	}
}