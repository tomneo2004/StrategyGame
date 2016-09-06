using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(UIArchitectureMenuController))]
public class UIArchitectureMenuControllerEditor : Editor 
{

	UIArchitectureMenuController _target;

	List<int> removedMenuIndex;

	public void OnEnable()
	{
		_target = target as UIArchitectureMenuController;

		if(_target.menuTypes == null)
		{
			_target.menuTypes = new List<ArchitectureMenuType>();
		}

		if(_target.menuPrefabs == null)
		{
			_target.menuPrefabs = new List<GameObject>();
		}

		if(removedMenuIndex == null)
		{
			removedMenuIndex = new List<int>();
		}
	}

	void DisplayMenuType()
	{
		EditorGUILayout.BeginVertical ();

		for(int i=0; i<_target.menuTypes.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();

			_target.menuTypes[i] = (ArchitectureMenuType)EditorGUILayout.EnumPopup(_target.menuTypes[i]);

			_target.menuPrefabs[i] = (GameObject)EditorGUILayout.ObjectField(_target.menuPrefabs[i], typeof(GameObject), false);

			GUI.color = Color.red;
			if(GUILayout.Button("-"))
			{
				if(EditorUtility.DisplayDialogComplex("Delete menu", "Delete menu type "+_target.menuTypes[i].ToString()+"?", "Ok", "Cancel", "") == 0)
				{
					removedMenuIndex.Add(i);
				}

			}
			GUI.color = Color.white;

			EditorGUILayout.EndHorizontal();


			if(_target.menuTypes[i] == ArchitectureMenuType.Unknow)
			{
				EditorGUILayout.HelpBox("Menu type can not be unknow, you must pick one", MessageType.Error);
			}
			else if(_target.menuPrefabs[i] == null)
			{
				EditorGUILayout.HelpBox("You must assigned menu gameobject prefab", MessageType.Error);
			}
		}

		for(int i=0; i<removedMenuIndex.Count; i++)
		{
			int index = removedMenuIndex[i];

			_target.menuTypes.RemoveAt(index);
			_target.menuPrefabs.RemoveAt(index);
		}
		removedMenuIndex.Clear ();


		GUI.color = Color.green;
		if(GUILayout.Button("Add menu prefab"))
		{
			_target.menuTypes.Add(ArchitectureMenuType.Unknow);
			_target.menuPrefabs.Add(null);
		}
		GUI.color = Color.white;

		EditorGUILayout.EndVertical ();
	}

	public override void OnInspectorGUI()
	{
		DisplayMenuType ();
	}
}
