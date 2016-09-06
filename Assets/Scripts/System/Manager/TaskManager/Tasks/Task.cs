using UnityEngine;
using System.Collections;

public class Task 
{
	protected TaskManager _taskManager;

	public TaskManager TaskManagerInstance
	{
		get
		{
			return _taskManager;
		}

		set
		{
			_taskManager = value;
		}
	}

	public delegate void OnTaskComplete(Task task);
	/// <summary>
	/// notify on task complete.
	/// </summary>
	public OnTaskComplete Evt_OnTaskComplete;

	public delegate void OnTaskCancel(Task task);
	/// <summary>
	/// notify on task cancel.
	/// </summary>
	public OnTaskCancel Evt_OnTaskCancel;

	public delegate void OnTaskStartOver(Task task);
	/// <summary>
	/// notify on task start over.
	/// </summary>
	public OnTaskStartOver Evt_OnTaskStartOver;

	/// <summary>
	/// Call to updates the task.
	/// </summary>
	public virtual void UpdateTask()
	{

	}

	/// <summary>
	/// Init the task.
	/// </summary>
	public virtual void InitTask()
	{

	}

	/// <summary>
	/// Completes the task.
	/// Task will remove itself from TaskManager
	/// </summary>
	public virtual void CompleteTask()
	{
		if(Evt_OnTaskComplete != null)
		{
			Evt_OnTaskComplete(this);
		}

		EndCoroutine ();

		_taskManager.RemoveTask (this);
	}

	public virtual void CompleteTaskInstant()
	{

	}

	public virtual bool CancelTask()
	{
		if(Evt_OnTaskCancel != null)
		{
			Evt_OnTaskCancel(this);
		}

		EndCoroutine ();

		_taskManager.RemoveTask (this);

		return true;
	}

	/// <summary>
	/// Restart this task.
	/// </summary>
	public virtual void StartOver()
	{

		if(Evt_OnTaskStartOver != null)
		{
			Evt_OnTaskStartOver(this);
		}
	}

	/// <summary>
	/// Easy way to begins the coroutine.
	/// </summary>
	/// <param name="coroutine">Coroutine.</param>
	protected void BeginCoroutine(IEnumerator coroutine)
	{
		_taskManager.BeginCoroutine (this, coroutine);
	}

	/// <summary>
	/// Easy way to ends the coroutine.
	/// </summary>
	protected void EndCoroutine()
	{
		_taskManager.EndCoroutine (this);
	}
}
