using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBattleReport : MonoBehaviour 
{
	public UIScrollView scrollview;

	public UITable table;

	public GameObject battleReportInfoPrefab;

	public UIContainer container;

	// Use this for initialization
	void Start () 
	{
		container.Evt_OnContainerActive = OnContainerActive;

		UpdateInfo ();
	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventNewBattleReport> (UpdateInfo);
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventNewBattleReport> (UpdateInfo);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void UpdateInfo(EventNewBattleReport e)
	{
		UpdateInfo ();
	}

	/// <summary>
	/// Updates the info.
	/// </summary>
	void UpdateInfo()
	{
		if(table.transform.childCount > 0)
		{
			List<Transform> childs = new List<Transform>();
			
			for(int i=0; i<table.transform.childCount; i++)
			{
				childs.Add(table.transform.GetChild(i));
			}
			
			for(int i=0; i<childs.Count; i++)
			{
				childs[i].parent = null;
				
				NGUITools.Destroy(childs[i].gameObject);
			}
		}

		BattleReportMetaData data = BattleReportMetaData.Load ();
		
		BattleReportInfo[] info = data.GetAllBattleReport ();
		
		for(int i=0; i<info.Length; i++)
		{
			GameObject newBattleInfo = NGUITools.AddChild(table.gameObject, battleReportInfoPrefab);
			
			UIBattleReportInfo display = newBattleInfo.GetComponent<UIBattleReportInfo>();
			
			display.SetInformation(info[i]);
		}

		table.Reposition ();

		scrollview.ResetPosition ();
	}

	/// <summary>
	/// Handle container active event.
	/// </summary>
	/// <param name="container">Container.</param>
	void OnContainerActive(UIContainer container)
	{
		UpdateInfo ();
	}
	
}
