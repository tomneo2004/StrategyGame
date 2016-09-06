using UnityEngine;
using System.Collections;

public class UIAddExp : MonoBehaviour 
{
	public int expToAdd = 50;

	// Use this for initialization
	void Start () 
	{
		GetComponentInChildren<UILabel>().text = "Add "+expToAdd+" Exp";
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void AddExp()
	{
		EventManager.GetInstance ().ExecuteEvent<EventUIAddExpClick> (new EventUIAddExpClick (expToAdd));
	}
}
