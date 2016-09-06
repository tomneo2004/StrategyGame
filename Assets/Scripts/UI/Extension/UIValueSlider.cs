using UnityEngine;
using System.Collections;

public class UIValueSlider : MonoBehaviour 
{
	float _maxValue = 0f;

	public float MaxValue
	{
		get
		{
			return _maxValue;
		}
	}

	float outputValue = 0f;

	public UILabel valueLabel;
	
	public UISlider valueSlider;
	
	public UIButton increaseBtn;
	
	public UIButton decreaseBtn;
	
	public float addSubVal = 1f;

	public bool displayAsInt = true;

	public delegate void OnValueChange(UIValueSlider slider, float output);
	public OnValueChange Evt_OnValueChange;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetupValueSlider(float maxVal, float beginVal)
	{
		_maxValue = maxVal;

		EventDelegate.Set (valueSlider.onChange, OnValueSliderChange);
		valueSlider.value = beginVal / maxVal;

		ChangeValue (beginVal);
	}

	/// <summary>
	/// On increase button press.
	/// </summary>
	public void OnIncrease()
	{
		float val = Mathf.Clamp (outputValue + addSubVal, 0f, _maxValue);

		ChangeValue (val);
	}

	/// <summary>
	/// On decrease button press.
	/// </summary>
	public void OnDecrease()
	{
		float val = Mathf.Clamp (outputValue + (-1f * addSubVal), 0f, _maxValue);

		ChangeValue (val);
	}

	void ChangeValue(float val)
	{
		outputValue = val;

		if(displayAsInt)
		{
			int intVal = (int)val;

			valueLabel.text = intVal.ToString();
		}
		else
		{
			valueLabel.text = val.ToString ();
		}


		//change button disable/enable
		if((outputValue > 0) && (outputValue < _maxValue))
		{
			increaseBtn.isEnabled = true;
			decreaseBtn.isEnabled = true;
		}
		else if(outputValue <= 0)
		{
			increaseBtn.isEnabled = true;
			decreaseBtn.isEnabled = false;
		}
		else
		{
			increaseBtn.isEnabled = false;
			decreaseBtn.isEnabled = true;
		}

		if(UISlider.current == null)
		{
			EventDelegate.Remove (valueSlider.onChange, OnValueSliderChange);
			
			valueSlider.value = val / _maxValue;
			
			EventDelegate.Set (valueSlider.onChange, OnValueSliderChange);
		}

		//respond event
		if(Evt_OnValueChange != null)
		{
			Evt_OnValueChange(this, outputValue);
		}
	}

	void OnValueSliderChange()
	{
		if(float.IsNaN(UISlider.current.value))
		{
			return;
		}
		
		float sliderVal = UISlider.current.value;
		
		float val = _maxValue * sliderVal;
		
		ChangeValue (val);
	}
}
