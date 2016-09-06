using UnityEngine;
using System.Collections;

public class UIAttackNavController : UINavigationController 
{
	[HideInInspector]
	public AttackManager attackManager;
	
	protected override void  OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventPresentAttack> (ShowNavController);
	}
	
	protected override void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventPresentAttack> (ShowNavController);
	}

	/// <summary>
	/// Handle show nav controller.
	/// </summary>
	/// <param name="e">E.</param>
	void ShowNavController(EventPresentAttack e)
	{
		attackManager = e.attackManager;
		
		ShowNavigationController ();
	}

}
