using UnityEngine;
using System.Collections;
using System;

public class TimeTask : Task 
{
	
	public delegate void OnTimeLeftToComplete(TimeTask task, int secondLeft);
	/// <summary>
	/// Notify every second and how much time left for this task to complete.
	/// </summary>
	public OnTimeLeftToComplete Evt_OnTimeLeftToComplete;

	/// <summary>
	/// The how long is this task complete.
	/// Base on second
	/// </summary>
	protected int _duration = 1;

	/// <summary>
	/// Gets the total duration of the task.
	/// Base on second
	/// </summary>
	/// <value>The duration of the task.</value>
	public int TaskDuration
	{
		get
		{
			return _duration;
		}
	}

	/// <summary>
	/// Current task duration
	/// </summary>
	protected int _currentDuration = 0;

	public int CurrentTaskDuration
	{
		get
		{
			if(_currentDuration < 0)
			{
				return 0;
			}

			return _currentDuration;
		}
	}

	/// <summary>
	/// True when task reach it's duration
	/// </summary>
	protected bool _isDeadline = false;
	
	/// <summary>
	/// Used to calculate 1 second pass.
	/// </summary>
	protected DateTime _lastTime;

	/// <summary>
	/// indicate this task should not be update anymore.
	/// </summary>
	protected bool _timeIsUp = false;

	~TimeTask()
	{
		Evt_OnTimeLeftToComplete = null;
	}

	/// <summary>
	/// Init time task.
	/// </summary>
	/// <param name="taskDuration">Task duration.</param>
	/// <param name="timeLeftCallBack">Time left call back.</param>
	public virtual void InitTask(int taskDuration, OnTimeLeftToComplete timeLeftCallBack)
	{
		_timeIsUp = false;

		_duration = taskDuration;

		_isDeadline = false;

		_currentDuration = taskDuration;

		_lastTime = DateTime.Now.Subtract (new TimeSpan (0, 0, 1));

		Evt_OnTimeLeftToComplete += timeLeftCallBack;
	}

	/// <summary>
	/// Call to updates the task.
	/// </summary>
	public override void UpdateTask()
	{
		if(_timeIsUp)
		{
			return;
		}

		base.UpdateTask ();

		TimeSpan tSpan = DateTime.Now.Subtract(_lastTime);
		
		//1 second pass
		if(tSpan.TotalMilliseconds >= 1000f)
		{
			--_currentDuration;

			if((_currentDuration <= 0) && !_isDeadline)
			{
				OnDeadline();

				_isDeadline = true;
			}
			
			TimeTick();

			if((Evt_OnTimeLeftToComplete != null) && (!_timeIsUp))
			{
				Evt_OnTimeLeftToComplete(this, _currentDuration);
			}
			
			_lastTime = DateTime.Now;
		}
	}

	/// <summary>
	/// tick
	/// </summary>
	protected virtual void TimeTick()
	{

	}

	/// <summary>
	/// This is called when task reach it's duration
	/// This only called once 
	/// </summary>
	protected virtual void OnDeadline()
	{

	}

	/// <summary>
	/// Completes the task.
	/// Task will remove itself from TaskManager
	/// </summary>
	public override void CompleteTask()
	{
		base.CompleteTask ();

		_timeIsUp = true;
	}

	/// <summary>
	/// Cancel task.
	/// </summary>
	/// <returns><c>true</c> if this instance cancel task; otherwise, <c>false</c>.</returns>
	public override bool CancelTask()
	{
		base.CancelTask ();

		_timeIsUp = true;

		return true;
	}

	/// <summary>
	/// Restart this task.
	/// </summary>
	public override void StartOver()
	{
		base.StartOver ();

		_currentDuration = _duration;

		_isDeadline = false;
	}
}
