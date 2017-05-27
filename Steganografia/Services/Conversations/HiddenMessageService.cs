using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Steganografia.Services.Conversations
{
	public class HiddenMessageService : IHiddenMessageService
	{
		private const string PATTERN = "{(.+);(.+);(.+)}";
		private const string PATH = "/Content/Emoticons/";
		private const string PATH_FORMAT = PATH + "{0}";
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

					string emoticonPath = string.Format(PATH_FORMAT, _emoticons.ContainsKey(emoticonSign.ToLower()) ? _emoticons[emoticonSign.ToLower()] : _emoticons.First().Value);
					var curent = Directory.GetCurrentDirectory();
					File.Copy("~" + emoticonPath, "~" + string.Format(PATH_FORMAT, "new.png"));
					message = message.Replace(match.Value, $"<img src=\"{string.Format(PATH_FORMAT, "new.png")}\" width=\"20\" height=\"20\">");
				}
			}
			return message;
		}
	}
}