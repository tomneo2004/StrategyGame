using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Event when produce combat unit.
/// </summary>
public class EventProduceCombatUnit : GameEvent 
{
	private List<ResourceType> costType;


	public EventProduceCombatUnit(List<ResourceType> cost)
	{
		costType = cost;
	}

	public bool ContainResourceType(ResourceType comparedType)
	{
		for(int i=0; i<costType.Count; i++)
		{
			if(comparedType == costType[i])
			{
				return true;
			}
		}

		return false;
	}
}
