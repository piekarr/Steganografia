using Steganografia.Services.BitmapCoders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
					var encryptionKey = values[1];
					var emoticonSign = values[2];
					var path = HostingEnvironment.MapPath("~" + PATH);
					string emoticonPath = string.Format("{0}{1}", path, _emoticons.ContainsKey(emoticonSign.ToLower()) ? _emoticons[emoticonSign.ToLower()] : _emoticons.First().Value);
					var result = _coderService.CodeDataToImage((Bitmap)Image.FromFile(emoticonPath), GenerateStreamFromString(messageToEncrypt), new BitmapCodeDataConfig(1, 1, 1));

					string newEmotName = Guid.NewGuid().ToString()+ ".png";
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
	}
}