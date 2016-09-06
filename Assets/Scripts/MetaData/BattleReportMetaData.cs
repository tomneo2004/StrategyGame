using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class UnitLostInfo
{
	public CombatUnitType unitType;

	/// <summary>
	/// original unit number
	/// </summary>
	public int from = 0;

	/// <summary>
	/// the number of lost unit
	/// </summary>
	public int lost = 0;

	/// <summary>
	/// unit number after lost
	/// </summary>
	public int to = 0;

	public UnitLostInfo(CombatUnitType type, int originalUnit, int unitLost)
	{
		unitType = type;
		from = originalUnit;
		lost = unitLost;
		to = Mathf.Clamp (from - lost, 0, from);
	}
}

[Serializable]
public class BattleReportInfo
{
	public string infoId;

	public string BattleResultStr
	{
		get
		{
			if(battleResut == AttackResult.Win)
			{
				return "Win";
			}
			else if(battleResut == AttackResult.Lost)
			{
				return "Defeat";
			}
			else
			{
				return "Draw";
			}
		}
	}

	public AttackResult battleResut;

	public string attackerName;

	public string targetName;

	public List<UnitLostInfo> attackerUnitLostInfo;

	public List<UnitLostInfo> targetUnitLostInfo;

	public Dictionary<ResourceType, float> resourceAward;

	public bool isRead = false;

	/// <summary>
	/// Gets the get attacker total sent unit number.
	/// </summary>
	/// <value>The get attacker sent unit number.</value>
	public int GetAttackerSentUnitNumber
	{
		get
		{
			int retVal = 0;

			for(int i=0; i<attackerUnitLostInfo.Count; i++)
			{
				retVal += attackerUnitLostInfo[i].from;
			}

			return retVal;
		}
	}

	/// <summary>
	/// Gets the get attacker remain unit number.
	/// </summary>
	/// <value>The get attacker remain unit number.</value>
	public int GetAttackerRemainUnitNumber
	{
		get
		{
			int retVal = 0;

			for(int i=0; i<attackerUnitLostInfo.Count; i++)
			{
				retVal += attackerUnitLostInfo[i].to;
			}

			return retVal;
		}
	}

	/// <summary>
	/// Gets the get target sent unit number.
	/// </summary>
	/// <value>The get target sent unit number.</value>
	public int GetTargetSentUnitNumber
	{
		get
		{
			int retVal = 0;

			for(int i=0; i<targetUnitLostInfo.Count; i++)
			{
				retVal += targetUnitLostInfo[i].from;
			}

			return retVal;
		}
	}

	/// <summary>
	/// Gets the get target remain unit number.
	/// </summary>
	/// <value>The get target remain unit number.</value>
	public int GetTargetRemainUnitNumber
	{
		get
		{
			int retVal = 0;

			for(int i=0; i<targetUnitLostInfo.Count; i++)
			{
				retVal += targetUnitLostInfo[i].to;
			}

			return retVal;
		}
	}

	public BattleReportInfo(string id, AttackResult result, string tName, string aName, List<UnitLostInfo> attackerUnitLost, List<UnitLostInfo> targetUnitLost, 
	                        Dictionary<ResourceType, float> theResourceAward)
	{
		infoId = id;
		battleResut = result;
		targetName = tName;
		attackerName = aName;
		attackerUnitLostInfo = attackerUnitLost;
		targetUnitLostInfo = targetUnitLost;
		resourceAward = theResourceAward;
		isRead = false;
	}
	
}

/// <summary>
/// This class BattleReportMetaData is automatic generated.
/// 
/// Implement your persistant data calss here
/// </summary>
[Serializable]
public class BattleReportMetaData : PersistantMetaData
{
	List<BattleReportInfo> _battleReports = new List<BattleReportInfo> ();

	public static BattleReportMetaData Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<BattleReportMetaData>())
		{
			BattleReportMetaData newData = new BattleReportMetaData();
			
			newData.Save();
			
			return newData;
		}
		
		BattleReportMetaData data = SaveLoadManager.SharedManager.Load<BattleReportMetaData> ();
		
		
		return data;
	}

	string GenerateReportId()
	{
		while(true)
		{
			bool isVaild = true;

			string newId = Guid.NewGuid().ToString();

			for(int i=0; i<_battleReports.Count; i++)
			{
				if(newId == _battleReports[i].infoId)
				{
					isVaild = false;

					break;
				}
			}

			if(isVaild)
			{
				return newId;
			}
		}
	}


	/// <summary>
	/// Adds the battle report.
	/// </summary>
	/// <param name="result">Result.</param>
	/// <param name="targetName">Target name.</param>
	/// <param name="attackerName">Attacker name.</param>
	/// <param name="attackerUnitLost">Attacker unit lost.</param>
	/// <param name="targetUnitLost">Target unit lost.</param>
	public void AddBattleReport(AttackResult result, string targetName, string attackerName, List<UnitLostInfo> attackerUnitLost, List<UnitLostInfo>targetUnitLost, 
	                            Dictionary<ResourceType, float> resourceAward = null)
	{
		if(resourceAward == null)
		{
			resourceAward = new Dictionary<ResourceType, float>();
		}

		BattleReportInfo info = new BattleReportInfo (GenerateReportId (), result, targetName, attackerName, attackerUnitLost, targetUnitLost, resourceAward);

		_battleReports.Add (info);

		Save ();

		EventManager.GetInstance ().ExecuteEvent<EventNewBattleReport> (new EventNewBattleReport ());


	}

	/// <summary>
	/// Mark specific report is read.
	/// </summary>
	/// <param name="reportId">Report identifier.</param>
	public void MarkRead(string reportId)
	{
		for(int i=0; i<_battleReports.Count; i++)
		{
			if(_battleReports[i].infoId == reportId)
			{
				_battleReports[i].isRead = true;

				Save();

				break;
			}
		}
	}

	/// <summary>
	/// Gets all battle report.
	/// </summary>
	/// <returns>The all battle report.</returns>
	/// <param name="reverse">If set to <c>true</c> reverse.</param>
	public BattleReportInfo[] GetAllBattleReport(bool reverse = true)
	{
		if(reverse)
		{
			BattleReportInfo[] retVal;

			_battleReports.Reverse();

			retVal = _battleReports.ToArray();

			_battleReports.Reverse();

			return retVal;
		}

		return _battleReports.ToArray ();


	}

	public void Save()
	{
		SaveLoadManager.SharedManager.Save <BattleReportMetaData>(this);
	}
}