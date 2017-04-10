using System;
using System.Drawing;
using System.IO;

namespace Steganografia.Services.BitmapCoders
{
    public class ImageDataEncoderService : ImageDataBaseService
    {
        public Stream EncodeDataFromImage(Bitmap bitmap, BitmapCodeDataConfig config)
        {
            var streamWriter = new MemoryStream();
            int row = 0, column = 0;
            PixelRGBValue selectedPixel = PixelRGBValue.R;
            int readedBitsInPixelSegment = 0;
            byte encodedByte = 0;
            do
            {
                encodedByte = EncodeByteFromBitmap(bitmap, config, ref row, ref column, ref selectedPixel,ref readedBitsInPixelSegment);
                if (encodedByte != 0)
                {
                    streamWriter.WriteByte(encodedByte);
                }
            } while (encodedByte != 0 && RowAndColumnIsValid(bitmap, row, column));
            streamWriter.Position = 0;
            return streamWriter;
        }

        private bool RowAndColumnIsValid(Bitmap bitmap, int row, int column)
        {
            return (row < bitmap.Height) && (column < bitmap.Width);
        }

        private byte EncodeByteFromBitmap(Bitmap bitmap, BitmapCodeDataConfig config, ref int row, ref int column, ref PixelRGBValue selectedPixel, ref int readedBitsInPixelSegment)
        {
            byte result = 0;
            for (int encodedBitsInByte = 0; encodedBitsInByte < 8 && RowAndColumnIsValid(bitmap, row, column); encodedBitsInByte++)
            {
                bool shouldIncreaseRowAndColumn = false;
                var pixelColor = bitmap.GetPixel(row, column);
                result = EncodeBitDataFromPixelColor(pixelColor, config, result, ref selectedPixel, ref readedBitsInPixelSegment, ref shouldIncreaseRowAndColumn);
                if (shouldIncreaseRowAndColumn)
                {
                    IncreaseRowAndColumn(bitmap, ref row, ref column);
                }
            }
            return result;
        }

        private byte EncodeBitDataFromPixelColor(Color pixelColor, BitmapCodeDataConfig config, byte result, ref PixelRGBValue selectedPixel, ref int readedBitsInPixelSegment, ref bool shouldIncreaseRowAndColumn)
        {
            result = AddBitToResultFromColor(pixelColor, config, selectedPixel, result, ref readedBitsInPixelSegment);
            if (config.GetBitsCount(selectedPixel) == readedBitsInPixelSegment)
            {
                selectedPixel = IncreasePixelRGBValue(selectedPixel, ref shouldIncreaseRowAndColumn);
                readedBitsInPixelSegment = 0;
            }
            return result;
        }

        private byte AddBitToResultFromColor(Color pixelColor, BitmapCodeDataConfig config, PixelRGBValue selectedPixel, byte result, ref int readedBitsInPixelSegment)
        {
            switch (selectedPixel)
            {
                case PixelRGBValue.R:
                    result = AddBitToResult(pixelColor.R, config.R, result, ref readedBitsInPixelSegment);
                    break;
                case PixelRGBValue.G:
                    result = AddBitToResult(pixelColor.G, config.G, result, ref readedBitsInPixelSegment);
                    break;
                case PixelRGBValue.B:
                    result = AddBitToResult(pixelColor.B, config.B, result, ref readedBitsInPixelSegment);
                    break;
                default:
                    throw new ArgumentException("Wrong PixelRGBValue value", "selectedPixel");
            }
            return result;
        }

        private byte AddBitToResult(byte colorValue, int configCount, byte result, ref int readedBitsInPixelSegment)
        {
            int bit = colorValue & (1 << (configCount - 1 - readedBitsInPixelSegment));
            bit = bit >> (configCount - 1 - readedBitsInPixelSegment);
            result = (byte)((result << 1) + bit);
            readedBitsInPixelSegment++;
            return result;
        }
    }
}
