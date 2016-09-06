using UnityEngine;
using System.Collections;

public class UISendMessageContent : UINavigationContent 
{

	public UIInput recipientIdInput;

	public UIInput titleInput;

	public UIInput contentInput;


	public override void OnContentBeginDisplay()
	{
		base.OnContentBeginDisplay ();

		UISendMessageNavController controller = (UISendMessageNavController)_navigationController;

		recipientIdInput.value = controller.RecipientId;
		titleInput.value = controller.PredefineTitle;
		contentInput.value = controller.PredefineContent;
	}

	/// <summary>
	/// Handle send message event.
	/// </summary>
	public void OnSendMessage()
	{
		if(ValidateMessage())
		{
			InboxMetaData data = InboxMetaData.Load();

			data.AddInboxMessage("Player", recipientIdInput.value, titleInput.value, contentInput.value);

			EventManager.GetInstance().ExecuteEvent<EventNewMessage>(new EventNewMessage());

			recipientIdInput.value = "";
			titleInput.value = "";
			contentInput.value = "";
			
			_navigationController.CloseNavigationController ();
		}

	}

	/// <summary>
	/// Validates the message.
	/// </summary>
	/// <returns><c>true</c>, if message was validated, <c>false</c> otherwise.</returns>
	bool ValidateMessage()
	{
		if(string.IsNullOrEmpty(recipientIdInput.value))
		{
			EventManager.GetInstance().ExecuteEvent<EventAlert>(new EventAlert("Error", "Please enter recipient id"));

			return false;
		}

		if(string.IsNullOrEmpty(titleInput.value))
		{
			EventManager.GetInstance().ExecuteEvent<EventAlert>(new EventAlert("Error", "Please enter title"));

			return false;
		}

		return true;
	}
}
