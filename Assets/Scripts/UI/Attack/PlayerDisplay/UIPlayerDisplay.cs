using UnityEngine;
using System.Collections;

public class UIPlayerDisplay : MonoBehaviour 
{
	public UILabel playerNameLabel;

	string _playerId;

	public string PlayerId
	{
		get
		{
			return _playerId;
		}
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetName(string name)
	{
		playerNameLabel.text = name;
	}

	public void SetPlayerId(string playerId)
	{
		_playerId = playerId;
	}
}
