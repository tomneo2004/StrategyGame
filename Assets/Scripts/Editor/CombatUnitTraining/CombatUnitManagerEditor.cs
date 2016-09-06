using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(CombatUnitManager))]
public class CombatUnitManagerEditor : Editor 
{
	CombatUnitManager _target;

	List<bool> _foldout;//foldout for CombatUnit

	List<bool> _resourceFoldout;//foldout for CombtUnit's resource

	List<bool> _unitAgainstFoldout;//foldout for CombatUnit's Unit against

	List<bool> _fearUnitFoldout;

	List<CombatUnit> _removedCombatUnits;//contain all removed combat units

	int currentUnitIndex = 0;//which unit info we are current modifying

	public void OnEnable()
	{
		_target = target as CombatUnitManager;

		if(_target == null)
		{
			_target.combatUnitInfo = new List<CombatUnit> ();
		}

		_foldout = new List<bool> ();
		
		_resourceFoldout = new List<bool> ();

		_unitAgainstFoldout = new List<bool> ();

		_fearUnitFoldout = new List<bool> ();
		
		for(int i=0; i<_target.combatUnitInfo.Count; i++)
		{
			_foldout.Add(false);
			_resourceFoldout.Add(false);
			_unitAgainstFoldout.Add(false);
			_fearUnitFoldout.Add(false);
		}
		
		
		_removedCombatUnits = new List<CombatUnit> ();
	}

	//add new CombatUnit info
	void AddNewCombatUnit()
	{
		CombatUnit newUnit = new CombatUnit ();

		//_target.combatUnitAtlas.Add (NGUISettings.atlas);
		serializedObject.Update ();


		_target.combatUnitInfo.Add (newUnit);

		_foldout.Add (true);
		_resourceFoldout.Add (false);
		_unitAgainstFoldout.Add (false);
		_fearUnitFoldout.Add (false);

		serializedObject.Update ();
	}

	//remove CombatUnit info
	void RemoveCombatUnit()
	{
		for(int i =0; i<_removedCombatUnits.Count; i++)
		{
			int foldoutIndex = _target.combatUnitInfo.IndexOf(_removedCombatUnits[i]);
			
			_foldout.RemoveAt(foldoutIndex);
			_resourceFoldout.RemoveAt(foldoutIndex);
			_unitAgainstFoldout.RemoveAt(foldoutIndex);
			_fearUnitFoldout.RemoveAt(foldoutIndex);

			//_target.combatUnitAtlas.RemoveAt(i);
			_target.combatUnitInfo.Remove(_removedCombatUnits[i]);
		}
		_removedCombatUnits.Clear ();

		serializedObject.Update ();
	}

	//add new resource cost to CombatUnit
	void CombatUnitAddResourceCost(int unitIndex)
	{
		CombatUnit unit = _target.combatUnitInfo [unitIndex];

		unit.costResourceTypes.Add (ResourceType.Unknow);
		unit.costResources.Add (1.0f);

		serializedObject.Update ();
	}

	//remove resource cost from CombatUnit
	void CombatUnitRemoveResourceCost(int unitIndex, int resourceIndex)
	{
		CombatUnit unit = _target.combatUnitInfo [unitIndex];

		unit.costResourceTypes.RemoveAt (resourceIndex);
		unit.costResources.RemoveAt (resourceIndex);

		serializedObject.Update ();
	}

	void CombatUnitAddUnitAgainstType(int unitIndex)
	{
		CombatUnit unit = _target.combatUnitInfo [unitIndex];

		unit.unitAgainst.Add (CombatUnitType.Unknow);

		serializedObject.Update ();
	}

	void CombatUnitRemoveUnitAgainstType(int unitIndex, int againstTypeIndex)
	{
		CombatUnit unit = _target.combatUnitInfo [unitIndex];

		unit.unitAgainst.RemoveAt (againstTypeIndex);

		serializedObject.Update ();
	}

	void CombatUnitAddFearUnitType(int unitIndex)
	{
		CombatUnit unit = _target.combatUnitInfo [unitIndex];

		unit.fearUnit.Add (CombatUnitType.Unknow);

		serializedObject.Update ();
	}

	void CombatUnitRemoveFearUnitType(int unitIndex, int fearTypeIndex)
	{
		CombatUnit unit = _target.combatUnitInfo [unitIndex];

		unit.fearUnit.RemoveAt (fearTypeIndex);

		serializedObject.Update ();
	}

	//check if there is duplicate resource type in CombatUnit
	bool CheckDuplicateResourceCostType(int unitIndex)
	{
		CombatUnit unit = _target.combatUnitInfo [unitIndex];

		for(int i=0; i<unit.costResourceTypes.Count; i++)
		{
			ResourceType comparedType = unit.costResourceTypes[i];

			int duplicateTime = 0;

			for(int j=0; j<unit.costResourceTypes.Count; j++)
			{
				if(comparedType == unit.costResourceTypes[j])
				{
					++duplicateTime;
				}
			}

			if(duplicateTime > 1)
			{
				return true;
			}
		}

		return false;
	}

