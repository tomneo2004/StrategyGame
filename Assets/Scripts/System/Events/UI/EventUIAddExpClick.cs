using UnityEngine;
using System.Collections;

/// <summary>
/// Event when UI add exp click Test only.
/// </summary>
public class EventUIAddExpClick : GameEvent 
{
	public int expToAdd;

	public EventUIAddExpClick(int exp)
	{
		expToAdd = exp;
	}
}
