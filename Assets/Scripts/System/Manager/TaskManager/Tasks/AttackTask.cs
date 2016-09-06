using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AttackResult
{
	Unknow,
	Win,
	Lost,
	Draw
}

public class AttackTask : TimeTask 
{
	public delegate void OnAttackComplete(AttackTask task);
	public OnAttackComplete Evt_OnAttackComplete;

	string _taskId;

	public string TaskId
	{
		get
		{
			return _taskId;
		}
	}

	Dictionary<CombatUnit, int> _playerUnit;

	public Dictionary<CombatUnit, int> PlayerUnit
	{
		get
		{
			return _playerUnit;
		}
	}

	string _targetPlayerId;

	public string TargetPlayerId
	{
		get
		{
			return _targetPlayerId;
		}
	}

	string _targetPlayerName;

	public string TargetPlayerName
	{
		get
		{
			return _targetPlayerName;
		}
	}

	Dictionary<CombatUnit, int> _targetPlayerUnit;

	public Dictionary<CombatUnit, int> TargetPlayerUnit
	{
		get
		{
			return _targetPlayerUnit;
		}
	}

	public int TargetPlayerTotalUnit
	{
		get
		{
			int retVal = 0;

			foreach(CombatUnit unit in _targetPlayerUnit.Keys)
			{
				retVal += _targetPlayerUnit[unit];
			}

			return retVal;
		}
	}

	int _waitDuration;

	public int WaitDuration
	{
		get
		{
			return _waitDuration;
		}
	}

	public int CurrentDuration
	{
		get
		{
			return _currentDuration;
		}
	}

	AttackResult _attackResult = AttackResult.Unknow;

	public AttackResult GetAttackResult
	{
		get
		{
			return _attackResult;
		}
	}

	public static AttackTask CreateTask(string taskId, string targetId, string targetName, Dictionary<CombatUnit, int> targetUnit, 
	                                    Dictionary<CombatUnit, int> playerUnit, OnAttackComplete completeCallBack = null,
	                                         OnTimeLeftToComplete timeLeftCallBack = null, int duration = 1)
	{
		AttackTask task = new AttackTask ();
		
		task.InitTask (taskId, targetId, targetName, targetUnit, playerUnit, completeCallBack, timeLeftCallBack, duration);
		
		return task;
	}

	public void InitTask(string taskId, string targetId, string targetName, Dictionary<CombatUnit, int> targetUnit, 
	                     Dictionary<CombatUnit, int> playerUnit, OnAttackComplete completeCallBack = null,
	                     OnTimeLeftToComplete timeLeftCallBack = null, int duration = 1)
	{
		
		base.InitTask (duration, timeLeftCallBack);
		
		_taskId = taskId;
		
		_targetPlayerId = targetId;

		_targetPlayerName = targetName;

		_targetPlayerUnit = targetUnit;

		_playerUnit = playerUnit;

		_waitDuration = duration;
		
		Evt_OnAttackComplete += completeCallBack;
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
		AttackManagerMetaData data = AttackManagerMetaData.Load ();

		AttackInfo info = null;
		
		while((info = data.IsAttackTaskComplete(_taskId)) == null)
		{
			yield return new WaitForSeconds(1f);
		}

		_attackResult = info.result;
		
		CompleteTask ();
	}

	/// <summary>
	/// Cancel task.
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	public override bool CancelTask()
	{
		AttackManagerMetaData data = AttackManagerMetaData.Load ();
		
		if(data.CancelAttack (_taskId))
		{
			base.CancelTask();

			return true;
		}

		return false;
	}

	/// <summary>
	/// Completes the task.
	/// Task will remove itself from TaskManager
	/// </summary>
	public override void CompleteTask()
	{
		base.CompleteTask ();
		
		if(Evt_OnAttackComplete != null)
		{
			Evt_OnAttackComplete(this);
		}
	}
}
