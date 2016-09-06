using UnityEngine;
using System.Collections;

public class UIActionComfirm : MonoBehaviour 
{
	public UILabel titleLabel;

	public UILabel messageLabel;

	public UIButton confirmButton;

	public UILabel confirmBtnLabel;

	public UIButton cancelButton;

	public UILabel cancelBtnLabel;

	public GameObject container;

	public delegate void OnButtonClick(int buttonIndex);

	/// <summary>
	/// Event on button click. 0 ok 1 cancel
	/// </summary>
	public OnButtonClick Evt_OnButtonClick;

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventPresentActionConfirm> (OnPresentActionConfirm);
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventPresentActionConfirm> (OnPresentActionConfirm);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Handle confirm click event.
	/// </summary>
	public void OnConfirmClick()
	{
		if(Evt_OnButtonClick != null)
		{
			Evt_OnButtonClick(0);
		}

		CloseWindow ();
	}

	/// <summary>
	/// Hnadle cancel click event.
	/// </summary>
	public void OnCancelClick()
	{
		if(Evt_OnButtonClick != null)
		{
			Evt_OnButtonClick(1);
		}

		CloseWindow ();
	}

	/// <summary>
	/// Closes the window.
	/// </summary>
	void CloseWindow()
	{
		container.SetActive (false);
	}

	/// <summary>
	/// Hnandle present action confirm event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnPresentActionConfirm(EventPresentActionConfirm e)
	{
		titleLabel.text = e.title;
		messageLabel.text = e.message;
		confirmBtnLabel.text = e.confirmBtn;

		if(string.IsNullOrEmpty(e.cancelBtn))
		{
			cancelButton.gameObject.SetActive(false);
		}
		else
		{
			cancelBtnLabel.text = e.cancelBtn;
			cancelButton.gameObject.SetActive(true);
		}

		Evt_OnButtonClick = e.callback;

		container.SetActive (true);
	}
}
