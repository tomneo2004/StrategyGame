using UnityEngine;
using System.Collections;

public class Barrack : Architecture 
{
	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();

		EventManager.GetInstance ().AddListener<EventProduceCombatUnitComplete> (OnProduceUnitComplete);
		EventManager.GetInstance ().AddListener<EventInstantCompleteTask> (OnInstantCompleteTask);
	}

	protected override void OnDisable ()
	{
		base.OnDisable ();

		EventManager.GetInstance ().RemoveListener<EventProduceCombatUnitComplete> (OnProduceUnitComplete);
		EventManager.GetInstance ().RemoveListener<EventInstantCompleteTask> (OnInstantCompleteTask);
	}

	protected override void Update ()
	{
		base.Update ();
	}

	public override void OnArchitectureSelect ()
	{
		base.OnArchitectureSelect ();

		/*
		CombatUnitManager mgr = GetComponent<CombatUnitManager> ();

		if(mgr != null)
		{
			EventManager.GetInstance ().ExecuteEvent<EventBarrackSelected> (new EventBarrackSelected (mgr));
		}
		else
		{
			Debug.LogError(gameObject.name+" has no CombatUnitManager component");
		}
		*/

		if(IsArchitectureOnTask())
		{
			UIArchitectureMenuController.Instance.GetMenu(transform, ArchitectureMenuType.OnTask);
		}
		else
		{
			UIArchitectureMenuController.Instance.GetMenu(transform, ArchitectureMenuType.BarrackNormal);
		}
	}

	void OnProduceUnitComplete(EventProduceCombatUnitComplete e)
	{
		UIArchitectureMenuController.Instance.GetMenu(transform, ArchitectureMenuType.BarrackNormal);
	}

	void OnInstantCompleteTask(EventInstantCompleteTask e)
	{
		CombatUnitManager mgr = GetComponent<CombatUnitManager> ();

		mgr.FinishTaskInstant ();
	}
}
