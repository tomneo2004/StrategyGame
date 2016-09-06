using UnityEngine;
using System.Collections;

public class UIUnitDisplay : MonoBehaviour 
{

	public UISprite unitSprite;

	public UILabel quantityLabel;

	private CombatUnit unitInfo;



	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Sets the combat unit info.
	/// </summary>
	/// <param name="info">Info.</param>
	public void SetCombatUnitInfo(CombatUnit info)
	{
		unitInfo = info;

		unitSprite.spriteName = ImageManager.Instance.CombatUnitSpriteNameForHead(info.unitType);

		quantityLabel.text = CombatUnitManager.Instance.GetUnitQuantityForType (unitInfo.unitType).ToString();
	}

	/// <summary>
	/// Handle detail button click event.
	/// </summary>
	public void OnDetailButtonClick()
	{
		NGUITools.FindInParents<UIUnitPresentContent> (transform).PresentUnitDetail (unitInfo);
	}
}
