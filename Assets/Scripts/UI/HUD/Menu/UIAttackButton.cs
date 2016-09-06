using UnityEngine;
using System.Collections;

public class UIAttackButton : UITweenAlphaButton 
{

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update ();
	}

	public override void OnClick()
	{
		base.OnClick ();

		EventManager.GetInstance ().ExecuteEvent<EventPresentAttack> (new EventPresentAttack (AttackManager.Instance));
	}
}
