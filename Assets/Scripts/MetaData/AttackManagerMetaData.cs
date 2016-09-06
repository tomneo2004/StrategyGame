using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[System.Serializable]
public class AttackInfo
{
	public string taskId;

	public string targetId;

	public string targetName;

	public Dictionary<CombatUnit, int> targetUnit;

	public Dictionary<CombatUnit, int> attackerUnit;

	public Dictionary<ResourceType, float> resourceAward;

	public int duration;

	public AttackResult result = AttackResult.Unknow;

	public DateTime taskStartTime;

	public DateTime taskEndTime;

	public AttackInfo(string tskId, string tId, string tName, Dictionary<CombatUnit, int> tUnit, Dictionary<CombatUnit, int> aUnit, int taskDuration, 
	                  Dictionary<ResourceType, float> theResourceAward)
	{
		taskId = tskId;
		targetId = tId;
		targetName = tName;
		targetUnit = tUnit;
		attackerUnit = aUnit;
		duration = taskDuration;
		resourceAward = theResourceAward;

		taskStartTime = DateTime.Now;
		taskEndTime = DateTime.Now.AddSeconds (taskDuration);
	}

	/// <summary>
	/// Calculates the battle result.
	/// </summary>
	public void CalculateResult()
	{
		BattleReportMetaData reportData = BattleReportMetaData.Load ();

		int fAttackerSum = GetAttackerSum ();
		int fDefenderSum = GetDefenderSum ();

		if((fAttackerSum - fDefenderSum) > 0)//attacker win, defender defeat
		{
			Debug.Log("Attacker "+GetAttackerSum()+" vs "+targetName+" "+GetDefenderSum()+" win");

			float ratio = (float)fDefenderSum * 100f / (float)fAttackerSum / 100f;
			
			CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load();

			//calculate unit lost for attacker
			List<UnitLostInfo> attackerLostInfo = new List<UnitLostInfo>();

			foreach(CombatUnit unit in attackerUnit.Keys)
			{
				int lostUnit = Mathf.Clamp((int)(ratio * attackerUnit[unit]), 1, attackerUnit[unit]);

				UnitLostInfo info = new UnitLostInfo(unit.unitType, attackerUnit[unit], lostUnit);

				//return unit
				data.AddCombatUnit(unit.unitType ,info.to);

				attackerLostInfo.Add(info);


			}

			//calculate unit lost for defender
			List<UnitLostInfo> targetLostInfo = new List<UnitLostInfo>();

			foreach(CombatUnit unit in targetUnit.Keys)
			{
				float penalty = Mathf.Clamp((1-ratio)*10f, 1f, 10f);

				int lostUnit = Mathf.Clamp((int)(ratio * penalty * targetUnit[unit]), 1, targetUnit[unit]);

				//TODO:Decrease ai player unit

				targetLostInfo.Add(new UnitLostInfo(unit.unitType, targetUnit[unit], lostUnit));
			}

			result = AttackResult.Win;

			reportData.AddBattleReport(result, targetName, "Player", attackerLostInfo, targetLostInfo, resourceAward);

			//add resource
			PlayerResourceStorageMetaData pData = PlayerResourceStorageMetaData.Load();

			foreach(ResourceType type in resourceAward.Keys)
			{
				pData.AddResourceForType(type, resourceAward[type]);
			}
		}
		else if((fAttackerSum - fDefenderSum) < 0)//attacker defeat, defender win
		{
			Debug.Log("Attacker "+GetAttackerSum()+" vs "+targetName+" "+GetDefenderSum()+" fail");

			float ratio = (float)fAttackerSum * 100f / (float)fDefenderSum / 100f;

			CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load();

			//calculate unit lost for attacker
			List<UnitLostInfo> attackerLostInfo = new List<UnitLostInfo>();

			foreach(CombatUnit unit in attackerUnit.Keys)
			{
				float penalty = Mathf.Clamp((1-ratio)*10f, 1f, 10f);

				int lostUnit = Mathf.Clamp((int)(ratio * penalty * attackerUnit[unit]), 1, attackerUnit[unit]);

				UnitLostInfo info = new UnitLostInfo(unit.unitType, attackerUnit[unit], lostUnit);

				//return unit
				data.AddCombatUnit(unit.unitType, info.to);

				attackerLostInfo.Add(info);
			}

			//calculate unit lost for defender
			List<UnitLostInfo> targetLostInfo = new List<UnitLostInfo>();

			foreach(CombatUnit unit in targetUnit.Keys)
			{
				int lostUnit = Mathf.Clamp((int)(ratio * targetUnit[unit]), 1, targetUnit[unit]);
				
				//TODO:Decrease ai player unit
				
				targetLostInfo.Add(new UnitLostInfo(unit.unitType, targetUnit[unit], lostUnit));
			}

			result = AttackResult.Lost;

			reportData.AddBattleReport(result, targetName, "Player", attackerLostInfo, targetLostInfo);
		}
		else//draw
		{
			Debug.Log("Attacker "+GetAttackerSum()+" vs "+targetName+" "+GetDefenderSum()+" draw");

			float ratio = (float)fAttackerSum * 100f / (float)fDefenderSum / 100f * 0.5f;

			CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load();

			//calculate unit lost for attacker
			List<UnitLostInfo> attackerLostInfo = new List<UnitLostInfo>();

			foreach(CombatUnit unit in attackerUnit.Keys)
			{
				int lostUnit = Mathf.Clamp((int)(ratio * 0.3f * attackerUnit[unit]), 1, attackerUnit[unit]);

				UnitLostInfo info = new UnitLostInfo(unit.unitType, attackerUnit[unit], lostUnit);

				//return unit
				data.AddCombatUnit(unit.unitType, info.to);

				attackerLostInfo.Add(info);
			}

			//calculate unit lost for defender
			List<UnitLostInfo> targetLostInfo = new List<UnitLostInfo>();

			foreach(CombatUnit unit in targetUnit.Keys)
			{
				int lostUnit = Mathf.Clamp((int)(ratio * 0.3f * targetUnit[unit]), 1, targetUnit[unit]);
				
				//TODO:Decrease ai player unit
				
				targetLostInfo.Add(new UnitLostInfo(unit.unitType, targetUnit[unit], lostUnit));
			}

			result = AttackResult.Draw;

			reportData.AddBattleReport(result, targetName, "Player", attackerLostInfo, targetLostInfo);
		}

		
	}

