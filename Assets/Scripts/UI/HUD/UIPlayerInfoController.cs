using UnityEngine;
using System.Collections;

public class UIPlayerInfoController : MonoBehaviour 
{
	public UISlider expProgress;

	public UILabel expLabel;

	public UILabel levelLabel;

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventUpdateLevelExp> (OnUpdateLevelExp);
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventUpdateLevelExp> (OnUpdateLevelExp);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Handle update level exp event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnUpdateLevelExp(EventUpdateLevelExp e)
	{
		levelLabel.text = e.currentLevel.ToString ();

		expLabel.text = e.currentExp + "/" + e.expToNextLevel;

		expProgress.value = (float)e.currentExp / (float)e.expToNextLevel;
	}
}
