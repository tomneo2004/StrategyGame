using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

[CustomEditor( typeof(AttackManager))]
public class AttackManagerEditor : Editor 
{
	AttackManager _target;

	List<bool> _foldout;

	List<int> _removedAIPlayers; 

	List<bool> _combatUnitFoldout;

	List<bool> _resourceAwardFoldout;

	List<int> _removedUnitIndex;

	List<int> _removedResourceAwardIndex;

	public void OnEnable()
	{
		_target = target as AttackManager;

		if(_target.aiPlayers == null)
		{
			_target.aiPlayers = new List<AIPlayer>();
		}

		if(_foldout == null)
		{
			_foldout = new List<bool>();

			for(int i=0; i<_target.aiPlayers.Count; i++)
			{
				_foldout.Add(false);
			}
		}

		if(_combatUnitFoldout == null)
		{
			_combatUnitFoldout = new List<bool>();

			for(int i=0; i<_target.aiPlayers.Count; i++)
			{
				_combatUnitFoldout.Add(false);
			}
		}

		if(_resourceAwardFoldout == null)
		{
			_resourceAwardFoldout = new List<bool>();

			for(int i=0; i<_target.aiPlayers.Count; i++)
			{
				_resourceAwardFoldout.Add(false);
			}
		}

		_removedAIPlayers = new List<int> ();

		_removedUnitIndex = new List<int> ();

		_removedResourceAwardIndex = new List<int> ();
	}

	string GenerateId()
	{
		while(true)
		{
			bool isVaild = true;

			string newId = Guid.NewGuid().ToString();

			for(int i=0; i<_target.aiPlayers.Count; i++)
			{
				if(newId == _target.aiPlayers[i].playerId)
				{
					isVaild = false;

					break;
				}
			}

			if(isVaild)
			{
				return newId;
			}
		}
	}

	void CreateNewAIPlayer()
	{
		string newId = GenerateId ();

		AIPlayer newAIPlayer = new AIPlayer (newId, "New AI Player", 10);

		_target.aiPlayers.Add (newAIPlayer);
		_foldout.Add (true);
		_combatUnitFoldout.Add (false);
	}

	void RemoveAIPlayers()
	{
		for(int i=0; i<_removedAIPlayers.Count; i++)
		{
			_target.aiPlayers.RemoveAt(_removedAIPlayers[i]);
			_foldout.RemoveAt(_removedAIPlayers[i]);
			_combatUnitFoldout.RemoveAt(i);
		}

		_removedAIPlayers.Clear ();
	}

