using UnityEngine;
using System.Collections;

public class UIContainer : MonoBehaviour 
{
	public delegate void OnContainerActive(UIContainer container);
	public OnContainerActive Evt_OnContainerActive;

	public delegate void OnContainerDeactive(UIContainer container);
	public OnContainerDeactive Evt_OnContainerDeactive;

	// Use this for initialization
	void Start () 
	{
	
	}

	void OnEnable()
	{
		if(Evt_OnContainerActive != null)
		{
			Evt_OnContainerActive(this);
		}
	}
	
	void OnDisable()
	{
		if(Evt_OnContainerDeactive != null)
		{
			Evt_OnContainerDeactive(this);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
