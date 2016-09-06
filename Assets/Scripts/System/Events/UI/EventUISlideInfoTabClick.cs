using UnityEngine;
using System.Collections;

/// <summary>
/// Event UI slide info tab click.
/// </summary>
public class EventUISlideInfoTabClick : GameEvent 
{

	private GameObject _clickedTab;

	public GameObject CurrentTab
	{
		get
		{
			return _clickedTab;
		}
	}

	public EventUISlideInfoTabClick(GameObject tab)
	{
		_clickedTab = tab;
	}
}
