using UnityEngine;
using System.Collections;

public class UIUnitPresentContent : UINavigationContent 
{
	public UILabel totalCombatUnit;

	public GameObject unitDisplayPrefab;

	public UIScrollView scrollview;

	public UIGrid grid;

	public UIUnitDetailContent detailContent;

	/// <summary>
	/// Presents the unit detail.
	/// </summary>
	/// <param name="info">Info.</param>
	public void PresentUnitDetail(CombatUnit info)
	{
		if(detailContent != null)
		{
			detailContent.SetUnitInfo(info);

			_navigationController.PushContent (detailContent);
		}
	}

	public override void OnContentDisplay()
	{
		base.OnContentDisplay ();

		CombatUnitManager mgr = NGUITools.FindInParents<UITrainingNavController> (transform).combatUnitManager;

		if (mgr != null) 
		{
			totalCombatUnit.text = mgr.GetCurrentCombatUnit + "/" + mgr.maxCombatUnit;
		}

		CombatUnit[] unitInfos = mgr.GetAllCombatUnitInfo ();

		if(grid.transform.childCount > 0)
		{
			Transform[] childs = new Transform[grid.transform.childCount];

			for(int i=0; i<grid.transform.childCount; i++)
			{
				childs[i] = grid.transform.GetChild(i);
			}

			for(int i=0; i<childs.Length; i++)
			{
				childs[i].parent = null;

				NGUITools.Destroy(childs[i].gameObject);
			}
		}

		for(int i=0; i<unitInfos.Length; i++)
		{
			GameObject go = NGUITools.AddChild(grid.gameObject, unitDisplayPrefab);

			UIUnitDisplay display = go.GetComponent<UIUnitDisplay>();

			display.SetCombatUnitInfo(unitInfos[i]);
		}

		grid.Reposition ();

		scrollview.ResetPosition ();
	}
}
