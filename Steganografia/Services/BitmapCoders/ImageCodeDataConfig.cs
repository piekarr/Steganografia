using System;

namespace Steganografia.Services.BitmapCoders
{
    public class BitmapCodeDataConfig
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

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