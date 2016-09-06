using UnityEngine;
using System.Collections;

/// <summary>
/// Event when present alert view.
/// </summary>
public class EventAlert : GameEvent 
{
	public string alertTitle;

	public string alertContent;

	public EventAlert(string title, string content)
	{
		alertTitle = title;
		alertContent = content;
	}

}
