namespace Steganografia.Services.Conversations
{
	public interface IHiddenMessageService
	{
		string FindPaternAndHideInPictureText(string message);
	}
}