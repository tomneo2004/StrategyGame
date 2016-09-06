using UnityEngine;
using System.Collections;

public class InviteFriendController : MonoBehaviour 
{
	public UIInput friendIdInput;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Handle add friend event.
	/// </summary>
	public void OnAddFriend()
	{
		if(!string.IsNullOrEmpty(friendIdInput.value))
		{
			FriendListMetaData data = FriendListMetaData.Load();

			data.AddFriend(friendIdInput.value, "Fake friend", Random.Range(1, 10));

			friendIdInput.value = "";
		}
	}
}
