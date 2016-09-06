using UnityEngine;
using System.Collections;

/// <summary>
/// Event resource transfer.
/// Event get fired when building collected resource transfer to player resource
/// </summary>
public class EventResourceTransferToPlayer : GameEvent 
{
	public ResourceType resourceId;

	public EventResourceTransferToPlayer(ResourceType _resourceId)
	{

		resourceId = _resourceId;

	}
}
