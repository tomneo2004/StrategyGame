using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IArchitecture
{
	//is architecture on task
	bool IsOnTask ();
}

public abstract class Architecture : MonoBehaviour 
{

	public bool isSelected = false;

	string architectureId;

	public string ArchitectureId
	{
		get
		{
			if(!string.IsNullOrEmpty(architectureId))
			{
				return architectureId;
			}

			return null;
		}
	}

	protected virtual void Awake()
	{
		architectureId = ArchitectureManager.Instance.AddArchitecture (this);
	}

	// Use this for initialization
	protected virtual void Start () 
	{
	
	}

	protected virtual void OnEnable()
	{

	}

	protected virtual void OnDisable()
	{

	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
	
	}

	/// <summary>
	/// On this architecture select.
	/// </summary>
	public virtual void OnArchitectureSelect()
	{

		isSelected = true;

	}

	/// <summary>
	/// On this architecture deselect.
	/// </summary>
	public virtual void OnArchitectureDeselect()
	{
		isSelected = false;
	}

	/// <summary>
	/// Chcek if this architecture on task.
	/// </summary>
	/// <returns><c>true</c> if this instance is architecture on task; otherwise, <c>false</c>.</returns>
	protected bool IsArchitectureOnTask()
	{
		MonoBehaviour[] mbs = GetComponents<MonoBehaviour> ();

		List<IArchitecture> list = new List<IArchitecture> ();

		for(int i=0; i<mbs.Length; i++)
		{
			if(mbs[i] is IArchitecture)
			{
				list.Add((IArchitecture)mbs[i]);
			}
		}

		for(int i=0; i<list.Count; i++)
		{
			if(list[i].IsOnTask())
			{
				return true;
			}
		}

		return false;
	}
}
