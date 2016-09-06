using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class FieldDataTest : MonoBehaviour 
{
	public string url;

	// Use this for initialization
	void Start () 
	{
		DownloadImageTask task;


		task = WWWTaskManager.Instance.CreateTask<DownloadImageTask> ();

		task.Evt_OnCompleteDownloading = OnCompleteDownloading;

		WWWTaskManager.Instance.AddTask (task);

		task.StartWWWRequest (url);

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnCompleteDownloading(Texture image)
	{
		GetComponent<UITexture> ().mainTexture = image;
	}

	void OnReceiveTextData(string text)
	{
		Debug.Log (text);
	}
}
