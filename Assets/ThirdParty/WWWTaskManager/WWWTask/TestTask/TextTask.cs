using UnityEngine;
using System.Collections;

public class TextTask : WWWTask 
{
	public delegate void OnReceivedData(string text);
	public OnReceivedData Evt_OnReceivedData;

	protected override void OnRequestStart (string url)
	{
		base.OnRequestStart (url);
	}

	protected override void OnRequestProgress (ref WWW request, ref WWWForm form, System.Collections.Generic.Dictionary<string, WWWFieldData> postFormData, float progress)
	{
		base.OnRequestProgress (ref request, ref form, postFormData, progress);
	}

	protected override void OnRequestComplete (ref WWW request, ref WWWForm form, System.Collections.Generic.Dictionary<string, WWWFieldData> postFormData)
	{
		base.OnRequestComplete (ref request, ref form, postFormData);

		if(Evt_OnReceivedData != null)
		{
			Evt_OnReceivedData(request.text);
		}
	}

	protected override void OnRequestError (ref WWW request, ref WWWForm form, System.Collections.Generic.Dictionary<string, WWWFieldData> postFormData, string error)
	{
		base.OnRequestError (ref request, ref form, postFormData, error);
	}

}
