using UnityEngine;
using System.Collections;

public class UIAttackProgress : MonoBehaviour 
{
	public delegate void OnAttackProgressDestroy();
	public OnAttackProgressDestroy Evt_OnAttackProgressDestroy;

	public UILabel timelabel;

	public UISlider progressBar;

	public UIAttackUnitDetail attackUnitDetail;

	string attackTaskId;

	AttackTask attackTask;

	bool _isExpand = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Start track  attack task.
	/// </summary>
	/// <param name="task">Task.</param>
	public void TrackAttackTask(AttackTask task)
	{
		attackTaskId = task.TaskId;
		attackTask = task;

		task.Evt_OnTimeLeftToComplete += OnUpdateProgress;
		task.Evt_OnAttackComplete += OnAttackTaskComplete;
	}

	/// <summary>
	/// Cancel attack task.
	/// </summary>
	/// <returns><c>true</c> if this instance cancel attack task; otherwise, <c>false</c>.</returns>
	public void CancelAttackTask()
	{
		EventManager.GetInstance ().ExecuteEvent<EventPresentActionConfirm> (new EventPresentActionConfirm ("Warning", "Remove this attack?", "OK", "Cancel", OnActionConfirm));
	}

	/// <summary>
	/// Handle action confirm event.
	/// </summary>
	/// <param name="buttonIndex">Button index.</param>
	void OnActionConfirm(int buttonIndex)
	{
		if(buttonIndex == 0)
		{
			if(AttackManager.Instance.CancelAttack(attackTaskId))
			{
				transform.parent = null;
				
				NGUITools.Destroy (gameObject);
				
				if(Evt_OnAttackProgressDestroy != null)
				{
					Evt_OnAttackProgressDestroy();
				}
			}
		}
	}

	/// <summary>
	/// Handle update progress event.
	/// </summary>
	/// <param name="task">Task.</param>
	/// <param name="secondLeft">Second left.</param>
	void OnUpdateProgress(TimeTask task, int secondLeft)
	{
		timelabel.text = TimeConverter.SecondToTimeString (secondLeft);

		progressBar.value = 1.0f - ((float)task.CurrentTaskDuration / (float)task.TaskDuration);
	}

	/// <summary>
	/// Handle attack task complete event.
	/// </summary>
	/// <param name="task">Task.</param>
	void OnAttackTaskComplete(AttackTask task)
	{
		transform.parent = null;

		NGUITools.Destroy (gameObject);

		if(Evt_OnAttackProgressDestroy != null)
		{
			Evt_OnAttackProgressDestroy();
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
		attackUnitDetail.SetAttackUnitDetail (attackTask);
	}

	/// <summary>
	/// Handle un expand event.
	/// </summary>
	void OnUnExpand()
	{
	}

}
