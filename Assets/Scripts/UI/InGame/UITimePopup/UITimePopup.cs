using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIFollowTarget))]
public class UITimePopup : MonoBehaviour 
{
	public UILabel timeLabel;

	public UISprite avatarSprite;

	public UISlider progressBar;

	private UIAtlas _originalAtlas;

	private UIFollowTarget _follow;

	void Awake()
	{
		_follow = GetComponent<UIFollowTarget> ();
	}

	// Use this for initialization
	void Start () 
	{
		_originalAtlas = avatarSprite.atlas;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void FollowTarget(Transform target)
	{
		if(_follow != null)
		{
			_follow.Target = target;
		}
	}

	/// <summary>
	/// Dispaly time.
	/// currentSecond:your current task progress in seconds
	/// totalSecond:how long is your task in seconds
	/// atlas:atlas that contain avatar for this task, null used original atlas
	/// avatarSpriteName:your avatar's name for this task
	/// </summary>
	/// <param name="currentSecond">Current second.</param>
	/// <param name="totalSecond">Total second.</param>
	/// <param name="avatarSpriteName">Avatar sprite name.</param>
	public void DisplayTime(int currentSecond, int totalSecond, string avatarSpriteName = null)
	{
		if(timeLabel != null)
		{

			timeLabel.text = TimeConverter.SecondToTimeString(currentSecond);

			if(!string.IsNullOrEmpty(avatarSpriteName))
			{
				avatarSprite.spriteName = avatarSpriteName;
			}

			if(progressBar != null)
			{
				progressBar.value = (float)(totalSecond - currentSecond) / (float)totalSecond;
			}
		}
		else
		{
			Debug.LogError(gameObject.name+" no label to display time");
		}
	}
}
