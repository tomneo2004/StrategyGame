using UnityEngine;
using System.Collections;
using System;

public abstract class ResourceGenerateTask  : TimeTask
{
	public delegate void OnResourceTaskComplete(ResourceGenerateTask task, float generatedResource, ResourceType resourceId);
	/// <summary>
	/// Notify when resource task complete.
	/// </summary>
	public OnResourceTaskComplete Evt_OnResourceTaskComplete;

	/// <summary>
	/// The resource identifier.
	/// </summary>
	private ResourceType _resourceId;

	/// <summary>
	/// How much resource will generate when complete.
	/// </summary>
	private float _resource = 0f;

	~ResourceGenerateTask()
	{
		Evt_OnResourceTaskComplete = null;
	}

	/// <summary>
	/// Easy way to create the resource task.
	/// </summary>
	/// <returns>The task.</returns>
	/// <param name="resourceId">Resource identifier.</param>
	/// <param name="resourceToGenerate">Resource to generate.</param>
	/// <param name="completeCallBack">Complete call back.</param>
	/// <param name="duration">Duration.</param>
	/// <param name="timeType">Time type.</param>
	public static T CreateTask<T>(ResourceType resourceId, float resourceToGenerate, OnResourceTaskComplete completeCallBack = null,
	                                              OnTimeLeftToComplete timeLeftCallBack = null, int duration = 1) where T : ResourceGenerateTask, new()
	{
		T task = new T ();

		((ResourceGenerateTask)task).InitTask (resourceId, resourceToGenerate, completeCallBack, timeLeftCallBack, duration);

		return task;
	}

	/// <summary>
	/// Init resource generate task.
	/// call after first call will reset all value to new value
	/// </summary>
	/// <param name="resourceId">Resource identifier.</param>
	/// <param name="resourceToGenerate">Resource to generate.</param>
	/// <param name="completeCallBack">Complete call back.</param>
	/// <param name="duration">Duration.</param>
	/// <param name="timeType">Time type.</param>
	public void InitTask(ResourceType resourceId, float resourceToGenerate, OnResourceTaskComplete completeCallBack = null,
	                     OnTimeLeftToComplete timeLeftCallBack = null, int duration = 1)
	{

		base.InitTask (duration, timeLeftCallBack);

		_resourceId = resourceId;

		_resource = resourceToGenerate;

		Evt_OnResourceTaskComplete = completeCallBack;
	}

	/// <summary>
	/// 1 second tick
	/// </summary>
	protected override void TimeTick()
	{
		base.TimeTick ();

		/*
		if(_currentDuration <= 0)
		{
			CompleteTask();
		}
		*/
	}

	/// <summary>
	/// This is called when task reach it's duration
	/// This only called once
	/// </summary>
	protected override void OnDeadline ()
	{
		base.OnDeadline ();
		
		BeginCoroutine (CheckTaskComplete ());
	}
	
	protected abstract IEnumerator CheckTaskComplete ();


	public override void CompleteTask()
	{
		base.CompleteTask ();

		if(Evt_OnResourceTaskComplete != null)
		{
			Evt_OnResourceTaskComplete(this, _resource, _resourceId);
		}
	}
}
