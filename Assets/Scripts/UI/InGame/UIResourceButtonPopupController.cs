using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UIButtonPopupInfo
{
	public UIButtonPopupInfo(ResourceType resourceId, GameObject prefab)
	{
		_resourceId = resourceId;
		_prefab = prefab;
	}

	[SerializeField]
	ResourceType _resourceId;

	/// <summary>
	/// Gets or sets the resource identifier.
	/// </summary>
	/// <value>The resource identifier.</value>
	public ResourceType ResourceId
	{
		get
		{
			return _resourceId;
		}

		set
		{
			_resourceId = value;
		}
	}

	[SerializeField]
	GameObject _prefab;

	/// <summary>
	/// Gets or sets the popup prefab.
	/// </summary>
	/// <value>The popup prefab.</value>
	public GameObject PopupPrefab
	{
		get
		{
			return _prefab;
		}

		set
		{
			_prefab = value;
		}
	}
}

public class UIResourceButtonPopupController : MonoBehaviour 
{

	[HideInInspector][Tooltip("Popup prefab that corespond to resource id")]
	public List<UIButtonPopupInfo> popupPrefabs;
	//public Dictionary<string, GameObject> popupPrefabs;

	// Use this for initialization
	void Start () 
	{

	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventCollectResource> (ResourceCollectEvent);
	}

	void OnDisable()
	{
		EventManager.GetInstance().RemoveListener<EventCollectResource> (ResourceCollectEvent);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Check if there is a resource popup for specified resource type.
	/// </summary>
	/// <returns><c>true</c> if this instance has resource popup the specified comparedId; otherwise, <c>false</c>.</returns>
	/// <param name="comparedId">Compared identifier.</param>
	bool HasResourcePopup(ResourceType comparedId)
	{
		if (popupPrefabs != null)
		{
			for(int i=0; i<popupPrefabs.Count; i++)
			{
				if(comparedId == popupPrefabs[i].ResourceId)
				{
					return true;
				}
			}
		}

		return false;
	}

	/// <summary>
	/// Gets the popup prefab.
	/// </summary>
	/// <returns>The popup prefab.</returns>
	/// <param name="resourceId">Resource identifier.</param>
	GameObject GetPopupPrefab(ResourceType resourceId)
	{
		if(popupPrefabs != null)
		{
			for(int i=0; i<popupPrefabs.Count; i++)
			{
				if(resourceId == popupPrefabs[i].ResourceId)
				{
					return popupPrefabs[i].PopupPrefab;
				}
			}
		}

		return null;
	}

	/// <summary>
	/// Handle resource collect event
	/// </summary>
	/// <param name="e">E.</param>
	void ResourceCollectEvent(EventCollectResource e)
	{
		GameObject btnPopup = null;

		//see if there is already exist one
		foreach(Transform t in transform)
		{
			if(t.GetComponent<UIFollowTarget>().target == e.GetTarget)
			{
				btnPopup = t.gameObject;
			}
		}

		if(btnPopup != null)
		{
			btnPopup.SetActive (true);
		}
		else
		{
			if(popupPrefabs == null)
			{
				Debug.LogError(gameObject.name+" could not find popup UI for resource id "+e.GetResourceId+" no prefabs");
				return;
			}

			if(!HasResourcePopup(e.GetResourceId))
			{
				Debug.LogError(gameObject.name+" could not find popup UI for resource id "+e.GetResourceId);
				return;
			}
			else
			{
				GameObject popupPrefab = GetPopupPrefab(e.GetResourceId);

				if(popupPrefab == null)
				{
					Debug.LogError(gameObject.name+" prefab is null");
					return;
				}


				btnPopup = NGUITools.AddChild (gameObject, popupPrefab);
				
				EventDelegate.Set(btnPopup.GetComponent<UIButton>().onClick, OnPopupClick);

				btnPopup.GetComponent<UIFollowTarget> ().Target = e.GetTarget;
				

			}


		}

	}

	/// <summary>
	/// Handle popup click event.
	/// </summary>
	void OnPopupClick()
	{
		EventManager.GetInstance ().ExecuteEvent<EventUIResoucePopupClick> (new EventUIResoucePopupClick (UIButton.current.GetComponent<UIFollowTarget> ().target, UIButton.current));

		UIButton.current.gameObject.SetActive (false);
	}
}
