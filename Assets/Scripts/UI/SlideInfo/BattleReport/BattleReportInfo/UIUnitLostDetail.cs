using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIUnitLostDetail : MonoBehaviour 
{
	public GameObject unitRowPrefab;

	public UIScrollView scrollView;

	public UIGrid grid;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Setups the unit lost detail info.
	/// </summary>
	/// <param name="playerUnitLostInfo">Player unit lost info.</param>
	/// <param name="targetUnitLostInfo">Target unit lost info.</param>
	public void SetupUnitLostDetailInfo(List<UnitLostInfo> playerUnitLostInfo, List<UnitLostInfo> targetUnitLostInfo)
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

		int maxCount = Mathf.Max (playerUnitLostInfo.Count, targetUnitLostInfo.Count);

		for(int i=0; i<maxCount; i++)
		{
			GameObject go = NGUITools.AddChild(grid.gameObject, unitRowPrefab);

			UIUnitRow rowDisplay = go.GetComponent<UIUnitRow>();

			if(i<playerUnitLostInfo.Count)
			{
				rowDisplay.SetPlayerUnitInfo(playerUnitLostInfo[i]);
			}
			else
			{
				rowDisplay.SetPlayerUnitInfo(null);
			}

			if(i<targetUnitLostInfo.Count)
			{
				rowDisplay.SetTargetUnitInfo(targetUnitLostInfo[i]);
			}
			else
			{
				rowDisplay.SetTargetUnitInfo(null);
			}
		}

		grid.Reposition ();

		scrollView.ResetPosition ();
	}
}
