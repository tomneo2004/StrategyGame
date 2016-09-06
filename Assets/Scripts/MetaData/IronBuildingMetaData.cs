using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


/// <summary>
/// This class IronBuildingMetaData is automatic generated.
/// 
/// Implement your persistant data calss here
/// </summary>
[Serializable]
public class IronBuildingMetaData : PersistantMetaData
{
	public ResourceType resourceId = ResourceType.Iron;

	public int collectPerDuration = 30;

	public float resourceRegenPerDuration = 10f;

	public float maxResourceStore = 1000f;

	public float currentResourceStore = 0f;

	public bool hasTask = false;

	public int taskDuration = 10;

	private DateTime taskStartTime;

	private DateTime taskEndTime;

	public static IronBuildingMetaData Load(bool processing = false)
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<IronBuildingMetaData>())
		{
			IronBuildingMetaData newData = new IronBuildingMetaData();

			newData.taskStartTime = DateTime.Now;
			newData.taskEndTime = DateTime.Now;

			newData.Save();

			return newData;
		}

		IronBuildingMetaData data = SaveLoadManager.SharedManager.Load<IronBuildingMetaData> ();

		if(processing)
		{
			data.ProcessData ();

			data.Save();
		}


		return data;
	}

	private void ProcessData()
	{
		if(hasTask)
		{
			int result = DateTime.Now.CompareTo(taskEndTime);

			//finish unfinished task
			if(result >= 0)
			{
				Debug.Log("server add resource to temp");
				currentResourceStore += resourceRegenPerDuration;
			}
			else
			{
				Debug.Log("current time:"+DateTime.Now.ToString());
				Debug.Log("end time:"+taskEndTime.ToString());
				Debug.Log("server last task not finished");
			}

			//time elapse
			if(result > 0)
			{
				TimeSpan tSpan = DateTime.Now.Subtract(taskEndTime);

				int numOfRegen = (int)(tSpan.TotalSeconds/collectPerDuration);
				int remainSeconds = (int)(tSpan.TotalSeconds%collectPerDuration);

				Debug.Log("Number of regen times:"+numOfRegen);
				Debug.Log("task remain time:"+remainSeconds);

				currentResourceStore += numOfRegen * resourceRegenPerDuration;

				if((currentResourceStore<maxResourceStore)&&(remainSeconds > 0))
				{
					hasTask = true;
					
					taskDuration = remainSeconds;

				}
				else
				{
					hasTask = false;
				}
			}
			else if(result < 0)
			{
				TimeSpan tSpan = taskEndTime.Subtract(DateTime.Now);

				int remainSeconds = (int)(tSpan.TotalSeconds%collectPerDuration);

				if(remainSeconds > 0)
				{
					hasTask = true;

					taskDuration = remainSeconds;
				}
				else
				{
					hasTask = false;
				}
			}
		}
		else
		{
			Debug.Log("No task");
		}
	}

	/// <summary>
	/// Start resource regen task.
	/// </summary>
	/// <returns><c>true</c>, if regen task was started, <c>false</c> otherwise.</returns>
	public bool StartRegenTask()
	{
		if(currentResourceStore >= maxResourceStore)
		{
			hasTask  = false;

			return false;
		}

		hasTask = true;

		taskDuration = collectPerDuration;

		taskStartTime = DateTime.Now;
		taskEndTime = DateTime.Now.AddSeconds (collectPerDuration);

		this.Save();

		return true;
	}

	/// <summary>
	/// Transfers the resource to player.
	/// </summary>
	/// <returns><c>true</c>, if resource to player was transfered, <c>false</c> otherwise.</returns>
	public bool TransferResourceToPlayer()
	{
		PlayerResourceStorageMetaData data = PlayerResourceStorageMetaData.Load ();
		
		float transferedResource = data.TransferResourceToPlayer (resourceId, currentResourceStore);
		
		currentResourceStore -= transferedResource;
		
		this.Save ();
		
		if(transferedResource > 0)
		{
			return true;
		}
		else
		{
			return false;
		}

	}

	/// <summary>
	/// Is task complete.
	/// </summary>
	/// <returns><c>true</c> if this instance is task complete; otherwise, <c>false</c>.</returns>
	public bool IsTaskComplete()
	{
		if(hasTask)
		{
			int result = DateTime.Now.CompareTo(taskEndTime);

			if(result >= 0)
			{
				Debug.Log("task complete add to current resource with resource to add:"+resourceRegenPerDuration);
				currentResourceStore += resourceRegenPerDuration;

				hasTask = false;

				this.Save();

				return true;
			}
		}

		this.Save ();

		return false;
	}

	public void Save()
	{
		SaveLoadManager.SharedManager.Save <IronBuildingMetaData>(this);
	}
}