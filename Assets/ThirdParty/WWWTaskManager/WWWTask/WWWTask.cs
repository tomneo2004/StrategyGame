using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

public class WWWFieldData
{
	private string _stringVal;
	private int _intVal;
	private byte[] _byteVal;

	System.Type _typeDef = null;

	string _fieldName;

	/// <summary>
	/// Get the define type of value.
	/// </summary>
	/// <value>The type of the define.</value>
	public System.Type DefineType
	{
		get
		{
			return _typeDef;
		}
	}

	/// <summary>
	/// Set value.
	/// string, int, byte[] support
	/// </summary>
	/// <param name="val">Value.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public void SetValue<T>(T val)
	{
		FieldInfo[] allInfos = this.GetType ().GetFields (BindingFlags.NonPublic | BindingFlags.Instance);

		foreach(FieldInfo f in allInfos)
		{
			if(f.FieldType.Equals(val.GetType()) && f.Name.Contains("_") && f.Name.Contains("Val"))
			{
				_fieldName = f.Name;

				_typeDef = typeof(T);

				f.SetValue(this, val);

				return;
			}
		}

		_fieldName = null;

		Debug.LogError ("Can not find field for type "+typeof(T).ToString());
	}

	/// <summary>
	/// Get value.
	/// return string, int, byte[]
	/// </summary>
	/// <returns>The valid value.</returns>
	public object GetValidValue()
	{
		if((!string.IsNullOrEmpty(_fieldName)) && (_typeDef != null))
		{
			FieldInfo info = this.GetType ().GetField (_fieldName, BindingFlags.NonPublic|BindingFlags.Instance);

			return info.GetValue(this);
		}

		return null;
	}
}

public class WWWTask 
{
	protected WWWTaskManager _manager;

	/// <summary>
	/// set WWWTaskManager.
	/// </summary>
	/// <value>The manager.</value>
	public WWWTaskManager Manager
	{
		get
		{
			return _manager;
		}
	}
	/// <summary>
	/// Unique id of this task.
	/// </summary>
	protected string _taskId;

	/// <summary>
	/// get/set unique id of this task
	/// </summary>
	/// <value>The task identifier.</value>
	public string TaskId
	{
		get
		{
			return _taskId;
		}

		set
		{
			_taskId = value;
		}
	}

	protected bool _isDone = false;

	/// <summary>
	/// Whether or not this task is done www process.
	/// </summary>
	/// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
	public bool IsDone
	{
		get
		{
			return _isDone;
		}
	}

	/// <summary>
	/// The current URL request.
	/// </summary>
	string currentUrlRequest = "";

	/// <summary>
	/// The form data that follow the current request.
	/// </summary>
	Dictionary<string, WWWFieldData> formData;

	public delegate void OnTaskComplete(WWWTask task);
	/// <summary>
	/// Notify when task complete.
	/// </summary>
	public OnTaskComplete Evt_OnTaskComplete;

	~WWWTask()
	{
		_manager = null;
	}

	public virtual void InitTask(string taskId, WWWTaskManager manager)
	{
		_taskId = taskId;
		_manager = manager;
		_isDone = false;
	}

	/// <summary>
	/// Create  a new form data.
	/// </summary>
	/// <returns>The form data.</returns>
	public static Dictionary<string, WWWFieldData> CreateFormData()
	{
		return new Dictionary<string, WWWFieldData> ();
	}

	/// <summary>
	/// Add a value to form data.
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void AddValueToFormData<T>(ref Dictionary<string, WWWFieldData> data, string key, T value)
	{
		WWWFieldData fData = new WWWFieldData ();

		fData.SetValue<T> (value);

		data.Add (key, fData);
	}

	/// <summary>
	/// Remove a value from form data.
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="key">Key.</param>
	public static void RemoveValueFromFormData(ref Dictionary<string, WWWFieldData> data, string key)
	{
		if(data.ContainsKey(key))
		{
			data.Remove(key);
		}
	}

	/// <summary>
	/// Start a WWW request.
	/// </summary>
	/// <param name="urlRequest">URL request.</param>
	/// <param name="data"> the field data.</param>
	public void StartWWWRequest(string urlRequest, Dictionary<string, WWWFieldData> data = null)
	{
		if(!string.IsNullOrEmpty(currentUrlRequest))
		{
			Debug.LogError("You can not start a www request in another www request");

			return;
		}

		if(string.IsNullOrEmpty(urlRequest))
		{
			Debug.LogError("You can not specify empty url request");

			return;
		}

		formData = data;

		currentUrlRequest = urlRequest;

		OnRequestStart (currentUrlRequest);

		_manager.StartCoroutine (ProcessWWWRequest());
	}

	/// <summary>
	/// Processes the WWW request.
	/// </summary>
	/// <returns>The WWW request.</returns>
	protected IEnumerator ProcessWWWRequest()
	{
		WWW request = null;
		WWWForm form = null;

		//handle field data
		if((formData != null) && (formData.Count > 0))
		{
			form = new WWWForm();

			foreach(string key in formData.Keys)
			{
				if(formData[key].DefineType == typeof(string))
				{
					form.AddField(key, (string)formData[key].GetValidValue());
				}
				else if(formData[key].DefineType == typeof(int))
				{
					form.AddField(key, (int)formData[key].GetValidValue());
				}
				else if(formData[key].DefineType == typeof(byte))
				{
					form.AddBinaryData(key, (byte[])formData[key].GetValidValue());
				}
			}
		}

		//make request
		if(form != null)
		{
			request = new WWW(currentUrlRequest, form);
		}
		else
		{
			request = new WWW(currentUrlRequest);
		}

		//check if request is done
		while(!request.isDone)
		{
			OnRequestProgress(ref request, ref form, formData, request.progress);

			yield return request;
		}

		//check if there is an error
		if(string.IsNullOrEmpty(request.error))
		{
			OnRequestComplete(ref request, ref form, formData);
		}
		else
		{
			OnRequestError(ref request, ref form, formData, request.error);
		}

		currentUrlRequest = "";
		formData = null;
	}

	/// <summary>
	/// call when request start.
	/// </summary>
	/// <param name="url">URL.</param>
	protected virtual void OnRequestStart(string url)
	{

	}

	/// <summary>
	/// call when request progress.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="form">Form.</param>
	/// <param name="postFormData">Post form data.</param>
	/// <param name="progress">Progress.</param>
	protected virtual void OnRequestProgress(ref WWW request, ref WWWForm form, Dictionary<string, WWWFieldData> postFormData, float progress)
	{

	}

	/// <summary>
	/// call when request complete.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="form">Form.</param>
	/// <param name="postFormData">Post form data.</param>
	protected virtual void OnRequestComplete(ref WWW request, ref WWWForm form, Dictionary<string, WWWFieldData> postFormData)
	{
		TaskComplete ();
	}

	/// <summary>
	/// call when request error.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="form">Form.</param>
	/// <param name="postFormData">Post form data.</param>
	/// <param name="error">Error.</param>
	protected virtual void OnRequestError(ref WWW request, ref WWWForm form, Dictionary<string, WWWFieldData> postFormData, string error)
	{
		TaskComplete ();
	}

	public virtual void OnTaskAdded()
	{
		
	}

	public virtual void OnTaskRemove()
	{
		_manager.StopCoroutine (ProcessWWWRequest ());
	}

	/// <summary>
	/// Call this TaskComplete() when task complete.
	/// </summary>
	private void TaskComplete()
	{
		_manager.StopCoroutine (ProcessWWWRequest ());

		if(Evt_OnTaskComplete != null)
		{
			Evt_OnTaskComplete(this);
		}

		_isDone = true;
	}
}
