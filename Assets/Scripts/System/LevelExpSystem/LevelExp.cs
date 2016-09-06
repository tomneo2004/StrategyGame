using UnityEngine;
using System.Collections;

public class LevelExp : MonoBehaviour 
{
	public int playerMaxLevel = 100;

	public int playerCurrentLevel = 1;

	public int playerBaseExp = 100;

	public int playerExpToNextLevel = 0;

	public int playerCurrentExp = 0;

	// Use this for initialization
	void Start () 
	{
		UpdateLevelExp ();
	}

	void OnEnable()
	{
		//test only
		EventManager.GetInstance ().AddListener<EventUIAddExpClick> (OnAddExp);
	}

	void OnDisable()
	{
		//test only
		EventManager.GetInstance ().RemoveListener<EventUIAddExpClick> (OnAddExp);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Updates the level exp.
	/// </summary>
	void UpdateLevelExp()
	{
		LevelExpMetaData data = LevelExpMetaData.Load (true);

		playerMaxLevel = data.playerMaxLevel;
		playerCurrentLevel = data.playerCurrentLevel;
		playerBaseExp = data.playerBaseExp;
		playerExpToNextLevel = data.playerExpToNextLevel;
		playerCurrentExp = data.playerCurrentExp;

		EventManager.GetInstance ().ExecuteEvent<EventUpdateLevelExp> (new EventUpdateLevelExp (playerCurrentLevel, playerCurrentExp, playerExpToNextLevel));

	}

	/// <summary>
	/// Adds exp.
	/// </summary>
	/// <param name="exp">Exp.</param>
	public void AddExp(int exp)
	{
		LevelExpMetaData data = LevelExpMetaData.Load (true);
		
		if(data.AddExp(exp))
		{
			UpdateLevelExp();
		}
	}

	//test only
	void OnAddExp(EventUIAddExpClick e)
	{
		AddExp (e.expToAdd);
	}


}
