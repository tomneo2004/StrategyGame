using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CombatUnit  
{
	public string unitName = "";

	public CombatUnitType unitType;

	public string unitDesc = "";
	
	public int generateDuration = 60;

	public int minDamage = 10;

	public int maxDamage = 20;

	public int minDefence = 10;

	public int maxDefence = 20;

	public List<CombatUnitType> unitAgainst;

	public List<CombatUnitType> fearUnit;

	public List<ResourceType> costResourceTypes;

	public List<float> costResources;

	/// <summary>
	/// Gets the get resource cost.
	/// return null if there is no resource type
	/// </summary>
	/// <value>The get resource cost.</value>
	public Dictionary<ResourceType, float> GetResourceCost
	{
		get
		{
			if(costResourceTypes.Count > 0)
			{
				Dictionary<ResourceType, float> retVal = new Dictionary<ResourceType, float>();

				for(int i=0; i<costResourceTypes.Count; i++)
				{
					retVal.Add(costResourceTypes[i], costResources[i]);
				}

				return retVal;
			}

			return null;
		}
	}

	public CombatUnit()
	{
		unitAgainst = new List<CombatUnitType> ();

		fearUnit = new List<CombatUnitType> ();

		costResourceTypes = new List<ResourceType> ();

		costResources = new List<float> ();
	}

	/// <summary>
	/// Get damage of this unit.
	/// </summary>
	/// <returns>The damage.</returns>
	/// <param name="fightUnitTypes">Fight unit types.</param>
	public int GetDamage(CombatUnitType[] fightUnitTypes)
	{
		bool findUnitAgainst = false;

		for(int i=0; i<fightUnitTypes.Length; i++)
		{
			if(unitAgainst.Contains(fightUnitTypes[i]))
			{
				findUnitAgainst = true;

				break;
			}
		}

	 	return findUnitAgainst?maxDamage:minDamage;
	}

	/// <summary>
	/// Get defence of this unit.
	/// </summary>
	/// <returns>The defence.</returns>
	/// <param name="fightUnitTypes">Fight unit types.</param>
	public int GetDefence(CombatUnitType[] fightUnitTypes)
	{
		bool findUnitAgainst = false;

		for(int i=0; i<fightUnitTypes.Length; i++)
		{
			if(unitAgainst.Contains(fightUnitTypes[i]))
			{
				findUnitAgainst = true;
				
				break;
			}
		}

		return findUnitAgainst?maxDefence:minDefence;
	}
}