	/// <summary>
	/// Gets the attacker damage sum.
	/// </summary>
	/// <returns>The attacker sum.</returns>
	int GetAttackerSum()
	{
		int retVal = 0;

		foreach(CombatUnit unit in attackerUnit.Keys)
		{
			retVal += (unit.GetDamage(GetAllUnitType(targetUnit.Keys)) * attackerUnit[unit]);
		}

		return retVal;
	}

	/// <summary>
	/// Gets the defender defence sum.
	/// </summary>
	/// <returns>The defender sum.</returns>
	int GetDefenderSum()
	{
		int retVal = 0;

		foreach(CombatUnit unit in targetUnit.Keys)
		{
			retVal += (unit.GetDefence(GetAllUnitType(attackerUnit.Keys)) * targetUnit[unit]);
		}

		return retVal;
	}

	/// <summary>
	/// Gets the type of the all unit.
	/// </summary>
	/// <returns>The all unit type.</returns>
	/// <param name="unitKeys">Unit keys.</param>
	CombatUnitType[] GetAllUnitType(Dictionary<CombatUnit, int>.KeyCollection unitKeys)
	{
		CombatUnitType[] retVal = new CombatUnitType[unitKeys.Count];

		int index = 0;

		foreach(CombatUnit theUnit in unitKeys)
		{
			retVal[index] = theUnit.unitType;

			++index;
		}

		return retVal;
	}
}

/// <summary>
/// This class AttackManagerMetaData is automatic generated.
/// 
/// Implement your persistant data calss here
/// </summary>
[Serializable]
public class AttackManagerMetaData : PersistantMetaData
{
	List<AttackInfo> _trackAttackTask = new List<AttackInfo>();

	List<AttackInfo> _completeAttackTask = new List<AttackInfo>();

