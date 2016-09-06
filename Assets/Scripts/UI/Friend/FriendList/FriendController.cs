using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendController : MonoBehaviour 
{
	public GameObject friendItemPrefab;

	public UIScrollView scrollview;

	public UIGrid grid;

	public UIContainer container;

	// Use this for initialization
	void Start () 
	{

	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventNewFriend> (OnNewFriend);

		UpdateFriendList ();
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventNewFriend> (OnNewFriend);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Handle new friend event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnNewFriend(EventNewFriend e)
	{
		UpdateFriendList ();
	}

	/// <summary>
	/// Updates the friend list.
	/// </summary>
	void UpdateFriendList()
	{
		if(grid.transform.childCount > 0)
		{
			List<Transform> childs = new List<Transform>();
			
			for(int i=0; i<grid.transform.childCount; i++)
			{
				childs.Add(grid.transform.GetChild(i));
			}
			
			for(int i=0; i<childs.Count; i++)
			{
				childs[i].parent = null;
				
				NGUITools.Destroy(childs[i].gameObject);
			}
		}

		FriendListMetaData data = FriendListMetaData.Load ();

		FriendInfo[] infos = data.GetAllFriends ();

		for(int i=0; i<infos.Length; i++)
		{
			GameObject newFriend = NGUITools.AddChild(grid.gameObject, friendItemPrefab);
			
			FriendItem display = newFriend.GetComponent<FriendItem>();

			display.Evt_OnFriendItemDelete = OnFriendItemDestroy;
			
			display.SetFriendInfo(infos[i]);
		}
		
		grid.Reposition ();
		
		scrollview.ResetPosition ();
	}

	/// <summary>
	/// Handle container active event.
	/// </summary>
	/// <param name="container">Container.</param>
	void OnContainerActive(UIContainer container)
	{
		UpdateFriendList ();
	}

	/// <summary>
	/// Handle friend item destroy event.
	/// </summary>
	void OnFriendItemDestroy()
	{
		grid.Reposition ();

		scrollview.ResetPosition ();
	}
}
