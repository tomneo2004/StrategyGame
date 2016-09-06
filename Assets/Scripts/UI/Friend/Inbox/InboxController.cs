using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InboxController : MonoBehaviour 
{
	public GameObject inboxItemPrefab;

	public UIScrollView scrollview;

	public UITable table;

	public UIContainer container;

	// Use this for initialization
	void Start () 
	{
	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventNewMessage> (UpdateInbox);

		UpdateInbox ();
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventNewMessage> (UpdateInbox);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Updates the inbox.
	/// </summary>
	void UpdateInbox()
	{
		if(table.transform.childCount > 0)
		{
			List<Transform> childs = new List<Transform>();
			
			for(int i=0; i<table.transform.childCount; i++)
			{
				childs.Add(table.transform.GetChild(i));
			}
			
			for(int i=0; i<childs.Count; i++)
			{
				childs[i].parent = null;
				
				NGUITools.Destroy(childs[i].gameObject);
			}
		}
		
		InboxMetaData data = InboxMetaData.Load ();
		
		InboxMessage[] messages = data.GetAllMessages ();
		
		for(int i=0; i<messages.Length; i++)
		{
			GameObject newMessage = NGUITools.AddChild(table.gameObject, inboxItemPrefab);
			
			InboxItem display = newMessage.GetComponent<InboxItem>();
			
			display.SetInboxItemInfo(messages[i]);
		}
		
		table.Reposition ();
		
		scrollview.ResetPosition ();
	}
	/// <summary>
	/// Updates the inbox.
	/// </summary>
	/// <param name="e">E.</param>
	void UpdateInbox(EventNewMessage e)
	{
		UpdateInbox ();
	}

	/// <summary>
	/// Handle container active event.
	/// </summary>
	/// <param name="container">Container.</param>
	void OnContainerActive(UIContainer container)
	{
		UpdateInbox ();
	}
}
