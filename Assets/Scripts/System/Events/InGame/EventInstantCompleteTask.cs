using UnityEngine;
using System.Collections;

public class EventInstantCompleteTask : GameEvent 
{

	public Transform target;

	public EventInstantCompleteTask(Transform theTarget)
	{
		target = theTarget;
	}
}
