using UnityEngine;
using System.Collections;

/// <summary>
/// Event collect resource.
/// Event get fired when building's resource ready to be collected
/// </summary>
public class EventCollectResource : GameEvent 
{
	ResourceType _resourceId;

	public ResourceType GetResourceId
	{
		get
		{
			return _resourceId;
		}
	}

	Transform _target;
		
	public Transform GetTarget
	{
		get
		{
			return _target;
		}
	}

	float _resource = 0f;

	public float GetResource
	{
		get
		{
			return _resource;
		}
	}


	public EventCollectResource(Transform target, ResourceType resourceId, float resource)
	{
		_target = target;
		_resourceId = resourceId;
		_resource = resource;
	}
}