	void DisplayAIPlayer()
	{
		EditorGUILayout.BeginVertical ();

		for(int i=0; i<_target.aiPlayers.Count; i++)
		{
			EditorGUILayout.BeginHorizontal();

			AIPlayer player = _target.aiPlayers[i];

			_foldout[i] = EditorGUILayout.Foldout(_foldout[i], player.playerName);

			GUI.color = Color.red;
			if(GUILayout.Button("X", GUILayout.Width(30f)))
			{
				if(EditorUtility.DisplayDialogComplex("Delete AI Player", "Delete AI player "+player.playerName, "Ok", "Cancel", "") == 0)
				{
					_removedAIPlayers.Add(i);
				}
			}
			GUI.color = Color.white;

			EditorGUILayout.EndHorizontal();

			if(_foldout[i])
			{
				++EditorGUI.indentLevel;

				EditorGUILayout.TextField(new GUIContent("ID:", "AI player id"), player.playerId);

				player.playerName = EditorGUILayout.TextField(new GUIContent("Name:", "AI player name"), player.playerName);

				if(string.IsNullOrEmpty(player.playerName))
				{
					EditorGUILayout.HelpBox("You must give name", MessageType.Error);
				}

				player.attackDuration = EditorGUILayout.IntField(new GUIContent("Attack duratoin:", "How long does it take to attack this player"), player.attackDuration);

				if(player.attackDuration <= 0)
				{
					player.attackDuration = 1;
				}

				_combatUnitFoldout[i] = EditorGUILayout.Foldout(_combatUnitFoldout[i], "CombatUnit");

				//render combat unit gui
				if(_combatUnitFoldout[i])
				{
					++EditorGUI.indentLevel;

					for(int j=0; j<player.combatUnitType.Count; j++)
					{
						EditorGUILayout.BeginHorizontal();

						player.combatUnitType[j] = (CombatUnitType)EditorGUILayout.EnumPopup(player.combatUnitType[j]);

						player.combatUnitNumber[j] = EditorGUILayout.IntField(player.combatUnitNumber[j]);

						GUI.color = Color.red;
						if(GUILayout.Button("X", GUILayout.Width(30f)))
						{
							_removedUnitIndex.Add(j);
						}
						GUI.color = Color.white;

						EditorGUILayout.EndHorizontal();
					}

					foreach(int rIndex in _removedUnitIndex)
					{
						player.combatUnitType.RemoveAt(rIndex);
						player.combatUnitNumber.RemoveAt(rIndex);
					}

					_removedUnitIndex.Clear();


					for(int k=0; k<player.combatUnitType.Count; k++)
					{
						int duplicateCout = 0;

						CombatUnitType compareType = player.combatUnitType[k];

						for(int f=0; f<player.combatUnitType.Count; f++)
						{
							if(compareType == player.combatUnitType[f])
							{
								++duplicateCout;
							}
						}

						if(duplicateCout > 1)
						{
							EditorGUILayout.HelpBox("You need to fix duplicate unit type", MessageType.Error);

							break;
						}
					}

					GUI.color = Color.yellow;
					if(GUILayout.Button(new GUIContent("+", "Add new unit")))
					{
						player.combatUnitType.Add(CombatUnitType.Unknow);
						player.combatUnitNumber.Add(1);
					}
					GUI.color = Color.white;

					--EditorGUI.indentLevel;
				}

				_resourceAwardFoldout[i] = EditorGUILayout.Foldout(_resourceAwardFoldout[i], "ResourceAward");

				//render resource award gui
				if(_resourceAwardFoldout[i])
				{
					--EditorGUI.indentLevel;

					for(int j=0; j<player.resourceAwardType.Count; j++)
					{
						EditorGUILayout.BeginHorizontal();

						player.resourceAwardType[j] = (ResourceType)EditorGUILayout.EnumPopup(player.resourceAwardType[j]);

						player.resourceAwardAmount[j] = (float)EditorGUILayout.IntField((int)player.resourceAwardAmount[j]);

						GUI.color = Color.red;
						if(GUILayout.Button("X", GUILayout.Width(30f)))
						{
							_removedResourceAwardIndex.Add(j);
						}
						GUI.color = Color.white;

						EditorGUILayout.EndHorizontal();
					}

					foreach(int rIndex in _removedResourceAwardIndex)
					{
						player.resourceAwardType.RemoveAt(rIndex);
						player.resourceAwardAmount.RemoveAt(rIndex);
					}

					_removedResourceAwardIndex.Clear();

					for(int k=0; k<player.resourceAwardType.Count; k++)
					{
						int duplicateCout = 0;
						
						ResourceType compareType = player.resourceAwardType[k];
						
						for(int f=0; f<player.resourceAwardType.Count; f++)
						{
							if(compareType == player.resourceAwardType[f])
							{
								++duplicateCout;
							}
						}
						
						if(duplicateCout > 1)
						{
							EditorGUILayout.HelpBox("You need to fix duplicate resource type", MessageType.Error);
							
							break;
						}
					}

					GUI.color = Color.yellow;
					if(GUILayout.Button(new GUIContent("+", "Add new resource award")))
					{
						player.resourceAwardType.Add(ResourceType.Unknow);
						player.resourceAwardAmount.Add(1f);
					}
					GUI.color = Color.white;

					++EditorGUI.indentLevel;
				}

				--EditorGUI.indentLevel; 
			}


		}

		RemoveAIPlayers ();

		GUI.color = Color.green;
		if(GUILayout.Button("Add new AI player"))
		{
			CreateNewAIPlayer();
		}
		GUI.color = Color.white;

		EditorGUILayout.EndVertical ();
	}

	void DispalyAttackTask()
	{
		if(_target.AllAttackTask == null)
		{
			return;
		}

		EditorGUILayout.BeginVertical ();

		for(int i=0; i<_target.AllAttackTask.Count; i++)
		{
			AttackTask task = _target.AllAttackTask[i];

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField(new GUIContent(task.TargetPlayerName));

			EditorGUILayout.LabelField(new GUIContent(task.CurrentDuration.ToString()));

			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndVertical ();
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField(new GUIContent("AI Player"));

		++EditorGUI.indentLevel;

		DisplayAIPlayer ();

		--EditorGUI.indentLevel;

		EditorGUILayout.LabelField(new GUIContent("Tasks"));

		++EditorGUI.indentLevel;

		DispalyAttackTask ();

		--EditorGUI.indentLevel;

		Repaint ();
	}
}
