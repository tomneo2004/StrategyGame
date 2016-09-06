using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


/// <summary>
/// This class LevelExpMetaData is automatic generated.
/// 
/// Implement your persistant data calss here
/// </summary>
[Serializable]
public class LevelExpMetaData : PersistantMetaData
{
	public int playerMaxLevel = 100;
	
	public int playerCurrentLevel = 1;
	
	public int playerBaseExp = 100;
	
	public int playerExpToNextLevel = 0;
	
	public int playerCurrentExp = 0;

	public int playerReceivedExp = 0;

	public float scale = 1.1f;// above 1 cause more exp requirement to next level, less 1 cause less exp requirement to next level


	public static LevelExpMetaData Load(bool processing = false)
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<LevelExpMetaData>())
		{
			LevelExpMetaData newData = new LevelExpMetaData();

			//newData.playerExpToNextLevel = newData.playerBaseExp * newData.playerCurrentLevel;

			newData.playerExpToNextLevel = Mathf.RoundToInt( Mathf.Pow(newData.playerBaseExp * newData.playerCurrentLevel, newData.scale));

			//newData.playerExpToNextLevel = Mathf.RoundToInt(Mathf.Pow(newData.playerCurrentLevel * newData.scale, 1.5f) * (float)newData.playerBaseExp);
			
			newData.Save();
			
			return newData;
		}
		
		LevelExpMetaData data = SaveLoadManager.SharedManager.Load<LevelExpMetaData> ();
		
		if(processing)
		{
			data.ProcessData ();
			
			data.Save();
		}
		
		
		return data;
	}

	private void ProcessData()
	{

		if(playerCurrentLevel < playerMaxLevel)
		{
			playerExpToNextLevel = Mathf.RoundToInt( Mathf.Pow(playerCurrentLevel * playerBaseExp, scale));

			//playerExpToNextLevel = Mathf.RoundToInt(Mathf.Pow(playerCurrentLevel * scale, 1.5f) * (float)playerBaseExp);
		}
		else
		{
			playerCurrentLevel = playerMaxLevel;

			playerExpToNextLevel = Mathf.RoundToInt( Mathf.Pow((playerMaxLevel-1) * playerBaseExp, scale));

			//playerExpToNextLevel = Mathf.RoundToInt(Mathf.Pow((playerMaxLevel-1) * scale, 1.5f) * (float)playerBaseExp);

			playerCurrentExp = playerExpToNextLevel;
		}
	}

	/// <summary>
	/// Adds experience.
	/// </summary>
	/// <returns><c>true</c>, if exp was added, <c>false</c> otherwise.</returns>
	/// <param name="exp">Exp.</param>
	public bool AddExp(int exp)
	{

		if(playerCurrentLevel < playerMaxLevel)
		{
			playerReceivedExp += exp;
			playerCurrentExp += exp;

			if(playerCurrentExp > Mathf.RoundToInt( Mathf.Pow((playerMaxLevel-1) * playerBaseExp, scale)))
			{
				playerExpToNextLevel = Mathf.RoundToInt( Mathf.Pow((playerMaxLevel-1) * playerBaseExp, scale));

				//playerExpToNextLevel = Mathf.RoundToInt(Mathf.Pow((playerMaxLevel-1) * scale, 1.5f) * (float)playerBaseExp);

				playerCurrentExp = playerExpToNextLevel;

				playerCurrentLevel = playerMaxLevel;
			}
			else
			{
				int levelToAdd = playerCurrentExp/playerExpToNextLevel;

				playerCurrentExp = playerCurrentExp%playerExpToNextLevel;

				playerCurrentLevel += levelToAdd;

				playerExpToNextLevel = Mathf.RoundToInt( Mathf.Pow(playerCurrentLevel * playerBaseExp, scale));

				//playerExpToNextLevel = Mathf.RoundToInt(Mathf.Pow(playerCurrentLevel * scale, 1.5f) * (float)playerBaseExp);
			}

			Save();

			return true;
		}
		else
		{
			return false;
		}
	}

	public void Save()
	{
		SaveLoadManager.SharedManager.Save <LevelExpMetaData>(this);
	}
}