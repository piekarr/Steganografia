using System;
using System.Drawing;
using System.IO;

namespace Steganografia.Services.BitmapCoders
{
	public class ImageDataCoderService : ImageDataBaseService
	{
		public Bitmap CodeDataToImage(Bitmap bitmap, Stream data, BitmapCodeDataConfig config)
		{
			var result = (Bitmap)bitmap.Clone();
			int readedData;
			int column = 0, row = 0;
			PixelRGBValue selectedPixel = PixelRGBValue.R;
			int savedBits = 0;
			while ((readedData = data.ReadByte()) != -1)
			{
				result = WriteDataToBitmap(result, readedData, config, ref column, ref row, ref selectedPixel, ref savedBits);
			}
			result = WriteDataToBitmap(result, 0, config, ref column, ref row, ref selectedPixel, ref savedBits);
			data.Dispose();
			return result;
		}

		private static Bitmap WriteDataToBitmap(Bitmap result, int readedData, BitmapCodeDataConfig config, ref int column, ref int row, ref PixelRGBValue selectedPixel, ref int savedBits)
		{
			for (int savedBitsInByte = 0; savedBitsInByte < 8; savedBitsInByte++)
			{
				bool shouldIncreaseRowAndColumn = false;
				var pixelColor = result.GetPixel(row, column);
				var newPixelColor = CodeBitDataToPixelColor(pixelColor, config, savedBitsInByte, readedData, ref selectedPixel, ref savedBits, ref shouldIncreaseRowAndColumn);
				result.SetPixel(row, column, newPixelColor);

				if (shouldIncreaseRowAndColumn)
				{
					IncreaseRowAndColumn(result, ref row, ref column);
				}
			}
			return result;
		}

		private static Color CodeBitDataToPixelColor(Color pixelColor, BitmapCodeDataConfig config, int savedBitsInByte, int readedData, ref PixelRGBValue selectedPixel, ref int savedBits, ref bool shouldIncreaseRowAndColumn)
		{
			pixelColor = SetColorValue(pixelColor, config, selectedPixel, savedBitsInByte, readedData, ref savedBits);
			if (config.GetBitsCount(selectedPixel) == savedBits)
			{
				selectedPixel = IncreasePixelRGBValue(selectedPixel, ref shouldIncreaseRowAndColumn);
				savedBits = 0;
			}
			return pixelColor;
		}

		private static Color SetColorValue(Color pixelColor, BitmapCodeDataConfig config, PixelRGBValue selectedPixel, int savedBitsInByte, int readedData, ref int savedBits)
		{
			int r = pixelColor.R;
			int g = pixelColor.G;
			int b = pixelColor.B;
			switch (selectedPixel)
			{
				case PixelRGBValue.R:
					r = CalculateNewColorPropertyValue(pixelColor.R, config.R, readedData, savedBitsInByte, ref savedBits);
					break;
				case PixelRGBValue.G:
					g = CalculateNewColorPropertyValue(pixelColor.G, config.G, readedData, savedBitsInByte, ref savedBits);
					break;
				case PixelRGBValue.B:
					b = CalculateNewColorPropertyValue(pixelColor.B, config.B, readedData, savedBitsInByte, ref savedBits);
					break;
				default:
					throw new ArgumentException("Wrong PixelRGBValue value", "selectedPixel");
			}
			return Color.FromArgb(pixelColor.A, r, g, b);
		}

		private static int CalculateNewColorPropertyValue(int value, int configBytesValue, int readedData, int savedBitsInByte, ref int savedBits)
		{
			var bitToAppend = (readedData >> (8 - savedBitsInByte - 1)) & 1;
			value = value >> (configBytesValue - savedBits);
			value = (value << (configBytesValue - savedBits)) + (bitToAppend << (configBytesValue - savedBits - 1));
			savedBits++;
			return value;
		}
	}
}
