using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WWWTaskManager : MonoBehaviour 
{
	static WWWTaskManager _instance;

	/// <summary>
	/// add task has been called.
	/// </summary>
	bool _taskAdded = false;


	/// <summary>
	/// Return WWWTaskManager instance.
	/// </summary>
	/// <value>The instance.</value>
	public static WWWTaskManager Instance
	{
		get
		{
			WWWTaskManager iQueue = GameObject.FindObjectOfType<WWWTaskManager>();

			if(iQueue != null)
			{
				_instance = iQueue;
			}
			else
			{
				GameObject newQueue = new GameObject();

				newQueue.name = "WWWTaskManager";

				_instance = newQueue.AddComponent<WWWTaskManager>();
			}

			return _instance;
		}
	}

	/// <summary>
	/// Contain all running tasks.
	/// </summary>
	Dictionary<string, WWWTask> _tasks;

	List<string> _pendingRemoveTaskId = new List<string>();

	/// <summary>
	/// Return how many tasks
	/// </summary>
	/// <value>The task count.</value>
	public int TaskCount
	{
		get
		{
			if(_tasks!=null)
			{
				return _tasks.Count;
			}

			return 0;
		}
	}

	//debug
	public int TotalTask = 0;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//debug
		TotalTask = _tasks.Count;

		for(int i=0; i<_pendingRemoveTaskId.Count; i++)
		{
			FindTask(_pendingRemoveTaskId[i]).OnTaskRemove();

			_tasks.Remove(_pendingRemoveTaskId[i]);
		}

		_pendingRemoveTaskId.Clear ();

		foreach(string taskId in _tasks.Keys)
		{
			if(_tasks[taskId].IsDone)
			{
				//remove task on next frame
				_pendingRemoveTaskId.Add(taskId);
			}
		}
	}

	/// <summary>
	/// Generate unique id.
	/// </summary>
	/// <returns>The GUI.</returns>
	private string GenerateGUID()
	{
		while(true)
		{
			string newId = Guid.NewGuid().ToString();

			if(_tasks == null)
			{
				return newId;
			}

			if(!_tasks.ContainsKey(newId))
			{
				return newId;
			}
		}
	}

	/// <summary>
	/// Create new task.
	/// </summary>
	/// <returns>The task.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public T CreateTask<T>() where T : WWWTask, new()
	{
		T newTask = new T ();

		newTask.InitTask (GenerateGUID(), this);

		return newTask;
	}

	/// <summary>
	/// Add task.
	/// </summary>
	/// <param name="taskToAdd">Task to add.</param>
	public void AddTask(WWWTask taskToAdd)
	{
		if(taskToAdd != null)
		{
			if(_tasks == null)
			{
				_tasks = new Dictionary<string, WWWTask>();
			}

			_tasks.Add(taskToAdd.TaskId, taskToAdd);

			_taskAdded = true;

			taskToAdd.OnTaskAdded();
		}
		else
		{
			Debug.LogError(gameObject.name+" pass in task is null");
		}
	}
	

	/// <summary>
	/// Remove a task.
	/// </summary>
	/// <param name="taskId">Task identifier.</param>
	public void RemoveTask(string taskId)
	{
		if(!_tasks.ContainsKey(taskId))
		{
			return;
		}
		else
		{
			FindTask(taskId).OnTaskRemove();

			_tasks.Remove(taskId);

		}
	}

	/// <summary>
	/// Find the task.
	/// Return null if it can not find it
	/// </summary>
	/// <returns>The task.</returns>
	/// <param name="taskId">Task identifier.</param>
	public WWWTask FindTask(string taskId)
	{
		if(!_tasks.ContainsKey(taskId))
		{
			return null;
		}
		else
		{
			return _tasks[taskId];
		}
	}
}
