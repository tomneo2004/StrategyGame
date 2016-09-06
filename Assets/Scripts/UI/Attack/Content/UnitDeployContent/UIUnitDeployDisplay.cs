using UnityEngine;
using System.Collections;

public class UIUnitDeployDisplay : MonoBehaviour 
{
	public UISprite unitAvatar;

	public UIValueSlider valueSlider;

	CombatUnitType _unitType;

	int _outputValue;

	public int UnitDeployNumber
	{
		get
		{
			return _outputValue;
		}
	}

	public delegate void OnUnitDeployNumberChange(CombatUnitType unitType, int unitNumber);
	public OnUnitDeployNumberChange Evt_OnUnitDeployNumberChange;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SetInfo(CombatUnitType type, int unitNumber)
	{
		_unitType = type;

		unitAvatar.spriteName = ImageManager.Instance.CombatUnitSpriteNameForHead(type);

		valueSlider.Evt_OnValueChange = OnValueChange;

		valueSlider.SetupValueSlider (unitNumber, unitNumber);
	}

	void OnValueChange(UIValueSlider slider, float output)
	{
		_outputValue = (int)output;

		if(Evt_OnUnitDeployNumberChange != null)
		{
			Evt_OnUnitDeployNumberChange(_unitType, (int)output);
		}
	}
}
