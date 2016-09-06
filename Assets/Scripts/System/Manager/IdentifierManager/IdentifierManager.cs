using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IdentifierManager
{
	static IdentifierManager _instance;

	public static IdentifierManager Instance
	{
		get
		{
			if(_instance != null)
			{
				return _instance;
			}
			else
			{
				_instance = new IdentifierManager();

				return _instance;
			}
		}
	}

	Dictionary<int, GameObject> _idReferences;

	public bool RegisterId(int id, GameObject obj)
	{
		if(_idReferences == null)
		{
			_idReferences = new Dictionary<int, GameObject>();
		}

		if(!_idReferences.ContainsKey(id))
		{
			_idReferences.Add(id, obj);

			return true;
		}
		else
		{
			Debug.LogError("The id:"+id+" already exist in IdentifierManager");

			return false;
		}
	}

	public void RemoveId(int id)
	{
		if(_idReferences != null)
		{
			if(_idReferences.ContainsKey(id))
			{
				_idReferences.Remove(id);
			}
		}
	}

	public GameObject FindGameObjectById(int id)
	{
		if(_idReferences != null)
		{
			if(_idReferences.ContainsKey(id))
			{
				return _idReferences[id];
			}
		}

		return null;
	}
}
