using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITimePopupController : MonoBehaviour 
{

	private static UITimePopupController _instance;

	public static UITimePopupController Instance
	{
		get
		{
			return _instance;
		}
	}

	public GameObject popupPrefab;

	private Dictionary<int,UITimePopup> _idToPopup = new Dictionary<int, UITimePopup>();

	void Awake()
	{
		_instance = this;
	}

	/// <summary>
	/// Gets the time popup.
	/// Create one if can't find popup
	/// </summary>
	/// <returns>The time popup.</returns>
	/// <param name="transform">Transform.</param>
	/// <param name="showPop">If set to <c>true</c> show pop.</param>
	public UITimePopup GetTimePopup(Transform transform, bool showPop = true)
	{
		if(popupPrefab != null)
		{
			if(_idToPopup.ContainsKey(transform.GetInstanceID()))
			{
				UITimePopup popup = _idToPopup[transform.GetInstanceID()];

				if(showPop)
				{
					popup.gameObject.SetActive(true);
				}

				return popup;
			}
			else
			{
				UITimePopup newPopup = NGUITools.AddChild(gameObject, popupPrefab).GetComponent<UITimePopup>();

				_idToPopup.Add(transform.GetInstanceID(), newPopup);

				newPopup.FollowTarget(transform);

				return newPopup;
			}
		}

		Debug.LogError(gameObject.name+" has no popup prefab assigned");
		return null;
	}

	/// <summary>
	/// Hides the time popup.
	/// </summary>
	/// <param name="transform">Transform.</param>
	public void HideTimePopup(Transform transform)
	{
		if(_idToPopup.ContainsKey(transform.GetInstanceID()))
		{
			_idToPopup[transform.GetInstanceID()].gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Removes the time popup.
	/// </summary>
	/// <param name="transform">Transform.</param>
	public void RemoveTimePopup(Transform transform)
	{
		if(_idToPopup.ContainsKey(transform.GetInstanceID()))
		{
			NGUITools.Destroy(_idToPopup[transform.GetInstanceID()].gameObject);

			_idToPopup.Remove(transform.GetInstanceID());
		}
	}

}
