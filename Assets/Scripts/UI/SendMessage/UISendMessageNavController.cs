using UnityEngine;
using System.Collections;

public class UISendMessageNavController : UINavigationController 
{
	string recipientId = "";

	public string RecipientId
	{
		get
		{
			return recipientId;
		}
	}

	string predefineTitle = "";

	public string PredefineTitle
	{
		get
		{
			return predefineTitle;
		}
	}

	string predefineContent = "";

	public string PredefineContent
	{
		get
		{
			return predefineContent;
		}
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		
		EventManager.GetInstance ().AddListener<EventPresentSendMessage> (OnPresentSendMessage);
	}
	
	protected override void OnDisable ()
	{
		base.OnDisable ();
		
		EventManager.GetInstance ().RemoveListener<EventPresentSendMessage> (OnPresentSendMessage);
	}

	/// <summary>
	/// Handle present send message event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnPresentSendMessage(EventPresentSendMessage e)
	{
		recipientId = e.recipientId;
		predefineTitle = e.title;
		predefineContent = e.content;

		ShowNavigationController ();
	}

}
