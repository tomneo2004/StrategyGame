using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TweenAlpha))]
public class UITweenAlphaButton : MonoBehaviour 
{
	public UISprite notifySprite;

	[Range(0.1f, 1.0f)]
	public float tweenReverseFactor = 0.5f;

	TweenAlpha tweenAlpha;

	float duration = 1f;

	protected virtual void Awake()
	{
		tweenAlpha = GetComponent<TweenAlpha> ();

		duration = tweenAlpha.duration;

		EventDelegate.Set (tweenAlpha.onFinished, OnTweenFinished);

	}

	// Use this for initialization
	protected virtual void Start () 
	{
		
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{

		if(tweenAlpha.direction == AnimationOrTween.Direction.Reverse)
		{
			tweenAlpha.duration = duration * tweenReverseFactor;

		}
		else if(tweenAlpha.direction == AnimationOrTween.Direction.Forward)
		{
			tweenAlpha.duration = duration;

		}
	}

	void OnTweenFinished()
	{
		if(tweenAlpha.direction == AnimationOrTween.Direction.Forward)
		{
			tweenAlpha.duration = duration * tweenReverseFactor;


		}
		else if(tweenAlpha.direction == AnimationOrTween.Direction.Reverse)
		{
			tweenAlpha.duration = duration;

		}

	}

	/// <summary>
	/// Handle click event.
	/// </summary>
	public virtual void OnClick()
	{
		if(notifySprite != null)
		{
			notifySprite.gameObject.SetActive(false);
		}
	}
	/// <summary>
	/// Handle notify event.
	/// </summary>
	public virtual void OnNotify()
	{
		if(notifySprite != null)
		{
			notifySprite.gameObject.SetActive(true);
		}
	}
}
