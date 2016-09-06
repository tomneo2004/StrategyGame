using UnityEngine;
using System.Collections;

public class IronResourceGenerateTask : ResourceGenerateTask 
{

	protected override IEnumerator CheckTaskComplete ()
	{
		IronBuildingMetaData data = IronBuildingMetaData.Load ();
		
		while(!data.IsTaskComplete ())
		{
			
			yield return new WaitForSeconds(1f);
		}
		
		CompleteTask ();
	}
}
