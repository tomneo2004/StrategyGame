using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


/// <summary>
/// This class CombatUnitManagerMetaData is automatic generated.
/// 
/// Implement your persistant data calss here
/// </summary>
[Serializable]
public class CombatUnitManagerMetaData : PersistantMetaData
{
	public int maxCombatUnit = 10000;

	private List<CombatUnitType> availableCombatUnit;

	private List<int> numberOfCombatUnit; 

	public int CurrentCombatUnit
	{
		get
		{
			int retVal = 0;

			if(availableCombatUnit != null)
			{
				for(int i=0; i<numberOfCombatUnit.Count; i++)
				{
					CombatUnit unit = CombatUnitManager.Instance.GetCombatUnitInfoByType(availableCombatUnit[i]);

					retVal += numberOfCombatUnit[i];

				}
			}

			return retVal;
		}
	}

	public bool producingUnit = false;

	public CombatUnitType producingUnitType;

	public int producingDuration = 10;

	public int producingAmount = 10;

	private DateTime producingStartTime;
	
	private DateTime producingEndTime;

	public static CombatUnitManagerMetaData Load(bool processing = false)
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<CombatUnitManagerMetaData>())
		{
			CombatUnitManagerMetaData newData = new CombatUnitManagerMetaData();

			newData.availableCombatUnit = new List<CombatUnitType>();
			newData.numberOfCombatUnit = new List<int>();
			
			newData.producingStartTime = DateTime.Now;
			newData.producingEndTime = DateTime.Now;
			
			newData.Save();
			
			return newData;
		}
		
		CombatUnitManagerMetaData data = SaveLoadManager.SharedManager.Load<CombatUnitManagerMetaData> ();
		
		if(processing)
		{
			data.ProcessData ();
			
			data.Save();
		}
		
		
		return data;
	}

	void ProcessData()
	{
		if(producingUnit)
		{
			int result = DateTime.Now.CompareTo(producingEndTime);

			//finish unfinished task
			if(result >= 0)
			{
				FinishProducingUnit();
			}
			else if(result < 0)
			{
				TimeSpan tSpan = producingEndTime.Subtract(DateTime.Now);

				int remainSeconds = (((int)tSpan.TotalSeconds) >= producingDuration)?producingDuration:(int)(tSpan.TotalSeconds%producingDuration);

				if(remainSeconds > 0)
				{
					producingUnit = true;
					
					producingDuration = remainSeconds;
				}
				else
				{
					producingUnit = false;
				}

			}
		}
	}

	/// <summary>
	/// Adds the combat unit.
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="produceAmount">Produce amount.</param>
	public void AddCombatUnit(CombatUnitType type, int produceAmount)
	{
		int addAmount = produceAmount;

		if((CurrentCombatUnit+addAmount) > maxCombatUnit)
		{
			Debug.LogError("Add combat unit overflow");

			addAmount = maxCombatUnit - CurrentCombatUnit;
		}

		for(int i=0; i<availableCombatUnit.Count; i++)
		{
			if(type == availableCombatUnit[i])
			{
				numberOfCombatUnit[i] += addAmount;

				Save();

				return;
			}
		}

		availableCombatUnit.Add (type);
		numberOfCombatUnit.Add (addAmount);

		Save ();
	}

	/// <summary>
	/// Decreases the combat unit.
	/// </summary>
	/// <returns><c>true</c>, if combat unit was decreased, <c>false</c> otherwise.</returns>
	/// <param name="type">Type.</param>
	/// <param name="decreaseAmount">Decrease amount.</param>
	public bool DecreaseCombatUnit(CombatUnitType type, int decreaseAmount)
	{
		if(numberOfCombatUnit.Count <= 0)
		{
			return false;
		}

		for(int i=0; i<availableCombatUnit.Count; i++)
		{
			if(type == availableCombatUnit[i])
			{
				if((numberOfCombatUnit[i] - decreaseAmount) >= 0)
				{
					numberOfCombatUnit[i] -= decreaseAmount;

				}
				else
				{
					numberOfCombatUnit[i] = 0;
				}

				Save();

				return true;
			}
		}

		Debug.LogError ("No unit type found " + type.ToString ());
		return false;
	}

	/*
	public bool DecreaseCombatUnit(int decreaseAmount)
	{
		if(numberOfCombatUnit.Count <= 0)
		{
			return false;
		}

		if((CurrentCombatUnit-decreaseAmount) < 0)
		{
			return false;
		}

		numberOfCombatUnit [0] -= decreaseAmount;

		Save ();

		return true;
	}
	*/

	/*
	public void AddCombatUnit(int amount)
	{
		if(numberOfCombatUnit.Count <= 0)
		{
			return;
		}

		int addAmount = amount;

		if((CurrentCombatUnit+amount) > maxCombatUnit)
		{
			addAmount = maxCombatUnit - CurrentCombatUnit;
		}

		numberOfCombatUnit [0] += addAmount;

		Save ();
	}
	*/

	/// <summary>
	/// Produces combat unit.
	/// </summary>
	/// <returns><c>true</c>, if combat unit was produced, <c>false</c> otherwise.</returns>
	/// <param name="unitType">Unit type.</param>
	/// <param name="produceAmount">Produce amount.</param>
	/// <param name="duration">Duration.</param>
	/// <param name="resourceCost">Resource cost.</param>
	public bool ProduceCombatUnit(CombatUnitType unitType, int produceAmount, int duration, Dictionary<ResourceType, float> resourceCost)
	{
		if(producingUnit)
		{
			return false;
		}

		PlayerResourceStorageMetaData data = PlayerResourceStorageMetaData.Load ();

		if(duration <=0)
		{
			AddCombatUnit(unitType, produceAmount);
			
			foreach(ResourceType type in resourceCost.Keys)
			{
				data.CostResource(type, produceAmount*resourceCost[type]);
			}

			Save();

			return true;
		}

		producingUnit = true;

		producingUnitType = unitType;

		producingAmount = produceAmount;

		producingDuration = duration;

		producingStartTime = DateTime.Now;
		producingEndTime = DateTime.Now.AddSeconds (producingDuration);

		//todo: cost resource
		foreach(ResourceType type in resourceCost.Keys)
		{
			data.CostResource(type, produceAmount*resourceCost[type]);
		}

		Save ();

		return true;
	}

	/// <summary>
	/// Is producing unit task complete.
	/// </summary>
	/// <returns><c>true</c> if this instance is produce unit complete; otherwise, <c>false</c>.</returns>
	public bool IsProduceUnitComplete()
	{
		if(producingUnit)
		{
			int result = DateTime.Now.CompareTo(producingEndTime);
			
			if(result >= 0)
			{
				AddCombatUnit(producingUnitType, producingAmount);
				
				producingUnit = false;
				
				this.Save();
				
				return true;
			}
		}
		
		this.Save ();
		
		return false;
	}

	/// <summary>
	/// Gets all unit with number.
	/// Filter true will filt out unit type that current number is 0
	/// </summary>
	/// <returns>The all unit.</returns>
	/// <param name="filter">If set to <c>true</c> filter.</param>
	public Dictionary<CombatUnitType, int> GetAllUnit(bool filter = true)
	{
		Dictionary<CombatUnitType, int> retVal = new Dictionary<CombatUnitType, int> ();

		for(int i=0; i<availableCombatUnit.Count; i++)
		{
			if(filter)
			{
				if(numberOfCombatUnit[i] != 0)
				{
					retVal.Add(availableCombatUnit[i], numberOfCombatUnit[i]);
				}
			}
			else
			{
				retVal.Add(availableCombatUnit[i], numberOfCombatUnit[i]);
			}

		}

		return retVal;
	}

	public int NumberOfUnitByType(CombatUnitType type)
	{
		for(int i=0; i<availableCombatUnit.Count; i++)
		{
			if(type == availableCombatUnit[i])
			{
				return numberOfCombatUnit[i];
			}
		}

		return 0;
	}

	public void FinishProducingUnit()
	{
		if(producingUnit)
		{
			AddCombatUnit(producingUnitType, producingAmount);
			
			producingUnit = false;

			Save();
		}
	}

	public void Save()
	{
		SaveLoadManager.SharedManager.Save <CombatUnitManagerMetaData>(this);
	}
}