using UnityEngine;
using System.Collections;

public class UISlideInfoButton : MonoBehaviour 
{
	public UITweener notifyIcon;

	// Use this for initialization
	void Start () 
	{
	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventNewBattleReport> (OnNewBattleReport);
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventNewBattleReport> (OnNewBattleReport);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Handle click event.
	/// </summary>
	public void OnClick()
	{
		notifyIcon.gameObject.SetActive (false);
	}

	/// <summary>
	/// Handle new battle report event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnNewBattleReport(EventNewBattleReport e)
	{
		notifyIcon.gameObject.SetActive (true);

		notifyIcon.ResetToBeginning ();
		notifyIcon.PlayForward ();
	}
}
