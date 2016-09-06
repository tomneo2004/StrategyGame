using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ArchitectureManager : MonoBehaviour 
{
	static ArchitectureManager _instance;

	public static ArchitectureManager Instance
	{
		get
		{
			ArchitectureManager arcMgr = GameObject.FindObjectOfType<ArchitectureManager>();
			
			if(arcMgr != null)
			{
				_instance = arcMgr;
			}
			else
			{
				GameObject arcMgrObject = new GameObject();
				
				arcMgrObject.name = "ArchitectureManager";
				
				_instance = arcMgrObject.AddComponent<ArchitectureManager>();
			}
			
			return _instance;
		}
	}

	private Dictionary<string, Architecture> _idToArchitecture = new Dictionary<string, Architecture>();

	string selectedArchitectureId;

	/// <summary>
	/// Get selected architecture.
	/// </summary>
	/// <value>The selected architecture.</value>
	public Architecture selectedArchitecture
	{
		get
		{
			if(string.IsNullOrEmpty(selectedArchitectureId))
			{
				return null;
			}

			return _idToArchitecture[selectedArchitectureId];
		}
	}

	// Use this for initialization
	void Start () 
	{

	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventInputOnObjectSelected> (OnGameObjectSelected);
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventInputOnObjectSelected> (OnGameObjectSelected);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	string GenerateNewArchitectureId()
	{
		string id;
		
		while(true)
		{
			id = Guid.NewGuid().ToString();
			
			if(!_idToArchitecture.ContainsKey(id))
			{
				return id;
			}
		}
	}

	/// <summary>
	/// Adds an architecture.
	/// </summary>
	/// <returns>The architecture.</returns>
	/// <param name="architecture">Architecture.</param>
	public string AddArchitecture(Architecture architecture)
	{
		string id = GenerateNewArchitectureId ();
		
		_idToArchitecture.Add (id, architecture);

		Debug.Log ("ArchitectureManager add architecture with id:" + id +"  GameObject name:"+ architecture.gameObject.name);
		
		return id;
	}

	/// <summary>
	/// Removes an architecture.
	/// </summary>
	/// <param name="architecture">Architecture.</param>
	public void RemoveArchitecture(Architecture architecture)
	{
		if(!string.IsNullOrEmpty(architecture.ArchitectureId))
		{
			_idToArchitecture.Remove(architecture.ArchitectureId);
		}
		else
		{
			Debug.LogError("Unable to remove architecture, id was not assigned");
		}
	}

	/// <summary>
	/// handle event when input controller select GameObject in scene.
	/// </summary>
	/// <param name="e">E.</param>
	void OnGameObjectSelected(EventInputOnObjectSelected e)
	{
		Architecture arc = e.selectedObject.GetComponent<Architecture> ();

		if(arc != null)
		{
			selectedArchitectureId = arc.ArchitectureId;

			arc.OnArchitectureSelect();
		}
		else
		{
			if(!string.IsNullOrEmpty(selectedArchitectureId))
			{
				_idToArchitecture[selectedArchitectureId].OnArchitectureDeselect();
			}

			selectedArchitectureId = null;
		}
	}
}
