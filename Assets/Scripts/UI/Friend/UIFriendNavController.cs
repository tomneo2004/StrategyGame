using UnityEngine;
using System.Collections;

public class UIFriendNavController : UINavigationController 
{
	FriendContentView currentViewToPresent = FriendContentView.FirendList;

	public UISprite newMessageNotifySprite;

	public FriendContentView ViewToPresent
	{
		get
		{
			return currentViewToPresent;
		}
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();

		EventManager.GetInstance ().AddListener<EventPresentFriend> (OnPresentFriend);
		EventManager.GetInstance ().AddListener<EventNewMessage> (OnNewMessage);
	}

	protected override void OnDisable ()
	{
		base.OnDisable ();

		EventManager.GetInstance ().RemoveListener<EventPresentFriend> (OnPresentFriend);
		EventManager.GetInstance ().RemoveListener<EventNewMessage> (OnNewMessage);
	}

	/// <summary>
	/// Handle present friend event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnPresentFriend(EventPresentFriend e)
	{
		currentViewToPresent = e.viewToPresent;

		ShowNavigationController ();
	}

	/// <summary>
	/// Handle new message event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnNewMessage(EventNewMessage e)
	{
		newMessageNotifySprite.gameObject.SetActive (true);
	}
}
