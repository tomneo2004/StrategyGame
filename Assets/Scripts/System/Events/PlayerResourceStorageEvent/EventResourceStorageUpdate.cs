using UnityEngine;
using System.Collections;

/// <summary>
/// Event when player resource storage update.
/// </summary>
public class EventResourceStorageUpdate : GameEvent 
{
	public ResourceType resourceId;
	public float currentResource = 0f;

	public EventResourceStorageUpdate(ResourceType type, float curRes)
	{
		resourceId = type;
		currentResource = curRes;
	}
}
