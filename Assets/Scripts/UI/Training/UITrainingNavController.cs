using UnityEngine;
using System.Collections;

public class UITrainingNavController : UINavigationController 
{
	[HideInInspector]
	public CombatUnitManager combatUnitManager;

	protected override void  OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventPresentUnitTraining> (ShowNavController);
	}
	
	protected override void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventPresentUnitTraining> (ShowNavController);
	}

	void ShowNavController(EventPresentUnitTraining e)
	{
		combatUnitManager = e.combatUnitManager;

		ShowNavigationController ();
	}

}
