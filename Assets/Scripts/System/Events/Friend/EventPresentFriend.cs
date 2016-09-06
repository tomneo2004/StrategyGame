using UnityEngine;
using System.Collections;

/// <summary>
/// Event when present friend list view.
/// </summary>
public class EventPresentFriend : GameEvent 
{
	//which sub view should be present at first
	public FriendContentView viewToPresent = FriendContentView.FirendList;

	public EventPresentFriend(FriendContentView viewToShow = FriendContentView.FirendList)
	{
		viewToPresent = viewToShow;
	}
}
