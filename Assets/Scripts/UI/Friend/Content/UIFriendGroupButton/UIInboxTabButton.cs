using UnityEngine;
using System.Collections;

public class UIInboxTabButton : MonoBehaviour 
{
	public UISprite unreadSprite;

	// Use this for initialization
	void Start () 
	{

	}

	void OnEnable()
	{

	}

	void OnDisable()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void OnClick()
	{
		unreadSprite.gameObject.SetActive (false);
	}
}