	bool CheckDuplicateUnitAgainstType(int unitIndex)
	{
		CombatUnit unit = _target.combatUnitInfo [unitIndex];

		for(int i=0; i<unit.unitAgainst.Count; i++)
		{
			CombatUnitType compareType = unit.unitAgainst[i];

			int duplicateTime = 0;

			for(int j=0; j<unit.unitAgainst.Count; j++)
			{
				if(compareType == unit.unitAgainst[j])
				{
					++duplicateTime;
				}
			}

			if(duplicateTime > 1)
			{
				return true;
			}
		}

		return false;
	}

	bool CheckDuplicateFearUnitType(int unitIndex)
	{
		CombatUnit unit = _target.combatUnitInfo [unitIndex];

		for(int i=0; i<unit.fearUnit.Count; i++)
		{
			CombatUnitType compareType = unit.fearUnit[i];

			int duplicateTime = 0;

			for(int j=0; j<unit.fearUnit.Count; j++)
			{
				if(compareType == unit.fearUnit[j])
				{
					++duplicateTime;
				}
			}

			if(duplicateTime > 1)
			{
				return true;
			}
		}

		return false;
	}

	//only allow one unit can be edit at a time
	void UnfoldoutExcept(int index)
	{
		for(int i=0; i<_foldout.Count; i++)
		{
			if(index != i)
			{
				_foldout[i] = false;
			}
		}
	}

