using UnityEngine;
using System.Collections;

public class UIAttackContent : UINavigationContent 
{
	///////////////////////
	/// Player
	//////////////////////
	public UIScrollView playerScrollview;
	public UIWrapContent wrapContent;

	public GameObject playerDisplayPrefab;

	private GameObject selectedChild;

	///////////////////////
	/// Resource
	//////////////////////
	public UILabel timeCostLabel;

	public UINavigationContent deployUnitContent;
	

	public override void OnContentBeginHide ()
	{
		base.OnContentBeginHide ();

		if(wrapContent.transform.childCount > 0)
		{
			GameObject[] childs  = new GameObject[wrapContent.transform.childCount];

			for(int i=0; i<wrapContent.transform.childCount; i++)
			{
				childs[i] = wrapContent.transform.GetChild(i).gameObject;
			}


			for(int i=0; i<childs.Length; i++)
			{
				childs[i].transform.parent = null;
				NGUITools.Destroy(childs[i]);
			}

			wrapContent.SortBasedOnScrollMovement ();
			wrapContent.WrapContent ();

		}
	}

	public override void OnContentDisplay ()
	{
		base.OnContentDisplay ();

		DisplayPlayers ();

		wrapContent.GetComponent<UICenterOnChild>().onFinished = OnChildSelected;

		CombatUnitManager mgr = CombatUnitManager.Instance;

	}

	/// <summary>
	/// Displaies the players.
	/// </summary>
	void DisplayPlayers()
	{
		AIPlayer[] players = ((UIAttackNavController)_navigationController).attackManager.AllAIPlayer ();
		
		for(int i=0; i<players.Length; i++)
		{
			AIPlayer playerInfo = players[i];
			
			GameObject newPlayerDisplay =  NGUITools.AddChild(wrapContent.gameObject, playerDisplayPrefab);
			
			UIPlayerDisplay display = newPlayerDisplay.GetComponent<UIPlayerDisplay>();
			
			display.SetName(playerInfo.playerName);
			Debug.Log("AI name: "+playerInfo.playerName);
			display.SetPlayerId(playerInfo.playerId);

			if(i==0)
			{
				selectedChild = newPlayerDisplay;
				
				UpdateInfo();
			}
		}

		playerScrollview.ResetPosition ();

		wrapContent.SortBasedOnScrollMovement ();
		wrapContent.WrapContent ();

		if(selectedChild != null)
		{
			wrapContent.GetComponent<UICenterOnChild>().CenterOn(selectedChild.transform);
		}

	}

	/// <summary>
	/// Deploies the unit.
	/// </summary>
	public void DeployUnit()
	{
		((UIDeployContent)deployUnitContent).playerId = selectedChild.GetComponent<UIPlayerDisplay> ().PlayerId;

		_navigationController.PushContent (deployUnitContent);
	}
	

	void OnChildSelected()
	{
		GameObject centeredObj = wrapContent.GetComponent<UICenterOnChild> ().centeredObject;

		if(selectedChild == centeredObj)
		{
			return;
		}

		selectedChild = centeredObj;

		UpdateInfo ();
	}

	void UpdateInfo()
	{
		string id = selectedChild.GetComponent<UIPlayerDisplay> ().PlayerId;

		AIPlayer playerInfo = ((UIAttackNavController)_navigationController).attackManager.GetAIPlayer (id);
		
		timeCostLabel.text = playerInfo.attackDuration.ToString ();
	}
	
}
