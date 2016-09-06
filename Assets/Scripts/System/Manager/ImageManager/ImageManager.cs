using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ImageManager : MonoBehaviour 
{
	static ImageManager _instance;

	public static ImageManager Instance
	{
		get
		{
			return _instance;
		}
	}

	//Combat unit image
	[HideInInspector]public List<CombatUnitType> combatUnitType = new List<CombatUnitType>();
	[HideInInspector]public List<string> combatUnitHeadSpriteName = new List<string>();
	[HideInInspector]public List<string> combatUnitFullBodySpriteName = new List<string>();
	[HideInInspector]public List<UIAtlas> combatUnitHeadAtlas = new List<UIAtlas>();
	[HideInInspector]public List<UIAtlas> combatunitFullBodyAtlas = new List<UIAtlas>();

	//Resource image
	[HideInInspector]public List<ResourceType> resourceType = new List<ResourceType>();
	[HideInInspector]public List<string> resourceSpriteName = new List<string>();
	[HideInInspector]public List<UIAtlas> resourceAtlas = new List<UIAtlas>();


	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public string CombatUnitSpriteNameForHead(CombatUnitType type)
	{
		if(combatUnitType.Contains(type))
		{
			for(int i=0; i<combatUnitType.Count; i++)
			{
				if(combatUnitType[i] == type)
				{
					return combatUnitHeadSpriteName[combatUnitType.IndexOf(type)];
				}
			}

			return "";
		}

		return "";
	}

	public string CombatUnitSpriteNameForFullBody(CombatUnitType type)
	{
		if(combatUnitType.Contains(type))
		{
			for(int i=0; i<combatUnitType.Count; i++)
			{
				if(combatUnitType[i] == type)
				{
					return combatUnitFullBodySpriteName[combatUnitType.IndexOf(type)];
				}
			}

			return "";
		}

		return "";
	}

	public string ResourceSpriteName(ResourceType type)
	{
		if(resourceType.Contains(type))
		{
			for(int i=0; i<resourceType.Count; i++)
			{
				if(resourceType[i] == type)
				{
					return resourceSpriteName[resourceType.IndexOf(type)];
				}
			}

			return "";
		}

		return "";
	}
}
