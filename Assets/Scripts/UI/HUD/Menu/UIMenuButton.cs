using UnityEngine;
using System.Collections;

public class UIMenuButton : MonoBehaviour 
{
	public UIPlayTween playTween;

	public UISprite messageNofitySprite;

	public GameObject[] subButtons;

	bool showSubMenu = false;



	// Use this for initialization
	void Start () 
	{

	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventNewMessage> (OnNewMessage);
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventNewMessage> (OnNewMessage);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void OnClick()
	{
		messageNofitySprite.gameObject.SetActive (false);

		if(showSubMenu)
		{
			showSubMenu = false;

			CloseSubMenu();
		}
		else
		{
			showSubMenu = true;

			ShowSubMenu();
		}
	}

	void ShowSubMenu()
	{
		playTween.Play (true);
	}

	void CloseSubMenu()
	{
		playTween.Play (false);
	}

	/// <summary>
	/// Handle new message event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnNewMessage(EventNewMessage e)
	{
		if(!showSubMenu)
		{
			messageNofitySprite.gameObject.SetActive (true);
		}


		for(int i=0; i<subButtons.Length; i++)
		{
			subButtons[i].GetComponent<UITweenAlphaButton>().OnNotify();
		}
	}
}
