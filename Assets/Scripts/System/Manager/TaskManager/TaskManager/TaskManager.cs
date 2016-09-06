using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour 
{

	static TaskManager _instance;

	/// <summary>
	/// Instance of TaskManager.
	/// </summary>
	/// <value>The instance.</value>
	public static TaskManager Instance
	{
		get
		{
			TaskManager taskMgr = GameObject.FindObjectOfType<TaskManager>();

			if(taskMgr != null)
			{
				_instance = taskMgr;
			}
			else
			{
				GameObject taskMgrObject = new GameObject();
				
				taskMgrObject.name = "TaskManager";
				
				_instance = taskMgrObject.AddComponent<TaskManager>();
			}
			
			return _instance;
		}
	}

	/// <summary>
	/// The max tasks manager can process at same time.
	/// -1 unlimit, otherwise it should be the value that is not lower than -1 or equal to 0
	/// </summary>
	int _maxProcessTask = -1;

	/// <summary>
	/// Gets or sets the max process task.
	/// -1 unlimit, otherwise it should be the value that is not lower than -1 or equal to 0
	/// </summary>
	/// <value>The max process task.</value>
	public int MaxProcessTask
	{
		get
		{
			return _maxProcessTask;
		}

		set
		{
			if((value<-1) || (value==0))
			{
				Debug.LogError(gameObject.name+" Max process task can't be 0 or lower than -1");
			}
			else
			{
				_maxProcessTask = value;
			}
		}
	}

	/// <summary>
	/// All tasks in queue.
	/// </summary>
	List<Task> _tasks;

	Dictionary<Task, IEnumerator> _taskToCoroutine = new Dictionary<Task, IEnumerator>();

	public delegate void OnAllTaskComplete(TaskManager manager);
	/// <summary>
	/// Notify when all task complete.
	/// </summary>
	public OnAllTaskComplete Evt_OnAllTaskComplete;

	void Start()
	{
	}

	void Update()
	{
		//process task
		if((_tasks != null) && (_tasks.Count > 0))
		{
			//process all tasks
			if(_maxProcessTask == -1)
			{
				for(int i=0; i<_tasks.Count; i++)
				{
					_tasks[i].UpdateTask();

				}
			}
			else//process limit tasks
			{
				for(int i=0; i<_maxProcessTask; i++)
				{
					_tasks[i].UpdateTask();
				}

			}
		}
	}

	/// <summary>
	/// Add the task.
	/// </summary>
	/// <param name="newTask">New task.</param>
	public void AddTask(Task newTask)
	{
		if(_tasks == null)
		{
			_tasks = new List<Task>();
		}

		newTask.TaskManagerInstance = this;

		_tasks.Add (newTask);
	}

	/// <summary>
	/// Remove the task.
	/// </summary>
	/// <param name="removedTask">Removed task.</param>
	public void RemoveTask(Task removedTask)
	{
		if((_tasks != null) && (_tasks.Count > 0))
		{
			EndCoroutine(removedTask);

			_tasks.Remove(removedTask);
		}
		else
		{
			Debug.LogWarning(gameObject.name+" either task queue is null or no task");
		}
	}

	/// <summary>
	/// Find the task.
	/// </summary>
	/// <returns>The task.</returns>
	/// <param name="taskToFind">Task to find.</param>
	public Task FindTask(Task taskToFind)
	{
		if((_tasks != null) && (_tasks.Count > 0))
		{
			for(int i=0; i<_tasks.Count; i++)
			{
				Task t = _tasks[i];

				if(t == taskToFind)
				{
					return t;
				}
			}
		}

		return null;
	}

	/// <summary>
	/// Complete the current task.
	/// First task is current 
	/// </summary>
	public void CompleteCurrentTask()
	{
		if((_tasks != null) && (_tasks.Count > 0))
		{
			_tasks[0].CompleteTask();

			_tasks.RemoveAt(0);
		}
	}

	/// <summary>
	/// Complete the task.
	/// </summary>
	/// <param name="t">T.</param>
	public void CompleteTask(Task t)
	{
		if((_tasks != null) && (_tasks.Count > 0))
		{
			for(int i=0; i<_tasks.Count; i++)
			{
				Task task = _tasks[i];
				
				if(task == t)
				{
					task.CompleteTask();

					_tasks.Remove(task);

					return;
				}
			}
		}
	}

	/// <summary>
	/// Begins a coroutine for task.
	/// Only one coroutine at a time.
	/// </summary>
	/// <param name="task">Task.</param>
	/// <param name="coroutine">Coroutine.</param>
	public void BeginCoroutine(Task task, IEnumerator coroutine)
	{

		if(_taskToCoroutine.ContainsKey(task))
		{
			StopCoroutine(_taskToCoroutine[task]);

			_taskToCoroutine[task] = coroutine;
		}
		else
		{
			_taskToCoroutine.Add(task, coroutine);

			StartCoroutine(coroutine);
		}
	}

	/// <summary>
	/// End/stop the coroutine for task.
	/// </summary>
	/// <param name="task">Task.</param>
	public void EndCoroutine(Task task)
	{
		if(_taskToCoroutine.ContainsKey(task))
		{
			StopCoroutine(_taskToCoroutine[task]);

			_taskToCoroutine.Remove(task);
		}
	}
}
