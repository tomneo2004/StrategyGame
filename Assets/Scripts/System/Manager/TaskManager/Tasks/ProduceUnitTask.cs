using UnityEngine;
using System.Collections;

public class ProduceUnitTask : TimeTask 
{
	public delegate void OnProduceUnitComplete(ProduceUnitTask task, CombatUnitType unitType, int amount);
	public OnProduceUnitComplete Evt_OnProduceUnitComplete;

	private CombatUnitType _producingUnitType;

	public CombatUnitType UnitType
	{
		get
		{
			return _producingUnitType;
		}
	}

	private int _producingUnitAmount;

	public int ProducingUnitAmount
	{
		get
		{
			return _producingUnitAmount;
		}
	}

	~ProduceUnitTask()
	{
		Evt_OnProduceUnitComplete = null;
	}


	public static ProduceUnitTask CreateTask(CombatUnitType unitType, int unitAmount, OnProduceUnitComplete completeCallBack = null,
	                                              OnTimeLeftToComplete timeLeftCallBack = null, int duration = 1)
	{
		ProduceUnitTask task = new ProduceUnitTask ();
		
		task.InitTask (unitType, unitAmount, completeCallBack, timeLeftCallBack, duration);
		
		return task;
	}

	public void InitTask(CombatUnitType unitType, int unitAmount, OnProduceUnitComplete completeCallBack = null,
	                     OnTimeLeftToComplete timeLeftCallBack = null, int duration = 1)
	{
		
		base.InitTask (duration, timeLeftCallBack);
		
		_producingUnitType = unitType;
		
		_producingUnitAmount = unitAmount;
		
		Evt_OnProduceUnitComplete += completeCallBack;
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

	/// <summary>
	/// Checks the task complete.
	/// </summary>
	/// <returns>The task complete.</returns>
	IEnumerator CheckTaskComplete()
	{
		bool taskComplete = false;

		CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load ();

		while(!taskComplete)
		{
			taskComplete = data.IsProduceUnitComplete ();

			yield return new WaitForSeconds(1f);
		}

		CompleteTask ();
	}

	/// <summary>
	/// Completes the task.
	/// Task will remove itself from TaskManager
	/// </summary>
	public override void CompleteTask()
	{
		base.CompleteTask ();
		
		if(Evt_OnProduceUnitComplete != null)
		{
			Evt_OnProduceUnitComplete(this, _producingUnitType, _producingUnitAmount);
		}
	}

	public override void CompleteTaskInstant ()
	{
		base.CompleteTaskInstant ();

		CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load ();

		data.FinishProducingUnit ();

		CompleteTask ();
	}
}
