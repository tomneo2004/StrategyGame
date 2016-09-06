using UnityEngine;
using System.Collections;

public class UIBarrackArchitectureNormal : UIArchitectureMenu 
{

	public void OnUpgradeClick()
	{

	}

	/// <summary>
	/// Handle training unit click event.
	/// </summary>
	public void OnTrainingUnitClick()
	{
		CombatUnitManager mgr = _target.GetComponent<CombatUnitManager> ();

		if(mgr != null)
		{
			EventManager.GetInstance().ExecuteEvent<EventPresentUnitTraining>(new EventPresentUnitTraining(mgr));
		}
		else
		{
			Debug.LogError(_target.name+" has no CombatUnitManager attached");
		}
	}
}
