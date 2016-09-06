using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class FriendInfo
{
	public string friendId;

	public string friendName;

	public int friendLevel;

	public FriendInfo(string id, string name, int level)
	{
		friendId = id;
		friendName = name;
		friendLevel = level;
	}
}

/// <summary>
/// This class FriendListMetaData is automatic generated.
/// 
/// Implement your persistant data class here
/// </summary>
[Serializable]
public class FriendListMetaData : PersistantMetaData
{
	List<FriendInfo> _friendList = new List<FriendInfo> ();

	public static FriendListMetaData Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<FriendListMetaData>())
		{
			FriendListMetaData newData = new FriendListMetaData();
			
			newData.Save();
			
			return newData;
		}
		
		FriendListMetaData data = SaveLoadManager.SharedManager.Load<FriendListMetaData> ();	
		
		return data;
	}

	/// <summary>
	/// Adds the friend.
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="name">Name.</param>
	/// <param name="level">Level.</param>
	public void AddFriend(string id, string name, int level)
	{
		for(int i=0; i<_friendList.Count; i++)
		{
			if(_friendList[i].friendId == id)
			{
				return;
			}
		}

		FriendInfo newFriendInfo = new FriendInfo (id, name, level);

		_friendList.Add (newFriendInfo);

		Save ();

		EventManager.GetInstance ().ExecuteEvent<EventNewFriend> (new EventNewFriend ());
	}

	/// <summary>
	/// Gets all friends.
	/// </summary>
	/// <returns>The all friends.</returns>
	/// <param name="reverse">If set to <c>true</c> reverse.</param>
	public FriendInfo[] GetAllFriends(bool reverse = false)
	{
		if(reverse)
		{
			_friendList.Reverse();

			FriendInfo[] retVal = _friendList.ToArray();

			_friendList.Reverse();

			return retVal;
		}

		return _friendList.ToArray ();
	}

	/// <summary>
	/// Removes the friend.
	/// </summary>
	/// <param name="friendId">Friend identifier.</param>
	public void RemoveFriend(string friendId)
	{
		for(int i=0; i<_friendList.Count; i++)
		{
			if(_friendList[i].friendId == friendId)
			{
				_friendList.RemoveAt(i);

				Save();

				return;
			}
		}

		Debug.LogError ("No friend to be removed " + friendId);
	}

	public void Save()
	{
		SaveLoadManager.SharedManager.Save <FriendListMetaData>(this);
	}
}