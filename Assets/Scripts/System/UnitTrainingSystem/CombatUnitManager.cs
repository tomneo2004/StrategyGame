using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CombatUnitManager : MonoBehaviour, IArchitecture 
{
	static CombatUnitManager _instance;

	public static CombatUnitManager Instance
	{
		get
		{
			return _instance;
		}
	}

	public int maxCombatUnit = 1000;

	public int currentCombatUnit = 0;

	public int GetCurrentCombatUnit
	{
		get
		{
			CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load ();

			currentCombatUnit = data.CurrentCombatUnit;

			return currentCombatUnit;
		}
	}
	
	public List<CombatUnit> combatUnitInfo = new List<CombatUnit>();

	private Dictionary<CombatUnitType, int> _unitTypeAndNumber;

	private ProduceUnitTask _task;

	/// <summary>
	/// Gets the available unit to produce.
	/// How many unit can produce. type is ignored
	/// No unit space involve
	/// </summary>
	/// <value>The available unit to produce.</value>

	public int availableUnitToProduce
	{
		get
		{
			if(_task != null)
			{
				return Mathf.Clamp(maxCombatUnit-(currentCombatUnit+_task.ProducingUnitAmount), 0, maxCombatUnit);
			}
			else
			{
				return Mathf.Clamp(maxCombatUnit-currentCombatUnit, 0, maxCombatUnit);
			}
		}
	}

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		UpdateCombatUnitManager ();
	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventUIProduceUnitClick> (ProduceUnit);
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventUIProduceUnitClick> (ProduceUnit);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Gets the available unit to produce for type.
	/// How many unit can produce for type.
	/// This is base on how much resource and available unit space
	/// </summary>
	public int GetAvaliableUnitToProduceForType(CombatUnitType type)
	{
		CombatUnit unit = GetCombatUnitInfoByType (type);

		Dictionary<ResourceType, float> costResource = unit.GetResourceCost;



		if(costResource != null)
		{
			int unitMinProduction = availableUnitToProduce;

			foreach(ResourceType rType in costResource.Keys)
			{
				ResourceStorage rs = Player.Instance.GetResourceStorage(rType);

				unitMinProduction = Mathf.Min(Mathf.FloorToInt(rs.currentResource/costResource[rType]), unitMinProduction);
			}

			return unitMinProduction;
		}
		else
		{
			Debug.LogError("Combat unit type "+type.ToString()+" has no resource cost setup");

			return 0;
		}
	}

	/// <summary>
	/// Produces combat unit.
	/// </summary>
	/// <returns><c>true</c>, if combat unit was produced, <c>false</c> otherwise.</returns>
	/// <param name="unitType">Unit type.</param>
	/// <param name="produceAmount">Produce amount.</param>
	public bool ProduceCombatUnit(CombatUnitType unitType, int produceAmount)
	{
		if((combatUnitInfo != null) && (_task == null))
		{
			CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load ();
			
			CombatUnit unitInfo = null;
			
			for(int i=0; i<combatUnitInfo.Count; i++)
			{
				if(unitType == combatUnitInfo[i].unitType)
				{
					unitInfo = combatUnitInfo[i];
					
					break;
				}
			}
			
			if(unitInfo != null)
			{
				//todo:check if resource is enough
				bool canProduceUnit = true;
				
				for(int i=0; i<unitInfo.costResourceTypes.Count; i++)
				{
					if(!Player.Instance.IsResourceEnough(unitInfo.costResourceTypes[i], unitInfo.costResources[i]*produceAmount))
					{
						EventManager.GetInstance().ExecuteEvent<EventAlert>(new EventAlert("Warning", "Not enough resources"));
						
						canProduceUnit = false;
						
						break;
					}
				}
				
				if(canProduceUnit)
				{
					Dictionary<ResourceType, float> resourceCost = new Dictionary<ResourceType, float>();
					
					for(int i=0; i<unitInfo.costResourceTypes.Count; i++)
					{
						resourceCost.Add(unitInfo.costResourceTypes[i], unitInfo.costResources[i]);
					}
					
					if(data.ProduceCombatUnit(unitInfo.unitType, produceAmount, unitInfo.generateDuration * produceAmount, resourceCost))
					{
						_task = ProduceUnitTask.CreateTask(unitInfo.unitType, produceAmount, OnProducingUnitComplete, OnTimeLeftToComplete, unitInfo.generateDuration * produceAmount);
						
						TaskManager.Instance.AddTask(_task);
						
						//todo:post produce unit event
						List<ResourceType> costType = new List<ResourceType>();
						foreach(ResourceType type in resourceCost.Keys)
						{
							costType.Add(type);
						}
						
						Architecture archi = GetComponent<Architecture> ();
						
						if(archi.isSelected)
						{
							UIArchitectureMenuController.Instance.GetMenu(transform, ArchitectureMenuType.OnTask);
						}
						
						EventManager.GetInstance().ExecuteEvent<EventProduceCombatUnit>(new EventProduceCombatUnit(costType));

						return true;
					}


				}
				else
				{
					return false;
				}
				
				
			}
			else
			{
				Debug.LogError(gameObject.name+" unable to produce combat unit, no unit definition");
			}
		}

		return false;
	}

	/// <summary>
	/// Instants produce combat unit.
	/// </summary>
	/// <returns><c>true</c>, if produce combat unit was instanted, <c>false</c> otherwise.</returns>
	/// <param name="unitType">Unit type.</param>
	/// <param name="produceAmount">Produce amount.</param>
	public bool InstantProduceCombatUnit(CombatUnitType unitType, int produceAmount)
	{
		if((combatUnitInfo != null) && (_task == null))
		{
			CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load ();
			
			CombatUnit unitInfo = null;
			
			for(int i=0; i<combatUnitInfo.Count; i++)
			{
				if(unitType == combatUnitInfo[i].unitType)
				{
					unitInfo = combatUnitInfo[i];
					
					break;
				}
			}

			if(unitInfo != null)
			{
				bool canProduceUnit = true;
				
				for(int i=0; i<unitInfo.costResourceTypes.Count; i++)
				{
					if(!Player.Instance.IsResourceEnough(unitInfo.costResourceTypes[i], unitInfo.costResources[i]*produceAmount))
					{
						EventManager.GetInstance().ExecuteEvent<EventAlert>(new EventAlert("Warning", "Not enough resources"));
						
						canProduceUnit = false;
						
						break;
					}
				}

				if(canProduceUnit)
				{
					Dictionary<ResourceType, float> resourceCost = new Dictionary<ResourceType, float>();
					
					for(int i=0; i<unitInfo.costResourceTypes.Count; i++)
					{
						resourceCost.Add(unitInfo.costResourceTypes[i], unitInfo.costResources[i]);
					}

					if(data.ProduceCombatUnit(unitInfo.unitType, produceAmount, 0, resourceCost))
					{
						List<ResourceType> costType = new List<ResourceType>();
						foreach(ResourceType type in resourceCost.Keys)
						{
							costType.Add(type);
						}
						
						EventManager.GetInstance().ExecuteEvent<EventProduceCombatUnit>(new EventProduceCombatUnit(costType));

						return true;
					}
				}
			}
			else
			{
				Debug.LogError(gameObject.name+" unable to produce combat unit, no unit definition");
			}
		}

		return false;
	}

	/// <summary>
	/// Gets the type of the combat unit info by unit type.
	/// </summary>
	/// <returns>The combat unit info by type.</returns>
	/// <param name="type">Type.</param>
	public CombatUnit GetCombatUnitInfoByType(CombatUnitType type)
	{
		for(int i=0; i<combatUnitInfo.Count; i++)
		{
			if(combatUnitInfo[i].unitType == type)
			{
				return combatUnitInfo[i];
			}
		}

		return null;
	}

	/// <summary>
	/// Gets all defined combat unit info.
	/// </summary>
	/// <returns>The all combat unit info.</returns>
	public CombatUnit[] GetAllCombatUnitInfo()
	{
		return combatUnitInfo.ToArray ();
	}

	/// <summary>
	/// Gets the current produced number of combat unit.
	/// Filter true will filt out unit type that current number is 0
	/// </summary>
	/// <returns>The avaliable combat unit number.</returns>
	/// <param name="filter">If set to <c>true</c> filter.</param>
	public Dictionary<CombatUnit, int> GetAvaliableCombatUnitNumber(bool filter = true)
	{
		CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load (false);

		Dictionary<CombatUnitType, int> dic = data.GetAllUnit (filter);

		Dictionary<CombatUnit, int> retDic = new Dictionary<CombatUnit, int>();

		//keep combat unit defination order
		for(int i=0; i<combatUnitInfo.Count; i++)
		{
			if(dic.ContainsKey(combatUnitInfo[i].unitType))
			{
				retDic.Add(combatUnitInfo[i], dic[combatUnitInfo[i].unitType]);
			}
		}

		return retDic;
	}

	/// <summary>
	/// Gets the type of the unit quantity for unit type.
	/// Return 0 if can't find unit type
	/// </summary>
	/// <returns>The unit quantity for type.</returns>
	/// <param name="type">Type.</param>
	public int GetUnitQuantityForType(CombatUnitType type)
	{
		CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load ();

		_unitTypeAndNumber = data.GetAllUnit ();

		foreach(CombatUnitType unitType in _unitTypeAndNumber.Keys)
		{
			if(type == unitType)
			{
				return _unitTypeAndNumber[unitType];
			}
		}

		return 0;
	}

	/// <summary>
	/// Gets the total quantity for each type of unit.
	/// </summary>
	/// <returns>The total quantity.</returns>
	public int GetUnitTotalQuantity()
	{
		CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load ();

		_unitTypeAndNumber = data.GetAllUnit ();

		int retVal = 0;

		foreach(CombatUnitType unitType in _unitTypeAndNumber.Keys)
		{
			retVal += _unitTypeAndNumber[unitType];
		}

		return retVal;
	}

	/// <summary>
	/// Finishs the task instant.
	/// </summary>
	public void FinishTaskInstant()
	{
		if(_task != null)
		{
			_task.CompleteTaskInstant();
		}
	}

	/// <summary>
	/// Handle produce event
	/// </summary>
	/// <param name="e">E.</param>
	void ProduceUnit(EventUIProduceUnitClick e)
	{
		ProduceCombatUnit (e.unitType, e.produceAmount);
	}

	/// <summary>
	/// Updates combat unit manager.
	/// </summary>
	void UpdateCombatUnitManager()
	{
		CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load (true);

		maxCombatUnit = data.maxCombatUnit;
		currentCombatUnit = data.CurrentCombatUnit;
		_unitTypeAndNumber = data.GetAllUnit ();

		if((data.producingUnit) && (_task == null))
		{
			_task = ProduceUnitTask.CreateTask(data.producingUnitType, data.producingAmount, OnProducingUnitComplete, OnTimeLeftToComplete, data.producingDuration);

			TaskManager.Instance.AddTask(_task);
		}
		else
		{
			_task = null;
		}

	}

	/*
	bool IsTaskComplete()
	{
		CombatUnitManagerMetaData data = CombatUnitManagerMetaData.Load ();

		return data.IsProduceUnitComplete ();
	}

	IEnumerator CompleteTask()
	{
		while(!IsTaskComplete())
		{
			Debug.Log("Wait for server task complete");
			
			yield return new WaitForSeconds(1f);
		}

		UITimePopupController.Instance.HideTimePopup (transform);
		
		UpdateCombatUnitManager ();

		Architecture archi = GetComponent<Architecture> ();
		
		if(archi.isSelected)
		{
			UIArchitectureMenuController.Instance.GetMenu(transform, ArchitectureMenuType.Normal);
		}
		
	}
	*/

	/// <summary>
	/// Handle producing unit complete event.
	/// </summary>
	/// <param name="task">Task.</param>
	/// <param name="unitType">Unit type.</param>
	/// <param name="amount">Amount.</param>
	void OnProducingUnitComplete(ProduceUnitTask task, CombatUnitType unitType, int amount)
	{
		//StartCoroutine (CompleteTask());

		_task = null;

		UITimePopupController.Instance.HideTimePopup (transform);
		
		UpdateCombatUnitManager ();

		EventManager.GetInstance ().ExecuteEvent<EventProduceCombatUnitComplete> (new EventProduceCombatUnitComplete (this));
	}

	void OnTimeLeftToComplete(TimeTask task, int secondLeft)
	{
		Debug.Log ("finish training time left: "+ TimeConverter.GetHours (secondLeft) + ":"
		           + TimeConverter.GetMinutes (secondLeft) + ":" + TimeConverter.GetSeconds (secondLeft));

		UITimePopupController.Instance.GetTimePopup (transform).DisplayTime (secondLeft, task.TaskDuration, ImageManager.Instance.CombatUnitSpriteNameForHead(((ProduceUnitTask)task).UnitType));

	}

	public bool IsOnTask ()
	{
		if(_task != null)
		{
			return true;
		}

		return false;
	}
}
