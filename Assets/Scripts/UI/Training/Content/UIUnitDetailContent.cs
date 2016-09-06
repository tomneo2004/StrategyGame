using UnityEngine;
using System.Collections;

public class UIUnitDetailContent : UINavigationContent 
{
	private CombatUnitType unitType;

	//public UISlider unitSlider;

	//public UILabel unitLabel;

	public UILabel unitLevelLabel;

	public UILabel descriptionLabel;

	public UIScrollView descScrollview;

	//public UIButton increaseBtn;

	//public UIButton decreaseBtn;

	public UILabel attackMaxLabel;

	public UILabel attackMinLabel;

	public UILabel defendMaxLabel;

	public UILabel defendMinLabel;

	public UILabel foodCostLabel;

	public UILabel woodCostLabel;

	public UILabel crystalLabel;

	public UILabel trainingTimeLabel;

	//public UILabel spaceRequireLabel;

	public UILabel strongAgainstLabel;

	public UILabel weakAgainstLabel;

	public UIValueSlider valueSlider;

	//[Tooltip("The value when increase/decrease producing unit")]
	//public int unitAddSubVal = 1;

	private int produceCount = 0;

	private CombatUnit unitInfo;
	

	public override void OnContentDisplay ()
	{
		base.OnContentDisplay ();

		CombatUnitManager mgr = ((UITrainingNavController)_navigationController).combatUnitManager;

		descriptionLabel.text = unitInfo.unitDesc;

		attackMaxLabel.text = unitInfo.maxDamage.ToString ();
		attackMinLabel.text = unitInfo.minDamage.ToString ();
		defendMaxLabel.text = unitInfo.maxDefence.ToString ();
		defendMinLabel.text = unitInfo.minDefence.ToString ();

		foodCostLabel.text = "0";
		woodCostLabel.text = "0";
		crystalLabel.text = "0";

		for(int i=0; i<unitInfo.costResourceTypes.Count; i++)
		{
			switch(unitInfo.costResourceTypes[i])
			{
			case ResourceType.Food:
				foodCostLabel.text = ((int)unitInfo.costResources[i]).ToString();
				break;

			case ResourceType.Wood:
				woodCostLabel.text = ((int)unitInfo.costResources[i]).ToString();
				break;

			case ResourceType.Crystal:
				crystalLabel.text = ((int)unitInfo.costResources[i]).ToString();
				break;
			}
		}

		trainingTimeLabel.text = TimeConverter.SecondToTimeString(unitInfo.generateDuration);

		strongAgainstLabel.text = unitInfo.unitAgainst [0].ToString ();

		weakAgainstLabel.text = unitInfo.fearUnit [0].ToString ();

		descScrollview.ResetPosition ();

		float val = (float)mgr.GetAvaliableUnitToProduceForType (unitType);

		valueSlider.SetupValueSlider (val, val);

		valueSlider.Evt_OnValueChange = OnValueSiderChange;

		/*
		EventDelegate.Set (unitSlider.onChange, OnUnitSliderChange);
		unitSlider.value = 1.0f;

		ChangeProduceCount (mgr.GetAvaliableUnitToProduceForType(unitType));
		*/	
	}

	void OnValueSiderChange(UIValueSlider sider, float output)
	{
		produceCount = (int)output;
	}

