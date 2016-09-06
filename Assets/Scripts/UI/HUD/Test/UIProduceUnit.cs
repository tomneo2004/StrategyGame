using UnityEngine;
using System.Collections;

public class UIProduceUnit : MonoBehaviour 
{
	public int produceAmount = 10;

	public void ProduceWarriorUnit()
	{
		EventManager.GetInstance ().ExecuteEvent<EventUIProduceUnitClick> (new EventUIProduceUnitClick (CombatUnitType.Warrior, produceAmount));
	}

	public void ProduceArcherUnit()
	{
		EventManager.GetInstance ().ExecuteEvent<EventUIProduceUnitClick> (new EventUIProduceUnitClick (CombatUnitType.Archer, produceAmount));
	}
}
