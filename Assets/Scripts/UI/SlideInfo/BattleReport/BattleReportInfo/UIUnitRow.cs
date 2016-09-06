using UnityEngine;
using System.Collections;

public class UIUnitRow : MonoBehaviour 
{
	public GameObject playerContainer;

	public UISprite playerUnitAvatar;

	public UILabel playerUnitFromLabel;

	public UILabel playerUnitToLabel;

	public GameObject targetContainer;

	public UISprite targetUnitAvatar;

	public UILabel targetUnitFromLabel;

	public UILabel targetUnitToLabel;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Sets the player unit info.
	/// If null it will not show
	/// </summary>
	/// <param name="info">Info.</param>
	public void SetPlayerUnitInfo(UnitLostInfo info = null)
	{
		if(info == null)
		{
			playerContainer.SetActive(false);

			return;
		}
		else
		{
			playerContainer.SetActive(true);
		}

		CombatUnit unit = CombatUnitManager.Instance.GetCombatUnitInfoByType (info.unitType);

		//playerUnitAvatar.spriteName = unit.avatarName;
		playerUnitAvatar.spriteName = ImageManager.Instance.CombatUnitSpriteNameForHead (unit.unitType);
		playerUnitFromLabel.text = info.from.ToString ();
		playerUnitToLabel.text = info.to.ToString ();
	}

	/// <summary>
	/// Sets the target unit info.
	/// If null it will not show
	/// </summary>
	/// <param name="info">Info.</param>
	public void SetTargetUnitInfo(UnitLostInfo info = null)
	{
		if(info == null)
		{
			targetContainer.SetActive(false);
			
			return;
		}
		else
		{
			targetContainer.SetActive(true);
		}

		CombatUnit unit = CombatUnitManager.Instance.GetCombatUnitInfoByType (info.unitType);

		//targetUnitAvatar.spriteName = unit.avatarName;
		targetUnitAvatar.spriteName = ImageManager.Instance.CombatUnitSpriteNameForHead (unit.unitType);
		targetUnitFromLabel.text = info.from.ToString ();
		targetUnitToLabel.text = info.to.ToString ();
	}
}
