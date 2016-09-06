using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ImageManager))]
public class ImageManagerEditor : Editor 
{
	ImageManager _target;

	bool _combatUnitFoldout = false;
	List<bool> _combatUnitTypeFoldout;
	int combatUnitWorkingIndex = 0;

	bool _resourceFoldout = false;
	List<bool> _resourceTypeFoldout;
	int resourceWorkingIndex = 0;

	public void OnEnable()
	{
		_target = target as ImageManager;

		if(_combatUnitTypeFoldout == null)
		{
			_combatUnitTypeFoldout = new List<bool>();
			for(int i=0; i<_target.combatUnitType.Count; i++)
			{
				_combatUnitTypeFoldout.Add(false);
			}
		}

		if(_resourceTypeFoldout == null)
		{
			_resourceTypeFoldout = new List<bool>();
			for(int i=0; i<_target.resourceType.Count; i++)
			{
				_resourceTypeFoldout.Add(false);
			}
		}
	}

	#region CombatUnit
	void OnSelectAtlasForHead (Object obj)
	{
		_target.combatUnitHeadAtlas [combatUnitWorkingIndex] = obj as UIAtlas;
		NGUISettings.atlas = obj as UIAtlas;

		serializedObject.Update ();
	}

	void SelectSpriteForHead (string spriteName)
	{
		_target.combatUnitHeadSpriteName [combatUnitWorkingIndex] = spriteName;
		NGUISettings.selectedSprite = spriteName;

		serializedObject.Update ();
	}

	void OnSelectAtlasForFullBody (Object obj)
	{
		_target.combatunitFullBodyAtlas [combatUnitWorkingIndex] = obj as UIAtlas;
		NGUISettings.atlas = obj as UIAtlas;
		
		serializedObject.Update ();
	}

	void SelectSpriteForFullBody (string spriteName)
	{
		_target.combatUnitFullBodySpriteName [combatUnitWorkingIndex] = spriteName;
		NGUISettings.selectedSprite = spriteName;
		
		serializedObject.Update ();
	}

	void DisplayCombatUnit()
	{
		_combatUnitFoldout = EditorGUILayout.Foldout(_combatUnitFoldout, new GUIContent("Combat unit", "Combat unit's image"));

		++EditorGUI.indentLevel;

		if(_combatUnitFoldout)
		{
			int indexToRemoved = -1;

			//render each combat unit
			for(int i=0; i<_target.combatUnitType.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();

				_combatUnitTypeFoldout[i] = EditorGUILayout.Foldout(_combatUnitTypeFoldout[i], _target.combatUnitType[i].ToString());

				GUI.color = Color.red;
				if(GUILayout.Button(new GUIContent("X", "Delete combat unit type"), GUILayout.Width(30f)))
				{
					if(EditorUtility.DisplayDialogComplex("Delete this combat unit?", "Delete combat unit type "+_target.combatUnitType[i].ToString()+"?", "Ok", "Cancel", "") == 0)
					{
						indexToRemoved = i;
					}
				}
				GUI.color = Color.white;

				EditorGUILayout.EndHorizontal();


				if(_combatUnitTypeFoldout[i])
				{
					//set current working index
					combatUnitWorkingIndex = i;

					//only edit one at a time
					for(int j=0; j<_combatUnitTypeFoldout.Count; j++)
					{
						if(i != j)
						{
							_combatUnitTypeFoldout[j] = false;
						}
					}

					++EditorGUI.indentLevel;

					EditorGUILayout.Space();

					//type
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(new GUIContent("Type:", "Combat unit type"));
					_target.combatUnitType[i] = (CombatUnitType)EditorGUILayout.EnumPopup(_target.combatUnitType[i]);
					EditorGUILayout.EndHorizontal();

					//head image
					EditorGUILayout.LabelField(new GUIContent("Head image:", "Combat unit's head image"));

					EditorGUILayout.BeginHorizontal();
					if (NGUIEditorTools.DrawPrefixButton("Atlas"))
					{
						ComponentSelector.Show<UIAtlas>(OnSelectAtlasForHead);
					}
					SerializedProperty headAtlas = serializedObject.FindProperty("combatUnitHeadAtlas").GetArrayElementAtIndex(i);
					NGUIEditorTools.DrawProperty("", headAtlas, GUILayout.MinWidth(20f));
					EditorGUILayout.EndHorizontal();

					NGUIEditorTools.DrawAdvancedSpriteField(headAtlas.objectReferenceValue as UIAtlas, _target.combatUnitHeadSpriteName[i], SelectSpriteForHead, false);

					if(string.IsNullOrEmpty(_target.combatUnitHeadSpriteName[i]))
					{
						EditorGUILayout.HelpBox("Please select a image for combat unit's head", MessageType.Error);
					}

					EditorGUILayout.Space();

					//full body image
					EditorGUILayout.LabelField(new GUIContent("Full Body image:", "Combat unit's full body image"));
					
					EditorGUILayout.BeginHorizontal();
					if(NGUIEditorTools.DrawPrefixButton("Atlas"))
					{
						ComponentSelector.Show<UIAtlas>(OnSelectAtlasForFullBody);
					}
					SerializedProperty fullBodyAtlas = serializedObject.FindProperty("combatunitFullBodyAtlas").GetArrayElementAtIndex(i);
					NGUIEditorTools.DrawProperty("", fullBodyAtlas, GUILayout.MinWidth(20f));
					EditorGUILayout.EndHorizontal();

					NGUIEditorTools.DrawAdvancedSpriteField(fullBodyAtlas.objectReferenceValue as UIAtlas, _target.combatUnitFullBodySpriteName[i], SelectSpriteForFullBody, false);

					if(string.IsNullOrEmpty(_target.combatUnitFullBodySpriteName[i]))
					{
						EditorGUILayout.HelpBox("Please select a image for combat unit's full body", MessageType.Error);
					}

					--EditorGUI.indentLevel;
				}

			}

			if(indexToRemoved >= 0)
			{
				_target.combatUnitType.RemoveAt(indexToRemoved);
				_target.combatUnitHeadSpriteName.RemoveAt(indexToRemoved);
				_target.combatUnitFullBodySpriteName.RemoveAt(indexToRemoved);
				_target.combatUnitHeadAtlas.RemoveAt(indexToRemoved);
				_target.combatunitFullBodyAtlas.RemoveAt(indexToRemoved);

				_combatUnitTypeFoldout.RemoveAt(indexToRemoved);

				serializedObject.Update();
			}

			GUI.color = Color.green;
			if(GUILayout.Button(new GUIContent("+", "Add new combat unit's image")))
			{
				_target.combatUnitType.Add(CombatUnitType.Unknow);
				_target.combatUnitHeadSpriteName.Add("");
				_target.combatUnitFullBodySpriteName.Add("");
				_target.combatUnitHeadAtlas.Add(NGUISettings.atlas);
				_target.combatunitFullBodyAtlas.Add(NGUISettings.atlas);

				_combatUnitTypeFoldout.Add(false);
				
				serializedObject.Update();
			}
			GUI.color = Color.white;


		}

		--EditorGUI.indentLevel;
	}
	#endregion CombatUnit

