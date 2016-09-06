using UnityEngine;
using System.Collections;

public class ResourceStorage : MonoBehaviour 
{
	public ResourceType resourceId;

	public float maxResource = 100f;

	public float currentResource = 0f;

	// Use this for initialization
	void Start () 
	{
		UpdateResourceStorage ();
	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventResourceTransferToPlayer> (OnResourceTransferToPlayer);
		EventManager.GetInstance ().AddListener<EventProduceCombatUnit> (OnProduceUnit);
		EventManager.GetInstance ().AddListener<EventResourceStorageModelUpdate> (OnResourceStorageModelUpdate);

	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventResourceTransferToPlayer> (OnResourceTransferToPlayer);
		EventManager.GetInstance ().RemoveListener<EventProduceCombatUnit> (OnProduceUnit);
		EventManager.GetInstance ().RemoveListener<EventResourceStorageModelUpdate> (OnResourceStorageModelUpdate);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void UpdateResourceStorage()
	{
		PlayerResourceStorageMetaData data = PlayerResourceStorageMetaData.Load ();

		ResourceStorageMetaData rsData = data.GetResourceMetaData (resourceId);

		maxResource = rsData.maxResource;
		currentResource = rsData.currentResource;

		EventManager.GetInstance ().ExecuteEvent<EventResourceStorageUpdate> (new EventResourceStorageUpdate (resourceId ,currentResource));
	}

	/// <summary>
	/// Handle resource transfer to player
	/// </summary>
	/// <param name="e">E.</param>
	void OnResourceTransferToPlayer(EventResourceTransferToPlayer e)
	{
		if(e.resourceId != resourceId)
		{
			return;
		}

		UpdateResourceStorage ();
	}


	/// <summary>
	/// Handle produce unit event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnProduceUnit(EventProduceCombatUnit e)
	{
		if(e.ContainResourceType(resourceId))
		{
			UpdateResourceStorage();
		}

	}

	/// <summary>
	/// Handle resource storage model update event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnResourceStorageModelUpdate(EventResourceStorageModelUpdate e)
	{
		if(e.resourceType == resourceId)
		{
			UpdateResourceStorage ();
		}

	}
	
}
