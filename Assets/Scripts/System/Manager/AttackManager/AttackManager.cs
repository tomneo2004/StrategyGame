using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class AIPlayer
{
	public string playerId;

	public string playerName;

	public int attackDuration;

	public List<CombatUnitType> combatUnitType = new List<CombatUnitType>();

	public List<int> combatUnitNumber = new List<int>();

	public List<ResourceType> resourceAwardType = new List<ResourceType> ();

	public List<float> resourceAwardAmount = new List<float> ();

	public AIPlayer(string id, string name, int duration)
	{
		playerId = id;
		playerName = name;
		attackDuration = duration;
	}

	/// <summary>
	/// Get this ai player combat unit info.
	/// </summary>
	/// <returns>The player combat unit info.</returns>
	public Dictionary<CombatUnit, int> GetPlayerCombatUnitInfo()
	{
		Dictionary<CombatUnit, int> retVal = new Dictionary<CombatUnit, int> ();

		for(int i=0; i<combatUnitType.Count; i++)
		{
			CombatUnit unit = CombatUnitManager.Instance.GetCombatUnitInfoByType(combatUnitType[i]);
			int number = combatUnitNumber[i];

			retVal.Add(unit, number);
		}

		return retVal;
	}
}

[System.Serializable]
public class AttackManager : MonoBehaviour 
{
	static AttackManager _instance;

	public static AttackManager Instance
	{
		get
		{
			AttackManager atkMgr = GameObject.FindObjectOfType<AttackManager>();
			
			if(atkMgr != null)
			{
				_instance = atkMgr;
			}
			else
			{
				_instance = null;

				Debug.LogError("You must create AttackManager in scene");
			}
			
			return _instance;
		}
	}

	public List<AIPlayer> aiPlayers;

	private List<AttackTask> _attackTasks = new List<AttackTask>();

	public List<AttackTask> AllAttackTask
	{
		get
		{
			return _attackTasks;
		}
	}

	// Use this for initialization
	void Start () 
	{
		DoFetchData ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Fetch data from meta data.
	/// </summary>
	void DoFetchData()
	{
		AttackManagerMetaData data = AttackManagerMetaData.Load (true);

		AttackInfo[] infos = data.GetAttackInfo ();

		for(int i=0; i<infos.Length; i++)
		{
			AttackInfo info = infos[i];

			AttackTask task = AttackTask.CreateTask(info.taskId, info.targetId, info.targetName, info.targetUnit, info.attackerUnit,
			                                        OnAttackTaskComplete, null, info.duration);

			_attackTasks.Add(task);

			TaskManager.Instance.AddTask(task);

			EventManager.GetInstance().ExecuteEvent<EventNewAttack>(new EventNewAttack(task));
		}
	}

	/// <summary>
	/// Get AI player information by id.
	/// </summary>
	/// <returns>The AI player.</returns>
	/// <param name="playerId">Player identifier.</param>
	public AIPlayer GetAIPlayer(string playerId)
	{
		for(int i=0; i<aiPlayers.Count; i++)
		{
			if(playerId == aiPlayers[i].playerId)
			{
				return aiPlayers[i];
			}
		}

		return null;
	}

	string GenerateTaskId()
	{
		while(true)
		{
			bool isVaild = true;
			
			string newId = Guid.NewGuid().ToString();

			for(int i=0; i<_attackTasks.Count; i++)
			{
				if(newId == _attackTasks[i].TaskId)
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

	/// <summary>
	/// Get all AI players.
	/// </summary>
	/// <returns>The AI player.</returns>
	public AIPlayer[] AllAIPlayer()
	{
		return aiPlayers.ToArray ();
	}

	/*
	public bool AttackPlayer(string playerId, int sentUnit)
	{
		AIPlayer aiPlayer = GetAIPlayer (playerId);

		if(aiPlayer != null)
		{
			AttackManagerMetaData data = AttackManagerMetaData.Load();

			string taskId = GenerateTaskId();

			if(data.AddAttackTask(taskId, aiPlayer.playerId, aiPlayer.playerName, aiPlayer.totalCombatUnit, sentUnit, aiPlayer.attackDuration))
			{
				AttackTask task = AttackTask.CreateTask(taskId, aiPlayer.playerId, aiPlayer.playerName, aiPlayer.totalCombatUnit, OnAttackTaskComplete, null, aiPlayer.attackDuration);

				_attackTasks.Add(task);
				
				TaskManager.Instance.AddTask(task);

				EventManager.GetInstance().ExecuteEvent<EventNewAttack>(new EventNewAttack(task));

				return true;
			}
		}
		else
		{
			Debug.LogError("Attack player with id "+playerId+" not exist");
		}

		return false;
	}
	*/

	/// <summary>
	/// Attacks player.
	/// </summary>
	/// <returns><c>true</c>, if player was attacked, <c>false</c> otherwise.</returns>
	/// <param name="playerId">Player identifier.</param>
	/// <param name="sentUnit">Sent unit.</param>
	public bool AttackPlayer(string playerId, Dictionary<CombatUnitType, int> sentUnit)
	{
		//todo:send unit

		AIPlayer aiPlayer = GetAIPlayer (playerId);
		
		if(aiPlayer != null)
		{
			AttackManagerMetaData data = AttackManagerMetaData.Load();
			
			string taskId = GenerateTaskId();

			//attacker(player) unit
			Dictionary<CombatUnit, int> dic = new Dictionary<CombatUnit, int>();

			foreach(CombatUnitType type in sentUnit.Keys)
			{
				if(sentUnit[type] > 0)
				{
					CombatUnit unit = CombatUnitManager.Instance.GetCombatUnitInfoByType(type);
					
					dic.Add(unit, sentUnit[type]);
				}
			}

			Dictionary<ResourceType, float> resoruceAward = new Dictionary<ResourceType, float>();

			for(int i=0; i<aiPlayer.resourceAwardType.Count; i++)
			{
				resoruceAward.Add(aiPlayer.resourceAwardType[i], aiPlayer.resourceAwardAmount[i]);
			}

			if(data.AddAttackTask(taskId, aiPlayer.playerId, aiPlayer.playerName, aiPlayer.GetPlayerCombatUnitInfo(), dic, aiPlayer.attackDuration, resoruceAward))
			{
				AttackTask task = AttackTask.CreateTask(taskId, aiPlayer.playerId, aiPlayer.playerName, aiPlayer.GetPlayerCombatUnitInfo(), dic,
				                                        OnAttackTaskComplete, null, aiPlayer.attackDuration);
				
				_attackTasks.Add(task);
				
				TaskManager.Instance.AddTask(task);
				
				EventManager.GetInstance().ExecuteEvent<EventNewAttack>(new EventNewAttack(task));
				
				return true;
			}
		}
		else
		{
			Debug.LogError("Attack player with id "+playerId+" not exist");
		}
		
		return false;
	}

	/// <summary>
	/// cancel specific attack task by id.
	/// </summary>
	/// <returns><c>true</c> if this instance cancel attack the specified attackTaskId; otherwise, <c>false</c>.</returns>
	/// <param name="attackTaskId">Attack task identifier.</param>
	public bool CancelAttack(string attackTaskId)
	{
		for(int i=0; i<_attackTasks.Count; i++)
		{
			if(_attackTasks[i].TaskId == attackTaskId)
			{
				return _attackTasks[i].CancelTask();
			}
		}

		return false;
	}

	/// <summary>
	/// Handle attack task complete.
	/// </summary>
	/// <param name="task">Task.</param>
	void OnAttackTaskComplete(AttackTask task)
	{
		_attackTasks.Remove (task);
	}
}