	#region Resource
	void OnSelectAtlasForResource (Object obj)
	{
		_target.resourceAtlas [resourceWorkingIndex] = obj as UIAtlas;
		NGUISettings.atlas = obj as UIAtlas;
		
		serializedObject.Update ();
	}

	void SelectSpriteForResource (string spriteName)
	{
		_target.resourceSpriteName [resourceWorkingIndex] = spriteName;
		NGUISettings.selectedSprite = spriteName;
		
		serializedObject.Update ();
	}

	void DisplayResource()
	{
		_resourceFoldout = EditorGUILayout.Foldout(_resourceFoldout, new GUIContent("Resource", "Resource type's image"));

		++EditorGUI.indentLevel;

		if(_resourceFoldout)
		{
			int indexToRemoved = -1;
			//render each resource
			for(int i=0; i<_target.resourceType.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();

				_resourceTypeFoldout[i] = EditorGUILayout.Foldout(_resourceTypeFoldout[i], new GUIContent(_target.resourceType[i].ToString(), "Resource type's image"));

				GUI.color = Color.red;
				if(GUILayout.Button(new GUIContent("X", "Delete resource type"), GUILayout.Width(30f)))
				{
					if(EditorUtility.DisplayDialogComplex("Delete this resource?", "Delete resource type "+_target.resourceType[i].ToString()+"?", "Ok", "Cancel", "") == 0)
					{
						indexToRemoved = i;
					}
				}
				GUI.color = Color.white;

				EditorGUILayout.EndHorizontal();

				if(_resourceTypeFoldout[i])
				{
					resourceWorkingIndex = i;

					for(int j=0; j<_resourceTypeFoldout.Count; j++)
					{
						if(i != j)
						{
							_resourceTypeFoldout[j] = false;
						}
					}

					++EditorGUI.indentLevel;

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(new GUIContent("Type:"));
					_target.resourceType[i] = (ResourceType)EditorGUILayout.EnumPopup(_target.resourceType[i]);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space();

					//Resource image
					EditorGUILayout.LabelField(new GUIContent("Resource image:", "Resource type's image"));

					EditorGUILayout.BeginHorizontal();
					if (NGUIEditorTools.DrawPrefixButton("Atlas"))
					{
						ComponentSelector.Show<UIAtlas>(OnSelectAtlasForResource);
					}
					SerializedProperty resourceAtlas = serializedObject.FindProperty("resourceAtlas").GetArrayElementAtIndex(i);
					NGUIEditorTools.DrawProperty("", resourceAtlas, GUILayout.MinWidth(20f));
					EditorGUILayout.EndHorizontal();
					
					NGUIEditorTools.DrawAdvancedSpriteField(resourceAtlas.objectReferenceValue as UIAtlas, _target.resourceSpriteName[i], SelectSpriteForResource, false);
					
					if(string.IsNullOrEmpty(_target.resourceSpriteName[i]))
					{
						EditorGUILayout.HelpBox("Please select a image for resource type", MessageType.Error);
					}

					--EditorGUI.indentLevel;
				}
			}

			if(indexToRemoved >= 0)
			{
				_target.resourceType.RemoveAt(indexToRemoved);
				_target.resourceSpriteName.RemoveAt(indexToRemoved);
				_target.resourceAtlas.RemoveAt(indexToRemoved);

				_resourceTypeFoldout.RemoveAt(indexToRemoved);

				serializedObject.Update();
			}

			GUI.color = Color.green;
			if(GUILayout.Button(new GUIContent("+", "Add new resource image")))
			{
				_target.resourceType.Add(ResourceType.Unknow);
				_target.resourceSpriteName.Add("");
				_target.resourceAtlas.Add(NGUISettings.atlas);

				_resourceTypeFoldout.Add(false);
				
				serializedObject.Update();
			}
			GUI.color = Color.white;
		}

		--EditorGUI.indentLevel;
	}
	#endregion Resource

	public override void OnInspectorGUI()
	{
		DisplayCombatUnit ();

		EditorGUILayout.Space ();

		DisplayResource ();

	}
}