	/*
	void ChangeProduceCount(int val)
	{
		CombatUnitManager mgr = ((UITrainingNavController)_navigationController).combatUnitManager;

		produceCount = val;

		unitLabel.text = val.ToString ();

		//change button disable/enable
		if((produceCount > 0) && (produceCount < mgr.GetAvaliableUnitToProduceForType(unitType)))
		{
			increaseBtn.isEnabled = true;
			decreaseBtn.isEnabled = true;
		}
		else if(produceCount <= 0)
		{
			increaseBtn.isEnabled = true;
			decreaseBtn.isEnabled = false;
		}
		else
		{
			increaseBtn.isEnabled = false;
			decreaseBtn.isEnabled = true;
		}


		//if was not slider alter the value...then change slider value
		if(UISlider.current == null)
		{
			//we don't want to receive event
			EventDelegate.Remove (unitSlider.onChange, OnUnitSliderChange);

			unitSlider.value = (float)val / (float)mgr.GetAvaliableUnitToProduceForType(unitType);

			EventDelegate.Set (unitSlider.onChange, OnUnitSliderChange);
		}
	}


	public void OnIncreaseUnitCount()
	{
		CombatUnitManager mgr = ((UITrainingNavController)_navigationController).combatUnitManager;

		int incVal = unitAddSubVal;

		int val = Mathf.Clamp (produceCount + incVal, 0, mgr.GetAvaliableUnitToProduceForType(unitType));

		ChangeProduceCount (val);
	}

	public void OnDecreaseUnitCount()
	{
		CombatUnitManager mgr = ((UITrainingNavController)_navigationController).combatUnitManager;

		int decVal = -1 * unitAddSubVal;

		int val = Mathf.Clamp (produceCount + decVal, 0, mgr.GetAvaliableUnitToProduceForType(unitType));

		ChangeProduceCount (val);
	}

	void OnUnitSliderChange()
	{
		CombatUnitManager mgr = ((UITrainingNavController)_navigationController).combatUnitManager;

		if(float.IsNaN(UISlider.current.value))
		{
			return;
		}

		float sliderVal = UISlider.current.value;

		int val = (int)((float)mgr.GetAvaliableUnitToProduceForType(unitType) * sliderVal);


		ChangeProduceCount (val);

	}
	*/

	/*
	void OnInputFieldChange()
	{
		if(string.IsNullOrEmpty(UIInput.current.value))
		{
			return;
		}

		CombatUnitManager mgr = ((UITrainingNavController)_navigationController).combatUnitManager;

		produceCount = Mathf.Clamp(int.Parse (UIInput.current.value), 0, mgr.availableUnitToProduce);

		UIInput.current.value = produceCount.ToString ();

		if((produceCount > 0) && (produceCount < mgr.availableUnitToProduce))
		{
			increaseBtn.isEnabled = true;
			decreaseBtn.isEnabled = true;
		}
		else if(produceCount <= 0)
		{
			increaseBtn.isEnabled = true;
			decreaseBtn.isEnabled = false;
		}
		else
		{
			increaseBtn.isEnabled = false;
			decreaseBtn.isEnabled = true;
		}

		//if was not slider alter the value...then change slider value
		if(UISlider.current == null)
		{
			EventDelegate.Remove (unitSlider.onChange, OnUnitSliderChange);

			unitSlider.value = (float)produceCount / (float)mgr.availableUnitToProduce;

			EventDelegate.Set (unitSlider.onChange, OnUnitSliderChange);
		}
	}
	*/

	public void OnProduceUnit()
	{
		if(produceCount <= 0)
		{
			return;
		}


		CombatUnitManager mgr = ((UITrainingNavController)_navigationController).combatUnitManager;

		if(mgr.ProduceCombatUnit (unitType, produceCount))
		{
			Debug.Log ("Produce unit " + unitType.ToString () + " with " + produceCount);

			_navigationController.CloseNavigationController();
		}

	}

	public void OnInstantProduceUnit()
	{
		if(produceCount <= 0)
		{
			return;
		}
		
		
		CombatUnitManager mgr = ((UITrainingNavController)_navigationController).combatUnitManager;
		
		if(mgr.InstantProduceCombatUnit(unitType, produceCount))
		{
			Debug.Log ("Instant produce unit " + unitType.ToString () + " with " + produceCount);
			
			_navigationController.CloseNavigationController();
		}
	}

	public void SetUnitInfo(CombatUnit info)
	{
		unitInfo = info;

		SetContentTitle = info.unitName;

		unitType = info.unitType;
	}
}
