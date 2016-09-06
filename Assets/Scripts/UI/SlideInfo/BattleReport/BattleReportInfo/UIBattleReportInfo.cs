using UnityEngine;
using System.Collections;

public class UIBattleReportInfo : MonoBehaviour 
{
	public UILabel resultLabel;

	public UILabel winnerIdLabel;

	public UISprite playerAvatarMask;

	public UISprite enemyAvatarMask;

	public UILabel playerUnitFromLabel;

	public UILabel enemyUnitFromLabel;

	public UILabel playerUnitToLabel;

	public UILabel enemyUnitToLabel;

	public UILabel foodEarnLabel;

	public UILabel woodEarnLabel;

	public UISprite buttonSprite;

	public UISprite notifySprite;

	public UIUnitLostDetail unitLostDetail;

	public Color unReadColor = new Color(224f/255f, 50f/255f, 83f/255f);

	private string _reportId;

	private bool _isRead = false;

	private BattleReportInfo _currentInfo;

	private bool _isExpand = false;


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Sets the information.
	/// </summary>
	/// <param name="info">Info.</param>
	public void SetInformation(BattleReportInfo info)
	{
		_reportId = info.infoId;

		_isRead = info.isRead;

		_currentInfo = info;


		resultLabel.text = info.BattleResultStr;

		if(info.battleResut == AttackResult.Win)
		{
			playerAvatarMask.enabled = false;
			enemyAvatarMask.enabled = true;

			winnerIdLabel.text = info.attackerName;
		}
		else if(info.battleResut == AttackResult.Lost)
		{
			playerAvatarMask.enabled = true;
			enemyAvatarMask.enabled = false;

			winnerIdLabel.text = info.targetName;
		}
		else
		{
			playerAvatarMask.enabled = false;
			enemyAvatarMask.enabled = false;

			winnerIdLabel.text = "";
		}

		playerUnitFromLabel.text = info.GetAttackerSentUnitNumber.ToString();
		playerUnitToLabel.text = info.GetAttackerRemainUnitNumber.ToString();

		enemyUnitFromLabel.text = info.GetTargetSentUnitNumber.ToString();
		enemyUnitToLabel.text = info.GetTargetRemainUnitNumber.ToString();


		woodEarnLabel.text = "0";
		foodEarnLabel.text = "0";

		foreach(ResourceType type in info.resourceAward.Keys)
		{
			switch(type)
			{
			case ResourceType.Wood:
				woodEarnLabel.text = ((int)info.resourceAward[type]).ToString();
				break;

			case ResourceType.Food:
				foodEarnLabel.text = ((int)info.resourceAward[type]).ToString();
				break;
			}
		}

		if(!info.isRead)
		{
			buttonSprite.color = unReadColor;
			notifySprite.enabled = true;
		}
		else
		{
			notifySprite.enabled = false;
		}

	}

	/// <summary>
	/// Handle click event.
	/// </summary>
	public void OnClick()
	{
		if(_isExpand)
		{
			_isExpand = !_isExpand;

			OnUnExpand();
		}
		else
		{
			_isExpand = !_isExpand;

			OnExpand();
		}
	}

	/// <summary>
	/// Handle expand event.
	/// </summary>
	void OnExpand()
	{
		unitLostDetail.SetupUnitLostDetailInfo (_currentInfo.attackerUnitLostInfo, _currentInfo.targetUnitLostInfo);

		if(!_isRead)
		{
			BattleReportMetaData data = BattleReportMetaData.Load ();
			
			data.MarkRead (_reportId);

			notifySprite.enabled = false;

			_isRead = true;
		}
	}

	/// <summary>
	/// Handle un expand event.
	/// </summary>
	void OnUnExpand()
	{

	}
	
}
