using UnityEngine;
using System.Collections;

public class UIArchitectureOnTask : UIArchitectureMenu 
{

	public void OnCancelTask()
	{

	}

	public void OnInstantFinishTask()
	{
		EventManager.GetInstance ().ExecuteEvent<EventInstantCompleteTask> (new EventInstantCompleteTask (_target));
	}
}
