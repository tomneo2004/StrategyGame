using UnityEngine;
using System.Collections;

/// <summary>
/// Event when present action confirm view(Dialog).
/// </summary>
public class EventPresentActionConfirm : GameEvent 
{
	public string title;

	public string message;

	public string confirmBtn;

	public string cancelBtn;

	public UIActionComfirm.OnButtonClick callback;

	public EventPresentActionConfirm(string theTitle, string msg, string okButton = "OK", string cancelButton = null, UIActionComfirm.OnButtonClick theCallBack = null)
	{
		title = theTitle;
		message = msg;
		confirmBtn = okButton;
		cancelBtn = cancelButton;
		callback = theCallBack;
	}
}
