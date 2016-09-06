using UnityEngine;
using System.Collections;

public class ResourceDisplay : MonoBehaviour 
{
	public ResourceType resourceId;

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnEnable()
	{

		EventManager.GetInstance ().AddListener<EventResourceStorageUpdate> (OnUpdateResourceStorage);
	}

	void OnDisable()
	{

		EventManager.GetInstance ().RemoveListener<EventResourceStorageUpdate> (OnUpdateResourceStorage);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Handle update resource storage event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnUpdateResourceStorage(EventResourceStorageUpdate e)
	{
		if(e.resourceId != resourceId)
		{
			return;
		}

		GetComponentInChildren<UILabel> ().text = e.currentResource.ToString();
	}
}
