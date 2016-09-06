using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UIResourceButtonPopupController))]
public class UIButtonPopupControllerEditor : Editor 
{
	UIResourceButtonPopupController _target;

	//remember all resource id of popup
	List<UIButtonPopupInfo> _removablePopups;

	//Dictionary<string, GameObject> _changedPrefabs;

	//prevent add same name of resource id when add new resource popup 
	//int incrementIndex = 0;

	//should modify resource id
	//bool modifyResourceId = false;

	//original resource id that is going to be modify
	//string modifyFromResourceId = null;

	//new resource id that will be modify to
	//string modifyToResourceId = null;


	public void OnEnable()
	{
		_target = target as UIResourceButtonPopupController;

		_removablePopups = new List<UIButtonPopupInfo> ();

		//_changedPrefabs = new Dictionary<string, GameObject> ();

		/*
		//calculate index
		if((_target.popupPrefabs != null) && (_target.popupPrefabs.Count > 0))
		{
			incrementIndex = _target.popupPrefabs.Count-1;
		}
		*/
	}

	bool HasResourceId(ResourceType comparedId)
	{
		for(int i=0; i<_target.popupPrefabs.Count; i++)
		{
			if(comparedId == _target.popupPrefabs[i].ResourceId)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Modifies the resource identifier.
	/// </summary>
	/*
	void ModifyResourceId()
	{

		for(int i=0; i<_target.popupPrefabs.Count; i++)
		{
			if(modifyFromResourceId == _target.popupPrefabs[i].ResourceId)
			{
				_target.popupPrefabs[i].ResourceId = modifyToResourceId;
			}
		}

		modifyFromResourceId = null;
		modifyToResourceId = null;
	}
	*/

	/// <summary>
	/// Displaies the add button.
	/// </summary>
	void DisplayAddButton()
	{
		GUI.color = Color.green;
		if(GUILayout.Button("Add popup button prefab"))
		{
			ResourceType preAddType = ResourceType.Unknow;

			//create dictionary if necessary
			if(_target.popupPrefabs == null)
			{
				_target.popupPrefabs = new List<UIButtonPopupInfo>();
			}

			//see if we need to change name of new resource popup
			/*
			if(HasResourceId(preAddType))
			{
				preAddType = preAddName + incrementIndex;
				
				++incrementIndex;
			}
			*/

			UIButtonPopupInfo newInfo = new UIButtonPopupInfo(preAddType, null);

			_target.popupPrefabs.Add(newInfo);
		}
		GUI.color = Color.white;
	}

	/// <summary>
	/// Displaies the popup prefabs.
	/// </summary>
	void DisplayPopupPrefabs()
	{
		if(_target.popupPrefabs != null)
		{

			EditorGUILayout.BeginVertical();

			for(int i=0; i<_target.popupPrefabs.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();

				UIButtonPopupInfo info = _target.popupPrefabs[i];

				EditorGUILayout.LabelField("Resource Id:", GUILayout.Width(70f));

				ResourceType newResourceId = (ResourceType)EditorGUILayout.EnumPopup(info.ResourceId, GUILayout.Width(100f));

				//if change text field then mark this resource id as modify
				if((newResourceId != info.ResourceId) && (!HasResourceId(newResourceId)))
				{
					/*
					modifyResourceId = true;
					modifyFromResourceId = info.ResourceId;
					modifyToResourceId = newResourceId;
					*/

					info.ResourceId = newResourceId;
				}

				//if drag new prefab
				Object source = EditorGUILayout.ObjectField(info.PopupPrefab, typeof(GameObject), false);

				if((GameObject)source != info.PopupPrefab)
				{
					info.PopupPrefab = (GameObject)source;
				}
				
				GUI.color = Color.red;
				if(GUILayout.Button("X"))//delete button
				{
					_removablePopups.Add(info);
					
				}
				GUI.color = Color.white;

				EditorGUILayout.EndHorizontal();
			}


			//see if need to modify
			/*
			if(modifyResourceId)
			{
				ModifyResourceId();

				modifyResourceId = false;
			}
			*/

			//changed prefab
			/*
			Dictionary<string, GameObject>.KeyCollection theKeys = _changedPrefabs.Keys;
			foreach(string resourceId in theKeys)
			{
				_target.popupPrefabs[resourceId] = _changedPrefabs[resourceId];
			}

			_changedPrefabs.Clear();
			*/

			//remove all resouce popup that was mark as remove
			for(int i=0; i<_removablePopups.Count; i++)
			{
				if(!_target.popupPrefabs.Remove(_removablePopups[i]))
				{
					Debug.LogError("Can not remove pop button for resource id "+_removablePopups[i].ResourceId);
				}
			}

			_removablePopups.Clear();


			DisplayAddButton();

			EditorGUILayout.EndVertical();
		}
		else
		{
			DisplayAddButton();
		}
	}

	public override void OnInspectorGUI()
	{
		DisplayPopupPrefabs ();

		Repaint ();
	}
}
