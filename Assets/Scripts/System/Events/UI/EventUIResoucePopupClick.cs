using UnityEngine;
using System.Collections;

/// <summary>
/// Event UI resouce popup click.
/// If building ready to be collection it popup icon.
/// Event get fired when popup icon click.
/// </summary>
public class EventUIResoucePopupClick : GameEvent 
{

	Transform _target;

	public Transform GetTarget
	{
		get
		{
			return _target;
		}
	}

	UIButton _popupBtn;

	public UIButton GetUIButton
	{
		get
		{
			return _popupBtn;
		}
	}

	public EventUIResoucePopupClick(Transform tans, UIButton btn)
	{
		_target = tans;
		_popupBtn = btn;
	}
}
