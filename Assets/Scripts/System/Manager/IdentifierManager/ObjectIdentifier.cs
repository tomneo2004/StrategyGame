using UnityEngine;
using System.Collections;

public class ObjectIdentifier : MonoBehaviour 
{
	public int Id = 1;

	void Awake()
	{
		if(!IdentifierManager.Instance.RegisterId (Id, gameObject))
		{
			Debug.LogError(gameObject.name+" can not register id, id already exist");
		}
	}

	void OnDestroy()
	{
		IdentifierManager.Instance.RemoveId (Id);
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
