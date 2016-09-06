using UnityEngine;
using System.Collections;

/// <summary>
/// Event when player level exp get update.
/// </summary>
public class EventUpdateLevelExp : GameEvent 
{
	
	public int currentLevel;

	public int currentExp;

	public int expToNextLevel;

	public EventUpdateLevelExp(int curLevel, int curExp, int nextlevelExp)
	{
		currentLevel = curLevel;
		currentExp = curExp;
		expToNextLevel = nextlevelExp;
	}
}
