using UnityEngine;
using System.Collections;

public class EventProduceCombatUnitComplete : GameEvent 
{

	public CombatUnitManager mgr;

	public EventProduceCombatUnitComplete(CombatUnitManager manager)
	{
		mgr = manager;
	}
}
