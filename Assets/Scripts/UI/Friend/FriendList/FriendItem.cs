using UnityEngine;
using System.Collections;

public class FriendItem : MonoBehaviour 
{
	public UISprite avatarSprite;

	public UILabel levelLabel;

	public UILabel nameLabel;

	string friendId;

	public delegate void OnFriendItemDelete();
	public OnFriendItemDelete Evt_OnFriendItemDelete;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Handle send message event.
	/// </summary>
	public void OnSendMessage()
	{
		EventManager.GetInstance ().ExecuteEvent<EventPresentSendMessage> (new EventPresentSendMessage (friendId));
	}

	/// <summary>
	/// Handle delete event.
	/// </summary>
	public void OnDelete()
	{
		EventManager.GetInstance ().ExecuteEvent<EventPresentActionConfirm> (new EventPresentActionConfirm ("Warning", "Delete friend " + nameLabel.text, "Ok", "Cancel", OnActionConfirm));
	}

	/// <summary>
	/// Handle action confirm event.
	/// </summary>
	/// <param name="buttonIndex">Button index.</param>
	void OnActionConfirm(int buttonIndex)
	{
		if(buttonIndex == 0)
		{
			FriendListMetaData data = FriendListMetaData.Load();
			data.RemoveFriend(friendId);

			transform.parent = null;

			NGUITools.Destroy(gameObject);

			if(Evt_OnFriendItemDelete != null)
			{
				Evt_OnFriendItemDelete();
			}
		}
	}

	/// <summary>
	/// Sets the friend info.
	/// </summary>
	/// <param name="info">Info.</param>
	public void SetFriendInfo(FriendInfo info)
	{
		friendId = info.friendId;

		levelLabel.text = info.friendLevel.ToString ();
		nameLabel.text = info.friendName.ToString ();

	}
	
}
