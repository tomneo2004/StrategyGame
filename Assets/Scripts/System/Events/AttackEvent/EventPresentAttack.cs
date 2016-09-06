using UnityEngine;
using System.Collections;

/// <summary>
/// Event when present attack management view.
/// </summary>
public class EventPresentAttack : GameEvent 
{
	public AttackManager attackManager;

	public EventPresentAttack(AttackManager mgr)
	{
		attackManager = mgr;
	}
}
