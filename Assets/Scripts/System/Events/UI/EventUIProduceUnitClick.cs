using UnityEngine;
using System.Collections;

/// <summary>
/// Event UI produce unit click Test only.
/// </summary>
public class EventUIProduceUnitClick : GameEvent 
{

	public CombatUnitType unitType;

	public int produceAmount;

	public EventUIProduceUnitClick(CombatUnitType type, int amount)
	{
		unitType = type;
		produceAmount = amount;
	}
}
