using UnityEngine;
using System.Collections;

public class UIAttackUnitDetail : MonoBehaviour 
{
	public GameObject attackUnitRowPrefab;

	public UIScrollView scrollview;

	public UIGrid grid;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetAttackUnitDetail(AttackTask task)
	{
		if(grid.transform.childCount > 0)
		{
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
		}

		foreach(CombatUnit unit in task.PlayerUnit.Keys)
		{
			GameObject go = NGUITools.AddChild(grid.gameObject, attackUnitRowPrefab);

			UIAttackUnitRow rowDisplay = go.GetComponent<UIAttackUnitRow>();

			rowDisplay.SetUnitInfo(unit.unitType, task.PlayerUnit[unit]);
		}

		grid.Reposition ();

		scrollview.ResetPosition ();
	}
}
