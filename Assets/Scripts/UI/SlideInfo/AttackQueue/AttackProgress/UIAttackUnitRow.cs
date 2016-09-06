using UnityEngine;
using System.Collections;

public class UIAttackUnitRow : MonoBehaviour 
{
	public UISprite unitAvatar;

	public UILabel unitNumberLabel;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Sets the unit info.
	/// </summary>
	/// <param name="avatarName">Avatar name.</param>
	/// <param name="unitNumber">Unit number.</param>
	public void SetUnitInfo(CombatUnitType type, int unitNumber)
	{
		unitAvatar.spriteName = ImageManager.Instance.CombatUnitSpriteNameForHead(type);

		unitNumberLabel.text = unitNumber.ToString ();
	}
}
