using UnityEngine;
using System.Collections;

/// <summary>
/// Event when present unit training view.
/// </summary>
public class EventPresentUnitTraining : GameEvent 
{

	public CombatUnitManager combatUnitManager;

	public EventPresentUnitTraining(CombatUnitManager mgr)
	{
		combatUnitManager = mgr;
	}
}
