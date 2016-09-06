using UnityEngine;
using System.Collections;

public class UIPostNewMessage : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void OnPostNewMessage()
	{
		EventManager.GetInstance ().ExecuteEvent<EventNewMessage> (new EventNewMessage ());
	}
}
