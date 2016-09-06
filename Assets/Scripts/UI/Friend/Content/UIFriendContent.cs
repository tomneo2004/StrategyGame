using UnityEngine;
using System.Collections;

public enum FriendContentView
{
	FirendList,
	InviteFriend,
	Inbox
}

public class UIFriendContent : UINavigationContent 
{

	public UIFriendGroupButton[] groupButtons;

	public override void OnContentBeginDisplay ()
	{
		base.OnContentBeginDisplay ();

		for(int i=0; i<groupButtons.Length; i++)
		{
			if(groupButtons[i].viewType == ((UIFriendNavController)_navigationController).ViewToPresent)
			{
				groupButtons[i].GetComponent<UIToggle>().Set(true);

			}
			else
			{
				groupButtons[i].GetComponent<UIToggle>().Set(false);
			}
		}
	}
}
