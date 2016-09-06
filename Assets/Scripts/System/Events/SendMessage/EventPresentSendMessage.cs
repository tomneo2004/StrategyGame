using UnityEngine;
using System.Collections;

/// <summary>
/// Event when present send message view.
/// </summary>
public class EventPresentSendMessage : GameEvent 
{

	public string recipientId;

	public string title;

	public string content;

	public EventPresentSendMessage(string receiverId, string preTitle = "", string preContent = "")
	{
		recipientId = receiverId;
		title = preTitle;
		content = preContent;
	}
}
