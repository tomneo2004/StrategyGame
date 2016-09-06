using UnityEngine;
using System.Collections;

public class FoodResourceGenerateTask : ResourceGenerateTask 
{
	protected override IEnumerator CheckTaskComplete ()
	{
		FarmlandBuildingMetaData data = FarmlandBuildingMetaData.Load ();
		
		while(!data.IsTaskComplete ())
		{
			
			yield return new WaitForSeconds(1f);
		}
		
		CompleteTask ();
	}
}
