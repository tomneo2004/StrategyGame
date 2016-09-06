using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class ResourceStorageMetaData
{
	public ResourceType resourceType;

	public float currentResource = 0f;

	public float maxResource = 0f;

	public ResourceStorageMetaData(ResourceType type, float curRes, float maxRes)
	{
		resourceType = type;
		currentResource = curRes;
		maxResource = maxRes;
	}
	
}
/// <summary>
/// This class PlayerResourceStorageMetaData is automatic generated.
/// 
/// Implement your persistant data calss here
/// </summary>
[Serializable]
public class PlayerResourceStorageMetaData : PersistantMetaData
{
	private List<ResourceStorageMetaData> rsMetaData = new List<ResourceStorageMetaData>();

	public static PlayerResourceStorageMetaData Load()
	{
		if(!SaveLoadManager.SharedManager.IsFileExist<PlayerResourceStorageMetaData>())
		{
			PlayerResourceStorageMetaData newData = new PlayerResourceStorageMetaData();

			newData.Init();
			
			return newData;
		}
		
		PlayerResourceStorageMetaData data = SaveLoadManager.SharedManager.Load<PlayerResourceStorageMetaData> ();
		
		
		return data;
	}

	public void Init()
	{
		if(GetResourceMetaData(ResourceType.Food) == null)
		{
			rsMetaData.Add(new ResourceStorageMetaData(ResourceType.Food, 80000f, 100000f));
		}

		if(GetResourceMetaData(ResourceType.Wood) == null)
		{
			rsMetaData.Add(new ResourceStorageMetaData(ResourceType.Wood, 80000f, 100000f));
		}

		if(GetResourceMetaData(ResourceType.Crystal) == null)
		{
			rsMetaData.Add(new ResourceStorageMetaData(ResourceType.Crystal, 80000f, 100000f));
		}
	}

	/// <summary>
	/// Gets the resource meta data.
	/// </summary>
	/// <returns>The resource meta data.</returns>
	/// <param name="type">Type.</param>
	public ResourceStorageMetaData GetResourceMetaData(ResourceType type)
	{
		for(int i=0; i<rsMetaData.Count; i++)
		{
			if(rsMetaData[i].resourceType == type)
			{
				return rsMetaData[i];
			}
		}

		return null;
	}

	/// <summary>
	/// Adds resource for resource type.
	/// </summary>
	/// <param name="rType">R type.</param>
	/// <param name="amount">Amount.</param>
	public void AddResourceForType(ResourceType rType, float amount)
	{
		ResourceStorageMetaData rsMetaData = GetResourceMetaData (rType);

		float amountResourceToAdd = amount;

		if((rsMetaData.currentResource+amountResourceToAdd) > rsMetaData.maxResource)
		{
			amountResourceToAdd = rsMetaData.maxResource - rsMetaData.currentResource;
		}

		rsMetaData.currentResource += amountResourceToAdd;

		Save ();

		EventManager.GetInstance().ExecuteEvent<EventResourceStorageModelUpdate>(new EventResourceStorageModelUpdate(rsMetaData.resourceType, rsMetaData.currentResource));
	}

	/// <summary>
	/// Transfers the resource to player.
	/// Reture how much resource will be transfered
	/// </summary>
	/// <returns>The resource to player.</returns>
	/// <param name="Type">Type.</param>
	/// <param name="amount">Amount.</param>
	public float TransferResourceToPlayer(ResourceType Type, float amount)
	{
		ResourceStorageMetaData rsMetaData = GetResourceMetaData (Type);

		float retVal = 0f;

		if(rsMetaData != null)
		{
			if((rsMetaData.currentResource+amount) > rsMetaData.maxResource)
			{
				retVal = rsMetaData.maxResource - rsMetaData.currentResource;

				rsMetaData.currentResource = rsMetaData.maxResource;
			}
			else
			{
				rsMetaData.currentResource += amount;

				retVal =  amount;
			}
		}
		else
		{
			retVal = 0f;
		}

		Save ();

		return retVal;
	}

	/// <summary>
	/// Adds the resource.
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="amountToAdd">Amount to add.</param>
	public void AddResource(ResourceType type, float amountToAdd)
	{
		ResourceStorageMetaData rsMetaData = GetResourceMetaData (type);

		if((rsMetaData.currentResource+amountToAdd) > rsMetaData.maxResource)
		{
			rsMetaData.currentResource  = rsMetaData.maxResource;

			Save();

			EventManager.GetInstance().ExecuteEvent<EventResourceStorageModelUpdate>(new EventResourceStorageModelUpdate(rsMetaData.resourceType, rsMetaData.currentResource));

			return;
		}

		rsMetaData.currentResource += amountToAdd;

		Save ();

		EventManager.GetInstance().ExecuteEvent<EventResourceStorageModelUpdate>(new EventResourceStorageModelUpdate(rsMetaData.resourceType, rsMetaData.currentResource));
	}

	/// <summary>
	/// Costs the resource.
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="amountToCost">Amount to cost.</param>
	public void CostResource(ResourceType type, float amountToCost)
	{
		ResourceStorageMetaData rsMetaData = GetResourceMetaData (type);

		if((rsMetaData.currentResource-amountToCost) >= 0f)
		{
			rsMetaData.currentResource = Mathf.Floor(rsMetaData.currentResource - amountToCost);

			Debug.Log("Cost resource "+type.ToString()+" resource left "+rsMetaData.currentResource);

			Save();

			EventManager.GetInstance().ExecuteEvent<EventResourceStorageModelUpdate>(new EventResourceStorageModelUpdate(rsMetaData.resourceType, rsMetaData.currentResource));

		}
		else
		{
			Debug.LogError("Resource "+rsMetaData.resourceType.ToString()+" is not enough");
		}

	}

	public void Save()
	{
		SaveLoadManager.SharedManager.Save <PlayerResourceStorageMetaData>(this);
	}
}