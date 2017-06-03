using System.IO;

namespace Steganografia.Services.Conversations
{
	public interface IHiddenMessageService
	{
		string FindPaternAndHideInPictureText(string message);
		string DecryptFromEmoticon(Stream inputStream, string password);
	}
}