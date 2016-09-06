using UnityEngine;
using System.Collections;

public class FoodResourceGenerator : MonoBehaviour, IArchitecture 
{

	/// <summary>
	/// The resource identifier.
	/// </summary>
	[Tooltip("Use to identify resource")]
	public ResourceType resourceId = ResourceType.Food;
	
	/// <summary>
	/// How long should it be to collect.
	/// Base on second
	/// </summary>
	[Tooltip("How long can resource be collected base on second")]
	public int collectPerDuration = 1;
	
	/// <summary>
	/// The resource regen per second.
	/// </summary>
	[Tooltip("How much resource will generate per duration")]
	public float resourceRegenPerDuration = 10f;
	
	
	/// <summary>
	/// The max resource store.
	/// </summary>
	[Tooltip("Max resource this building can store")]
	public float maxResourceStore = 1000f;
	
	/// <summary>
	/// The _current resource store.
	/// </summary>
	[Tooltip("Debug")]
	public float _currentResourceStore = 0f;
	
	/// <summary>
	/// The temp resource store.
	/// </summary>
	//[Tooltip("Debug")]
	//public float _tempResourceStore = 0f;
	
	/// <summary>
	/// The resource generate task.
	/// </summary>
	private ResourceGenerateTask _task = null;
	
	
	
	// Use this for initialization
	void Start () 
	{
		DoFetchData ();
		/*
		EventManager.GetInstance ().ExecuteEvent<EventResourceTransfer> (new EventResourceTransfer (0f, maxResourceStore, resourceId));


		if(_currentResourceStore < maxResourceStore)
		{
			AddResourceRegenTask();
		}
		*/
		
	}
	
	//pull data from fake server
	void DoFetchData()
	{
		Debug.Log("Fetch data");
		FarmlandBuildingMetaData data = FarmlandBuildingMetaData.Load (true);
		
		resourceId = data.resourceId;
		collectPerDuration = data.collectPerDuration;
		resourceRegenPerDuration = data.resourceRegenPerDuration;
		maxResourceStore = data.maxResourceStore;
		_currentResourceStore = data.currentResourceStore;
		
		Debug.Log ("current resource:" + data.currentResourceStore);
		
		if((data.hasTask) && (_task == null))
		{
			Debug.Log("Resume task");
			
			_task = FoodResourceGenerateTask.CreateTask<FoodResourceGenerateTask>(data.resourceId, data.resourceRegenPerDuration, OnResourceTaskComplete, OnTimeLeftToComplete, data.taskDuration);
			
			TaskManager.Instance.AddTask (_task);
		}
		else if(_currentResourceStore < maxResourceStore)
		{
			Debug.Log("Add task");
			AddResourceRegenTask();
		}
		else
		{
			_task = null;
		}
		
		if(_currentResourceStore > 0)
		{
			EventManager.GetInstance ().ExecuteEvent<EventCollectResource> (new EventCollectResource (transform, resourceId, _currentResourceStore));
		}
	}
	
	//notify fake server to start a new task
	bool DoStartRegenTask()
	{
		FarmlandBuildingMetaData data = FarmlandBuildingMetaData.Load ();
		
		return data.StartRegenTask ();
	}
	
	//notify fake server to transfer resource from temp to player resource
	bool DoTransferResource()
	{
		FarmlandBuildingMetaData data = FarmlandBuildingMetaData.Load ();
		
		return data.TransferResourceToPlayer();
	}

	/*
	//ask fake server if task is complete
	bool IsTaskComplete()
	{
		FarmlandBuildingMetaData data = FarmlandBuildingMetaData.Load ();
		
		return data.IsTaskComplete ();
	}
	*/
	/*
	//time not sync with fake server's time, keep asking for a task is complete or not
	IEnumerator CompleteTask()
	{
		while(!IsTaskComplete())
		{
			Debug.Log("Wait for server task complete");
			
			yield return new WaitForSeconds(1f);
		}
		
		TaskManager.Instance.RemoveTask (_task);
		
		DoFetchData ();
		
		/*
		if(_tempResourceStore > 0)
		{
			EventManager.GetInstance ().ExecuteEvent<EventCollectResource> (new EventCollectResource (transform, _tempResourceStore));
		}
		*/
		
	//}
	
	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventUIResoucePopupClick> (OnUIResourcePopupClick);
	}
	
	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventUIResoucePopupClick> (OnUIResourcePopupClick);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void AddResourceRegenTask()
	{
		/*
		if(_task != null)
		{
			_task.InitTask(resourceId, resourceRegenPerDuration, OnResourceTaskComplete, OnTimeLeftToComplete, collectPerDuration);
		}
		else
		{
			_task = ResourceGenerateTask.CreateTask(resourceId, resourceRegenPerDuration, OnResourceTaskComplete, OnTimeLeftToComplete, collectPerDuration);
		}
		*/
		
		/*
		_task = ResourceGenerateTask.CreateTask(resourceId, resourceRegenPerDuration, OnResourceTaskComplete, OnTimeLeftToComplete, collectPerDuration);
		
		TaskManager.Instance.AddTask (_task);
		*/
		
		if(DoStartRegenTask())
		{
			_task = FoodResourceGenerateTask.CreateTask<FoodResourceGenerateTask>(resourceId, resourceRegenPerDuration, OnResourceTaskComplete, OnTimeLeftToComplete, collectPerDuration);
			
			TaskManager.Instance.AddTask (_task);
		}
		
	}
	
	void OnResourceTaskComplete(ResourceGenerateTask task, float generatedResource, ResourceType resourceId)
	{
		/*
		_tempResourceStore += generatedResource;
		
		Debug.Log ("Collect resource " + resourceId);
		Debug.Log("You may click to transfer resource");


		EventManager.GetInstance ().ExecuteEvent<EventCollectResource> (new EventCollectResource (transform, _tempResourceStore));
		
		if((_currentResourceStore+_tempResourceStore) < maxResourceStore)
		{
			AddResourceRegenTask();
		}
		else
		{
			_task = null;
		}
		*/
		
		//StartCoroutine (CompleteTask ());

		DoFetchData ();
		
	}
	
	void OnTimeLeftToComplete(TimeTask task, int secondLeft)
	{
		Debug.Log (gameObject.name+" collect time left: "+ TimeConverter.GetHours (secondLeft) + ":"
		           + TimeConverter.GetMinutes (secondLeft) + ":" + TimeConverter.GetSeconds (secondLeft));
	}
	
	void OnUIResourcePopupClick(EventUIResoucePopupClick e)
	{
		if(e.GetTarget != transform)
		{
			return;
		}
		
		/*
		TransferResource ();

		e.GetUIButton.gameObject.SetActive(false);
		*/
		
		if(DoTransferResource())
		{
			EventManager.GetInstance ().ExecuteEvent<EventResourceTransferToPlayer> (new EventResourceTransferToPlayer (resourceId));
			
			Debug.Log (gameObject.name+" Transfer resource ");
		}
	}

	public bool IsOnTask ()
	{
		if(_task != null)
		{
			return true;
		}
		
		return false;
	}
}
