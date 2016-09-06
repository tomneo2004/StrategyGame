using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class InboxMessage
{
	public string messageId;

	public string senderId;

	public string recipientId;

	public string title;

	public string content;

	public bool isRead = false;

	public InboxMessage(string msgId, string theSenderId, string receiverId, string msgTitle, string msgContent)
	{
		messageId = msgId;
		senderId = theSenderId;
		recipientId = receiverId;

		title = msgTitle;

		content = msgContent;
	}
}

/// <summary>
/// This class InboxMetaData is automatic generated.
/// 
/// Implement your persistant data calss here
/// </summary>
[Serializable]
public class InboxMetaData : PersistantMetaData
{
	List<InboxMessage> _allInboxMessages = new List<InboxMessage>();

	public static InboxMetaData Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<InboxMetaData>())
		{
			InboxMetaData newData = new InboxMetaData();
			
			newData.Save();
			
			return newData;
		}
		
		InboxMetaData data = SaveLoadManager.SharedManager.Load<InboxMetaData> ();	
		
		return data;
	}

	string GenerateNewId()
	{
		while(true)
		{
			bool isVaild = true;

			string newId = Guid.NewGuid().ToString();

			for(int i=0; i<_allInboxMessages.Count; i++)
			{
				if(newId == _allInboxMessages[i].messageId)
				{
					isVaild = false;

					break;
				}
			}

			if(isVaild)
			{
				return newId;
			}
		}
	}

	/// <summary>
	/// Adds the inbox message.
	/// </summary>
	/// <param name="senderId">Sender identifier.</param>
	/// <param name="receiverId">Receiver identifier.</param>
	/// <param name="title">Title.</param>
	/// <param name="content">Content.</param>
	public void AddInboxMessage(string senderId, string receiverId, string title, string content)
	{
		InboxMessage newMsg = new InboxMessage (GenerateNewId (), senderId, receiverId, title, content);

		_allInboxMessages.Add (newMsg);

		Save ();
	}

	/// <summary>
	/// Marks inbox message as read.
	/// </summary>
	/// <param name="messageId">Message identifier.</param>
	public void MarkRead(string messageId)
	{
		for(int i=0; i<_allInboxMessages.Count; i++)
		{
			if(messageId == _allInboxMessages[i].messageId)
			{
				_allInboxMessages[i].isRead = true;

				Save();

				break;
			}
		}
	}

	/// <summary>
	/// Gets all inbox messages.
	/// </summary>
	/// <returns>The all messages.</returns>
	/// <param name="reverse">If set to <c>true</c> reverse.</param>
	public InboxMessage[] GetAllMessages(bool reverse = true)
	{
		if(reverse)
		{
			_allInboxMessages.Reverse();

			InboxMessage[] retVal = _allInboxMessages.ToArray();

			_allInboxMessages.Reverse();

			return retVal;
		}

		return _allInboxMessages.ToArray ();
	}

	public void Save()
	{
		SaveLoadManager.SharedManager.Save <InboxMetaData>(this);
	}
}