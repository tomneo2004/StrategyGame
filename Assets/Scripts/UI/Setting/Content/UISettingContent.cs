using UnityEngine;
using System.Collections;

public class UISettingContent : UINavigationContent 
{
	public void OnPlayerProfileClick()
	{
		NavController.PushContent (NavController.GetContent<UIProfileContent> ());
	}
}
