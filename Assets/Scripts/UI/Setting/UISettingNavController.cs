using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UISettingNavController : UINavigationController 
{
	protected override void  OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventUISettingButtonClick> (ShowNavController);
	}
	
	protected override void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventUISettingButtonClick> (ShowNavController);
	}

	/// <summary>
	/// Shows the nav controller.
	/// </summary>
	/// <param name="e">E.</param>
	void ShowNavController(EventUISettingButtonClick e)
	{
		ShowNavigationController ();
	}
}
