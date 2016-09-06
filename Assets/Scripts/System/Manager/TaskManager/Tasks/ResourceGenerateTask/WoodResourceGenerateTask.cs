using UnityEngine;
using System.Collections;

public class WoodResourceGenerateTask : ResourceGenerateTask 
{

	protected override IEnumerator CheckTaskComplete ()
	{
		
		SawmillBuildingMetaData data = SawmillBuildingMetaData.Load ();
		
		while(!data.IsTaskComplete())
		{
			
			yield return new WaitForSeconds(1f);
		}


		CompleteTask ();
	}
}
