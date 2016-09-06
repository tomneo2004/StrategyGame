using UnityEngine;
using System.Collections;

/// <summary>
/// Event when player issue a new attack.
/// </summary>
public class EventNewAttack : GameEvent 
{

	public AttackTask newAttackTask;

	public EventNewAttack(AttackTask task)
	{
		newAttackTask = task;
	}
}
