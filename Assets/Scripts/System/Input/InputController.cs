using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour 
{
	/// <summary>
	/// The collision mask.
	/// </summary>
	public LayerMask collisionMask;

	// Use this for initialization
	void Start () 
	{
		//NGUI fall through callback
		UICamera.fallThrough = gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnClick()
	{
		RaycastHit2D hit2d = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (UICamera.currentTouch.pos), Vector2.zero, Mathf.Infinity, collisionMask);

		if(hit2d)
		{

			EventManager.GetInstance().ExecuteEvent<EventInputOnObjectSelected>(new EventInputOnObjectSelected(hit2d.collider.gameObject));

		}
	}
}
