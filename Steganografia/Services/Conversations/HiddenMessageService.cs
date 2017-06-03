using Steganografia.Services.BitmapCoders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace Steganografia.Services.Conversations
{
	public class HiddenMessageService : IHiddenMessageService
	{
		private const string PATTERN = "{(.+);(.+);(.+)}";
		private const string PATH = "/Content/Emoticons/";
		private const string PATH_FORMAT = PATH + "{0}";

		private readonly ImageDataCoderService _coderService;
		private readonly ImageDataEncoderService _encoderService;

		private readonly IDictionary<string, string> _emoticons = new Dictionary<string, string>
		{
			{":)", "facebook-smiley-face.png"},
			{"<3", "facebook-heart-emoticon.png"},
			{":(", "frowny-face.png"},
			{":D", "grinning-facebook-emoticon.png"},
			{":p","grinning-facebook-emoticon.png"}
		};
		public HiddenMessageService()
		{
			_coderService = new ImageDataCoderService();
			_encoderService = new ImageDataEncoderService();
		}

		public string FindPaternAndHideInPictureText(string message)
		{
			Regex rx = new Regex(PATTERN);
			var matches = rx.Matches(message);
			foreach (Match match in matches)
			{
				var values = match.Value.TrimStart('{').TrimEnd('}').Split(';');
				if (values.Count() == 3)
				{
					var messageToEncrypt = values[0];
					var password = values[1];
					var emoticonSign = values[2];
					var path = HostingEnvironment.MapPath("~" + PATH);
					string emoticonPath = string.Format("{0}{1}", path, _emoticons.ContainsKey(emoticonSign.ToLower()) ? _emoticons[emoticonSign.ToLower()] : _emoticons.First().Value);
					var result = _coderService.CodeDataToImage((Bitmap)Image.FromFile(emoticonPath), GenerateStreamFromString(Encrypt(messageToEncrypt, password)), new BitmapCodeDataConfig(1, 1, 1));

					string newEmotName = Guid.NewGuid().ToString() + ".png";
					result.Save(string.Format("{0}{1}", path, newEmotName));
					message = message.Replace(match.Value, $"<img src=\"{string.Format(PATH_FORMAT, newEmotName)}\" width=\"20\" height=\"20\">");
				}
			}
			return message;
		}

		private static Stream GenerateStreamFromString(string s)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		public string DecryptFromEmoticon(Stream inputStream, string password)
		{
			var emoticon = (Bitmap)Image.FromStream(inputStream);
			var result = _encoderService.EncodeDataFromImage(emoticon, new BitmapCodeDataConfig(1, 1, 1));
			StreamReader reader = new StreamReader(result);
			string encodedMessage = "Złe hasło";
			try
			{
				encodedMessage = Decrypt(reader.ReadToEnd(), password);
			}
			catch (Exception)
			{

			}
			return encodedMessage;
		}

		private const string initVector = "tu89geji340t89u2";

		private const int keysize = 256;

		public static string Encrypt(string Text, string Key)
		{
			byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(Text);
			PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
			byte[] keyBytes = password.GetBytes(keysize / 8);
			RijndaelManaged symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
			cryptoStream.FlushFinalBlock();
			byte[] Encrypted = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(Encrypted);
		}

		public static string Decrypt(string EncryptedText, string Key)
		{
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] DeEncryptedText = Convert.FromBase64String(EncryptedText);
			PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
			byte[] keyBytes = password.GetBytes(keysize / 8);
			RijndaelManaged symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
			MemoryStream memoryStream = new MemoryStream(DeEncryptedText);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			byte[] plainTextBytes = new byte[DeEncryptedText.Length];
			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
		}
	}
}