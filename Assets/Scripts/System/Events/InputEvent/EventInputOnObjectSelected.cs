using UnityEngine;
using System.Collections;

/// <summary>
/// Event when input controller select a gameobject in game scene.
/// </summary>
public class EventInputOnObjectSelected : GameEvent 
{

	public GameObject selectedObject;

	public EventInputOnObjectSelected(GameObject theObject)
	{
		selectedObject = theObject;
	}

}
