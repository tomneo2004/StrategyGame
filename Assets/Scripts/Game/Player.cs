using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	static Player _instance;

	public static Player Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<Player>();

				if(_instance == null)
				{
					Debug.LogError("There is no Player object in scene with Player script");
				}
			}

			return _instance;
		}
	}

	private Dictionary<ResourceType, ResourceStorage> _resourceStorage;

	void Awake()
	{
		InitResourceStorage ();
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void InitResourceStorage()
	{
		ResourceStorage[] rs = GetComponents<ResourceStorage> ();
		
		if((rs!=null) && (rs.Length > 0))
		{
			_resourceStorage = new Dictionary<ResourceType, ResourceStorage>();
			
			for(int i=0; i<rs.Length; i++)
			{
				if(_resourceStorage.ContainsKey(rs[i].resourceId))
				{
					_resourceStorage[rs[i].resourceId] = rs[i];
				}
				else
				{
					_resourceStorage.Add(rs[i].resourceId, rs[i]);
				}
			}
		}
		else
		{
			Debug.LogError(gameObject.name+" has no ResourceStorage attached, must at least one");
		}
	}

	public ResourceStorage GetResourceStorage(ResourceType resourceType)
	{
		if(_resourceStorage != null)
		{
			return _resourceStorage[resourceType];
		}

		return null;
	}

	public bool IsResourceEnough(ResourceType resourceType, float resourceToSpend)
	{
		if(resourceToSpend <= _resourceStorage[resourceType].currentResource)
		{
			return true;
		}

		return false;
	}
}
