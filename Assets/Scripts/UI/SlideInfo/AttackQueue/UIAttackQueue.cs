using UnityEngine;
using System.Collections;

public class UIAttackQueue : MonoBehaviour 
{
	public UIScrollView scrollview;

	public UITable table;

	public UIContainer container;

	public GameObject attackTaskTrackPrefab;

	// Use this for initialization
	void Start () 
	{
		container.Evt_OnContainerActive = OnContainerActive;
	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventNewAttack> (OnNewAttack);
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventNewAttack> (OnNewAttack);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Handle new attack event.
	/// </summary>
	/// <param name="e">E.</param>
	void OnNewAttack(EventNewAttack e)
	{
		GameObject newAttackTrack = NGUITools.AddChild (table.gameObject, attackTaskTrackPrefab);

		table.Reposition ();

		UIAttackProgress progress = newAttackTrack.GetComponent<UIAttackProgress> ();

		progress.Evt_OnAttackProgressDestroy = OnAttackProgressDestroy;

		progress.TrackAttackTask (e.newAttackTask);
	}

	/// <summary>
	/// Handle container active event.
	/// </summary>
	/// <param name="container">Container.</param>
	void OnContainerActive(UIContainer container)
	{
		table.Reposition ();

		scrollview.ResetPosition ();
	}

	/// <summary>
	/// Handle attack progress destroy event.
	/// </summary>
	void OnAttackProgressDestroy()
	{
		table.Reposition ();

		scrollview.ResetPosition ();
	}
}
