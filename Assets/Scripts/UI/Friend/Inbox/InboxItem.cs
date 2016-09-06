using UnityEngine;
using System.Collections;

public class InboxItem : MonoBehaviour 
{
	public UILabel titleLabel;

	public UILabel messageLabel;

	public UIScrollView messageScrollview;

	public UILabel senderLabel;

	public UISprite baseSprite;

	public UISprite notifySprite;

	public Color unReadColor = new Color(224f/255f, 50f/255f, 83f/255f);

	string messageId;

	bool isShowMessage = false;

	bool isRead = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	//call this on tween animaiton finish
	public void OnExpand()
	{
		if(!isShowMessage)
		{
			isShowMessage = !isShowMessage;

			messageScrollview.gameObject.SetActive(true);

			messageScrollview.ResetPosition();
		}
	}

	/// <summary>
	/// Handle unexpand event.
	/// </summary>
	public void OnUnexpand()
	{
		if(isShowMessage)
		{
			isShowMessage = !isShowMessage;
			
			messageScrollview.gameObject.SetActive(false);
		}

		if(!isRead)
		{
			InboxMetaData data = InboxMetaData.Load();
			
			data.MarkRead(messageId);
			
			notifySprite.enabled = false;
			
			isRead = true;
		}
	}

	/// <summary>
	/// Sets the inbox item info.
	/// </summary>
	/// <param name="msg">Message.</param>
	public void SetInboxItemInfo(InboxMessage msg)
	{
		messageId = msg.messageId;
		titleLabel.text = msg.title;
		messageLabel.text = msg.content;
		senderLabel.text = "From "+msg.senderId;
		isRead = msg.isRead;

		if(!msg.isRead)
		{
			baseSprite.color = unReadColor;

			notifySprite.enabled = true;
		}
		else
		{
			notifySprite.enabled = false;
		}

	}
}
