using UnityEngine;
using System.Collections;

/// <summary>
/// Event when barrack building selected.
/// </summary>
public class EventBarrackSelected : GameEvent 
{

	public CombatUnitManager manager;

	public EventBarrackSelected(CombatUnitManager mgr)
	{
		manager = mgr;
	}
}
