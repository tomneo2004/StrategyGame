using UnityEngine;
using System.Collections;

public class UIAlertController : MonoBehaviour 
{
	public GameObject container;

	public UILabel title;

	public UILabel content;

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventAlert> (ShowAlert);
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventAlert> (ShowAlert);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Closes the alert.
	/// </summary>
	public void CloseAlert()
	{
		container.SetActive (false);
	}

	/// <summary>
	/// Handle show alert event
	/// </summary>
	/// <param name="e">E.</param>
	void ShowAlert(EventAlert e)
	{
		container.SetActive (true);

		title.text = e.alertTitle;
		content.text = e.alertContent;
	}
}
