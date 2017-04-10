using System;
using System.Drawing;

namespace Steganografia.Services.BitmapCoders
{
    public abstract class ImageDataBaseService
    {
        protected static void IncreaseRowAndColumn(Bitmap result, ref int row, ref int column)
        {
            column++;
            if (column > result.Width)
            {
                row++;
                column = 0;
            }
        }
        protected static PixelRGBValue IncreasePixelRGBValue(PixelRGBValue selectedPixel, ref bool shouldIncreaseRowAndColumn)
        {
            switch (selectedPixel)
            {
                case PixelRGBValue.R:
                    return PixelRGBValue.G;
                case PixelRGBValue.G:
                    return PixelRGBValue.B;
                case PixelRGBValue.B:
                    shouldIncreaseRowAndColumn = true;
                    return PixelRGBValue.R;
                default:
                    throw new ArgumentException("Wrong PixelRGBValue value", "selectedPixel");
            }
        }
    }
}
