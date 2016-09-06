using UnityEngine;
using System.Collections;

public class UIProfileContent : UINavigationContent 
{
	public void ChangePass()
	{
		EventManager.GetInstance().ExecuteEvent<EventAlert>(new EventAlert("Warning", "Password is not correct!!!"));
	}
}
