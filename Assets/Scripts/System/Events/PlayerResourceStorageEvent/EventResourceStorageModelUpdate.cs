using UnityEngine;
using System.Collections;

/// <summary>
/// Event when player resource storage meta data update.
/// </summary>
public class EventResourceStorageModelUpdate : GameEvent 
{

	public ResourceType resourceType;
	public float currentResource = 0f;

	public EventResourceStorageModelUpdate(ResourceType type, float curResource)
	{
		resourceType = type;
		currentResource = curResource;
	}
}
