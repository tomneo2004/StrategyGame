using UnityEngine;
using System.Collections;

public class UIButtonPressEvent : MonoBehaviour 
{
	[Tooltip("The class that will be sent event to")]
	public MonoBehaviour behaviour;

	[Tooltip("Function name inside class that will be called")]
	public string functionName;

	[Tooltip("Fire event frequency")]
	public float eventFireDuration = 0.5f;

	[Tooltip("Time greater than this will start to fire event")]
	public float delayBegin = 0.5f;

	bool _Pressed = false;//if pressed 

	bool _fireEvent = false;//if is firing event

	float _timeElapsed = 0.0f;//fire event time pass

	float _DelayTimeElapsed = 0.0f;//delay fire event time pass

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//handle delay begin
		if(_Pressed && !_fireEvent)
		{
			if(_DelayTimeElapsed >= delayBegin)
			{
				_fireEvent = true;

				_DelayTimeElapsed = 0.0f;
			}
			else
			{
				_DelayTimeElapsed += Time.deltaTime;
			}
		}

		//handle pressed
		if(_fireEvent)
		{
			if(_timeElapsed >= eventFireDuration)
			{
				FireEvent();

				_timeElapsed = 0.0f;
			}
			else
			{
				_timeElapsed += Time.deltaTime;
			}
		}
	}

	void FireEvent()
	{
		if((behaviour != null) && !string.IsNullOrEmpty(functionName))
		{
			behaviour.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnPress(bool pressed)
	{
		if(pressed)
		{
			_Pressed = true;

		}
		else
		{
			//if it is click and not pressed
			if(_Pressed && (_DelayTimeElapsed < delayBegin))
			{
				FireEvent();
			}

			_Pressed = false;

			_fireEvent = false;

			_timeElapsed = 0.0f;

			_DelayTimeElapsed = 0.0f;
		}
	}


}