	public static AttackManagerMetaData Load(bool processing = false)
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<AttackManagerMetaData>())
		{
			AttackManagerMetaData newData = new AttackManagerMetaData();
			
			newData.Save();
			
			return newData;
		}
		
		AttackManagerMetaData data = SaveLoadManager.SharedManager.Load<AttackManagerMetaData> ();
		
		if(processing)
		{
			data.ProcessData ();
			
			data.Save();
		}
		
		
		return data;
	}

	void ProcessData()
	{
		List<AttackInfo> removedInfo = new List<AttackInfo>();

		for(int i=0; i<_trackAttackTask.Count; i++)
		{
			AttackInfo attackInfo = _trackAttackTask[i];

			int result = DateTime.Now.CompareTo(attackInfo.taskEndTime);

			if(result >= 0)
			{
				removedInfo.Add(attackInfo);

				_completeAttackTask.Add(attackInfo);

				attackInfo.CalculateResult();
			}
			else if(result < 0)
			{
				TimeSpan tSpan = attackInfo.taskEndTime.Subtract(DateTime.Now);
				
				int remainSeconds = (int)(tSpan.TotalSeconds%attackInfo.duration);

				attackInfo.duration = remainSeconds;
			}
		}

		for(int i=0; i<removedInfo.Count; i++)
		{
			_trackAttackTask.Remove(removedInfo[i]);
		}

		removedInfo.Clear ();
	}

	/// <summary>
	/// Finds the attack task by id.
	/// </summary>
	/// <returns>The attack task.</returns>
	/// <param name="tskId">Tsk identifier.</param>
	AttackInfo FindAttackTask(string tskId)
	{
		foreach(AttackInfo info in _trackAttackTask)
		{
			if(info.taskId == tskId)
			{
				return info;
			}
		}

		return null;
	}

	/// <summary>
	/// Finds the attack task in complete.
	/// </summary>
	/// <returns>The attack task in complete.</returns>
	/// <param name="tskId">Tsk identifier.</param>
	AttackInfo FindAttackTaskInComplete(string tskId)
	{
		foreach(AttackInfo info in _completeAttackTask)
		{
			if(info.taskId == tskId)
			{
				return info;
			}
		}

		return null;
	}

	/// <summary>
	/// Gets all attack info.
	/// </summary>
	/// <returns>The attack info.</returns>
	public AttackInfo[] GetAttackInfo()
	{
		return _trackAttackTask.ToArray();
	}

	/// <summary>
	/// Adds the attack task.
	/// </summary>
	/// <returns><c>true</c>, if attack task was added, <c>false</c> otherwise.</returns>
	/// <param name="tskId">Tsk identifier.</param>
	/// <param name="tId">T identifier.</param>
	/// <param name="tName">T name.</param>
	/// <param name="tUnit">T unit.</param>
	/// <param name="aUnit">A unit.</param>
	/// <param name="taskDuration">Task duration.</param>
	public bool AddAttackTask(string tskId ,string tId, string tName, Dictionary<CombatUnit, int> tUnit, Dictionary<CombatUnit, int> aUnit, int taskDuration,
	                          Dictionary<ResourceType, float> resourceAward)
	{

		CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load ();

		foreach(CombatUnit unit in aUnit.Keys)
		{
			if(!data.DecreaseCombatUnit(unit.unitType, aUnit[unit]))
			{
				Debug.LogError("Unable to decrease combat unit of type "+unit.unitType.ToString());

				return false;
			}
		}


		AttackInfo newInfo = new AttackInfo(tskId, tId, tName, tUnit, aUnit, taskDuration, resourceAward);

		_trackAttackTask.Add (newInfo);

		Save ();

		return true;
	}

	/// <summary>
	/// Cancel attack by id.
	/// </summary>
	/// <returns><c>true</c> if this instance cancel attack the specified tskId; otherwise, <c>false</c>.</returns>
	/// <param name="tskId">Tsk identifier.</param>
	public bool CancelAttack(string tskId)
	{
		AttackInfo attackInfo = FindAttackTask (tskId);

		if(attackInfo != null)
		{
			CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load();


			foreach(CombatUnit unit in attackInfo.attackerUnit.Keys)
			{
				data.AddCombatUnit(unit.unitType, attackInfo.attackerUnit[unit]);
			}

			_trackAttackTask.Remove(attackInfo);

			Save();

			return true;
		}

		attackInfo = FindAttackTaskInComplete (tskId);

		if(attackInfo != null)
		{
			_completeAttackTask.Remove(attackInfo);

			Save();

			return true;
		}

		return false;

	}

	/// <summary>
	/// Check if is attack task complete.
	/// return Attack info if complete otherwise null
	/// </summary>
	/// <returns><c>true</c> if this instance is attack task complete the specified tskId; otherwise, <c>false</c>.</returns>
	/// <param name="tskId">Tsk identifier.</param>
	public AttackInfo IsAttackTaskComplete(string tskId)
	{
		AttackInfo attackInfo = FindAttackTask (tskId);

		if(attackInfo != null)
		{
			int result = DateTime.Now.CompareTo(attackInfo.taskEndTime);
			
			if(result >= 0)
			{	
				attackInfo.CalculateResult();

				_completeAttackTask.Add(attackInfo);

				_trackAttackTask.Remove(attackInfo);

				Save();

				return attackInfo;
			}

			return null;
		}


		attackInfo = FindAttackTaskInComplete (tskId);

		if(attackInfo != null)
		{
			return attackInfo;
		}

		Debug.LogError ("No attack task exist " + tskId);

		return null;
	}

	public void Save()
	{
		SaveLoadManager.SharedManager.Save <AttackManagerMetaData>(this);
	}
}