using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDeployContent : UINavigationContent 
{
	public GameObject unitDeployPrefab;

	public UIScrollView scrollview;

	public UIGrid grid;

	public UILabel totalUnitLabel;

	[HideInInspector]public string playerId;

	Dictionary<CombatUnitType, int> deployInfos = new Dictionary<CombatUnitType, int>();
	

	public override void OnContentDisplay()
	{
		base.OnContentDisplay ();

		CombatUnitManager mgr = CombatUnitManager.Instance;

		totalUnitLabel.text = mgr.GetUnitTotalQuantity().ToString();

		Transform[] child = new Transform[grid.transform.childCount];

		for(int i=0; i<grid.transform.childCount; i++)
		{
			child[i] = grid.transform.GetChild(i);
		}

		for(int i=0; i<child.Length; i++)
		{
			child[i].parent = null;
			NGUITools.Destroy(child[i].gameObject);
		}

		Dictionary<CombatUnit, int> dic = mgr.GetAvaliableCombatUnitNumber ();

		foreach(CombatUnit c in dic.Keys)
		{
			GameObject go = NGUITools.AddChild(grid.gameObject, unitDeployPrefab);

			UIUnitDeployDisplay display = go.GetComponent<UIUnitDeployDisplay>();

			display.Evt_OnUnitDeployNumberChange = OnValueChange;

			display.SetInfo(c.unitType, dic[c]);
		}

		grid.Reposition ();

		scrollview.ResetPosition ();
	}

	public void OnDeployUnit()
	{
		Debug.Log("Send unit to battle");

		foreach(CombatUnitType type in deployInfos.Keys)
		{
			Debug.Log(type.ToString()+" with unit of "+deployInfos[type]);
		}

		if(((UIAttackNavController)_navigationController).attackManager.AttackPlayer(playerId, deployInfos))
		{
			_navigationController.PopContent ();
		}


		deployInfos.Clear ();
	}

	void OnValueChange(CombatUnitType unitType, int unitNumber)
	{
		if(deployInfos.ContainsKey(unitType))
		{
			deployInfos[unitType] = unitNumber;
		}
		else
		{
			deployInfos.Add(unitType, unitNumber);
		}
	}
}