	void DisplayCombatUnit()
	{

		for(int i=0; i<_target.combatUnitInfo.Count; i++)
		{

			CombatUnit info = _target.combatUnitInfo[i];
			
			EditorGUILayout.BeginHorizontal();
			
			_foldout[i] = EditorGUILayout.Foldout(_foldout[i], info.unitType.ToString());

			//remove CombatUnit button
			GUI.color = Color.red;
			if(GUILayout.Button("X", GUILayout.Width(30f)))
			{
				if(EditorUtility.DisplayDialogComplex("Delete Unit", "Delete combat unit "+info.unitType.ToString(), "Ok", "Cancel", "") == 0)
				{
					_removedCombatUnits.Add(info);
				}
			}
			GUI.color = Color.white;
			
			EditorGUILayout.EndHorizontal();


			//display CombatUnit detail
			if(_foldout[i])
			{
				UnfoldoutExcept(i);

				currentUnitIndex = i;

				++EditorGUI.indentLevel;

				info.unitName = EditorGUILayout.TextField(new GUIContent("Unit name", "Name of this unit"), info.unitName);

				if(string.IsNullOrEmpty(info.unitName))
				{
					EditorGUILayout.HelpBox("You have give a name to this unit", MessageType.Error);
				}

				EditorGUILayout.LabelField(new GUIContent("Unit description", "Description of this unit"));

				GUIStyle style = new GUIStyle(EditorStyles.textField);
				float height = style.CalcHeight(new GUIContent(info.unitDesc), Screen.width - 100f);
				Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(height));

				info.unitDesc = EditorGUI.TextArea(rect, info.unitDesc, style);

				if(string.IsNullOrEmpty(info.unitDesc))
				{
					EditorGUILayout.HelpBox("Give a description to this unit", MessageType.Warning);
				}

				info.minDamage = EditorGUILayout.IntField(new GUIContent("Min damage", "Minimum damage of this unit"), info.minDamage);

				info.maxDamage = EditorGUILayout.IntField(new GUIContent("Max damage", "Maximum damage of this unit"), info.maxDamage);

				info.minDefence = EditorGUILayout.IntField(new GUIContent("Min defence", "Minimum defence of this unit"), info.minDefence);

				info.maxDefence = EditorGUILayout.IntField(new GUIContent("Max defence", "Maximum defence of this unit"), info.maxDefence);
				
				info.unitType = (CombatUnitType)EditorGUILayout.EnumPopup(new GUIContent("Unit type:", "Type of this unit"), info.unitType);

				info.generateDuration = EditorGUILayout.IntField(new GUIContent("Generate duration:", "How long does it take to produce this type of unit"), info.generateDuration);

				_resourceFoldout[i] = EditorGUILayout.Foldout(_resourceFoldout[i], new GUIContent("Resource cost", "What type of resource and how many resource do this unit cost to produce 1"));

				//display CombatUnit's resource cost
				if(_resourceFoldout[i])
				{
					++EditorGUI.indentLevel;
					
					for(int j=0; j<info.costResourceTypes.Count; j++)
					{
						EditorGUILayout.BeginHorizontal();
						
						info.costResourceTypes[j] = (ResourceType)EditorGUILayout.EnumPopup(info.costResourceTypes[j]);
						
						info.costResources[j] = EditorGUILayout.FloatField(info.costResources[j]);

						//clamp at least 1 resource cost
						info.costResources[j] = Mathf.Clamp(info.costResources[j], 1f, info.costResources[j]+1f);
						
						GUI.color = Color.red;
						if(GUILayout.Button("X"))
						{
							CombatUnitRemoveResourceCost(i, j);
						}
						GUI.color = Color.white;
						
						EditorGUILayout.EndHorizontal();
					}

					//check if duplicate resource type
					if(CheckDuplicateResourceCostType(i))
					{
						EditorGUILayout.HelpBox("You have duplicate resource type that you need to fix", MessageType.Error);
					}
					
					GUI.color = Color.yellow;
					if(GUILayout.Button(new GUIContent("+", "Add new resource cost")))
					{
						CombatUnitAddResourceCost(i);
					}
					GUI.color = Color.white;
					
					--EditorGUI.indentLevel;
				}

				_unitAgainstFoldout[i] = EditorGUILayout.Foldout(_unitAgainstFoldout[i], new GUIContent("Unit against", "What type of unit this unit against with"));

				//display CombatUnit's unit against type
				if(_unitAgainstFoldout[i])
				{
					++EditorGUI.indentLevel;

					for(int j=0; j<info.unitAgainst.Count; j++)
					{
						EditorGUILayout.BeginHorizontal();

						info.unitAgainst[j] = (CombatUnitType)EditorGUILayout.EnumPopup(info.unitAgainst[j]);

						GUI.color = Color.red;
						if(GUILayout.Button("X", GUILayout.Width(30f)))
						{
							CombatUnitRemoveUnitAgainstType(i, j);
						}
						GUI.color = Color.white;

						EditorGUILayout.EndHorizontal();
					}

					if(CheckDuplicateUnitAgainstType(i))
					{
						EditorGUILayout.HelpBox("You have duplicate unit against type that you need to fix", MessageType.Error);
					}

					GUI.color = Color.yellow;
					if(GUILayout.Button(new GUIContent("+", "Add new unit against type")))
					{
						CombatUnitAddUnitAgainstType(i);
					}
					GUI.color = Color.white;

					--EditorGUI.indentLevel;
				}

				_fearUnitFoldout[i] = EditorGUILayout.Foldout(_fearUnitFoldout[i], new GUIContent("Fear unit", "What type of unit this unit afraid of"));

				if(_fearUnitFoldout[i])
				{
					++EditorGUI.indentLevel;

					for(int j=0; j<info.fearUnit.Count; j++)
					{
						EditorGUILayout.BeginHorizontal();

						info.fearUnit[j] = (CombatUnitType)EditorGUILayout.EnumPopup(info.fearUnit[j]);

						GUI.color = Color.red;
						if(GUILayout.Button("X", GUILayout.Width(30f)))
						{
							CombatUnitRemoveFearUnitType(i, j);
						}
						GUI.color = Color.white;

						EditorGUILayout.EndHorizontal();
					}

					if(CheckDuplicateFearUnitType(i))
					{
						EditorGUILayout.HelpBox("You have duplicate fear unit type that yoy need to fix", MessageType.Error);
					}

					GUI.color = Color.yellow;
					if(GUILayout.Button(new GUIContent("+", "Add new fear unit type")))
					{
						CombatUnitAddFearUnitType(i);
					}
					GUI.color = Color.white;


					--EditorGUI.indentLevel;
				}
				
				--EditorGUI.indentLevel;
			}


			EditorGUILayout.Space();
		}
	}

	void DisplayCombatUnitManager()
	{
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.LabelField(new GUIContent("Unit defination", "This area is for defining your combat unit"));

		++EditorGUI.indentLevel;

		DisplayCombatUnit ();

		RemoveCombatUnit ();

		EditorGUILayout.Space();

		//add new CombatUnit info button
		GUI.color = Color.green;
		if(GUILayout.Button("Add combat unit"))
		{
			AddNewCombatUnit();
		}
		GUI.color = Color.white;

		--EditorGUI.indentLevel;

		EditorGUILayout.EndVertical ();
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.BeginVertical ();

		EditorGUILayout.LabelField(new GUIContent("Info", "This area is for debug only"));

		++EditorGUI.indentLevel;

		EditorGUILayout.IntField (new GUIContent ("Max combat unit:", "Max combat unit, can not change"), _target.maxCombatUnit);

		EditorGUILayout.IntField (new GUIContent ("Current combat unit:", "Current combat unit, can not change"), _target.currentCombatUnit);

		--EditorGUI.indentLevel;

		EditorGUILayout.EndVertical ();

		EditorGUILayout.Space ();

		DisplayCombatUnitManager ();
	}
}
