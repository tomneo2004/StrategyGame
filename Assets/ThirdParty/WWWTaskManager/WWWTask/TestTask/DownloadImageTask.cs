using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DownloadImageTask : WWWTask
{
	public delegate void OnCompleteDownloading(Texture image);
	public OnCompleteDownloading Evt_OnCompleteDownloading;

	/// <summary>
	/// call when request start.
	/// </summary>
	/// <param name="url">URL.</param>
	protected override void OnRequestStart(string url)
	{
		base.OnRequestStart (url);

		Debug.Log("Start downloading image");
	}
	
	/// <summary>
	/// call when request progress.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="form">Form.</param>
	/// <param name="postFormData">Post form data.</param>
	/// <param name="progress">Progress.</param>
	protected override void OnRequestProgress(ref WWW request, ref WWWForm form, Dictionary<string, WWWFieldData> postFormData, float progress)
	{
		base.OnRequestProgress (ref request, ref form, postFormData, progress);

		Debug.Log ("Task "+_taskId+" downloading image " + progress + "%");
	}
	
	/// <summary>
	/// call when request complete.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="form">Form.</param>
	/// <param name="postFormData">Post form data.</param>
	protected override void OnRequestComplete(ref WWW request, ref WWWForm form, Dictionary<string, WWWFieldData> postFormData)
	{
		base.OnRequestComplete (ref request, ref form, postFormData);

		Debug.Log("Task "+_taskId+" download image complete");

		if(Evt_OnCompleteDownloading != null)
		{
			Evt_OnCompleteDownloading(request.texture);
		}
	}

	/// <summary>
	/// call when request error.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="form">Form.</param>
	/// <param name="postFormData">Post form data.</param>
	/// <param name="error">Error.</param>
	protected override void OnRequestError(ref WWW request, ref WWWForm form, Dictionary<string, WWWFieldData> postFormData, string error)
	{
		base.OnRequestError (ref request, ref form, postFormData, error);

		Debug.Log("Task "+_taskId+" download image fail");
	}

}
