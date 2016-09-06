using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIFollowTarget))]
public class UIArchitectureMenu : MonoBehaviour
{
	protected Transform _target;

	 protected UIFollowTarget _followTarget;

	protected void Awake()
	{
		_followTarget = GetComponent<UIFollowTarget> ();
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	public virtual void SetTargetToFollow(Transform trans)
	{
		_target = trans;

		_followTarget.Target = trans;
	}
}

public enum ArchitectureMenuType
{
	Unknow,
	BarrackNormal,
	OnTask
}

[System.Serializable]
public class UIArchitectureMenuController : MonoBehaviour 
{
	private static UIArchitectureMenuController _instance;

	public static UIArchitectureMenuController Instance
	{
		get
		{
			return _instance;
		}
	}

	public List<ArchitectureMenuType> menuTypes;

	public List<GameObject> menuPrefabs;

	private Dictionary<int , Dictionary<ArchitectureMenuType, UIArchitectureMenu>> _idToMenu;

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		_idToMenu = new Dictionary<int, Dictionary<ArchitectureMenuType, UIArchitectureMenu>> ();
	}

	void OnEnable()
	{
		EventManager.GetInstance ().AddListener<EventInputOnObjectSelected> (OnGameObjectSelected);
	}

	void OnDisable()
	{
		EventManager.GetInstance ().RemoveListener<EventInputOnObjectSelected> (OnGameObjectSelected);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	GameObject GetPrefab(ArchitectureMenuType menuType)
	{
		//find index
		int index = menuTypes.IndexOf (menuType);

		if(index >= 0)
		{
			return menuPrefabs[index];
		}
		else
		{
			Debug.LogError(gameObject.name+" cannot find prefab of menu type of "+menuType.ToString());
		}

		return null;
	}

	/// <summary>
	/// Creates the menu.
	/// </summary>
	/// <returns>The menu.</returns>
	/// <param name="transform">Transform.</param>
	/// <param name="menuType">Menu type.</param>
	UIArchitectureMenu CreateMenu(Transform transform, ArchitectureMenuType menuType)
	{
		GameObject go = NGUITools.AddChild (gameObject, GetPrefab (menuType));
		UIArchitectureMenu newMenu = go.GetComponent<UIArchitectureMenu> ();

		//if dictionary of menu type is exist
		if(_idToMenu[transform.GetInstanceID()] != null)
		{
			_idToMenu[transform.GetInstanceID()].Add(menuType, newMenu);
		}
		else//dictionary of menu type is not exist
		{
			Dictionary<ArchitectureMenuType, UIArchitectureMenu> newDic = new Dictionary<ArchitectureMenuType, UIArchitectureMenu>();

			newDic.Add(menuType, newMenu);

			_idToMenu[transform.GetInstanceID()] = newDic;
		}

		newMenu.SetTargetToFollow (transform);

		return newMenu;
	}

	/// <summary>
	/// Hides the menu except.
	/// </summary>
	/// <param name="transform">Transform.</param>
	/// <param name="menuType">Menu type.</param>
	void HideMenuExcept(Transform transform, ArchitectureMenuType menuType)
	{
		foreach(int id in _idToMenu.Keys)
		{
			foreach(ArchitectureMenuType type in _idToMenu[id].Keys)
			{
				if((id == transform.GetInstanceID()) && (type == menuType))
				{
					continue;
				}

				_idToMenu[id][type].gameObject.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Get menu that is bind to certain architecture gameboject.
	/// It will automatically create new one if it can not found.
	/// </summary>
	/// <returns>The menu.</returns>
	/// <param name="transform">Transform.</param>
	/// <param name="menuType">Menu type.</param>
	/// <param name="hideOther">If set to <c>true</c> hide other.</param>
	public UIArchitectureMenu GetMenu(Transform transform, ArchitectureMenuType menuType, bool hideOther = true)
	{
		//if other menu need to hide
		if(hideOther)
		{
			HideMenuExcept(transform, menuType);
		}

		//if it has object id
		if(_idToMenu.ContainsKey(transform.GetInstanceID()))
		{
			//if it contain certain menu type...return that menu
			if(_idToMenu[transform.GetInstanceID()].ContainsKey(menuType))
			{
				UIArchitectureMenu retMenu = _idToMenu[transform.GetInstanceID()][menuType];

				//set menu active
				retMenu.gameObject.SetActive(true);

				return retMenu;
			}
			else//it does not contatin certain menu type...create new menu
			{
				return CreateMenu(transform, menuType);
			}
		}
		else//it does not have object id
		{
			//add object id
			_idToMenu.Add(transform.GetInstanceID(), null);

			return CreateMenu(transform, menuType);
		}
	}

	/// <summary>
	/// Hides all menu.
	/// </summary>
	public void HideAllMenu()
	{
		foreach(int id in _idToMenu.Keys)
		{
			foreach(ArchitectureMenuType type in _idToMenu[id].Keys)
			{	
				_idToMenu[id][type].gameObject.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Hide all menu that are bind to certain architecture gameboject.
	/// </summary>
	/// <param name="transform">Transform.</param>
	public void HideMenu(Transform transform)
	{
		foreach(ArchitectureMenuType type in _idToMenu[transform.GetInstanceID()].Keys)
		{
			_idToMenu[transform.GetInstanceID()][type].gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Hide all menu that are bind to certain architecture gameboject except certain menu type.
	/// </summary>
	/// <param name="transform">Transform.</param>
	/// <param name="menuType">Menu type.</param>
	public void HideMenu(Transform transform, ArchitectureMenuType menuType)
	{
		foreach(ArchitectureMenuType type in _idToMenu[transform.GetInstanceID()].Keys)
		{
			if(type == menuType)
			{
				_idToMenu[transform.GetInstanceID()][type].gameObject.SetActive(true);
				continue;
			}

			_idToMenu[transform.GetInstanceID()][type].gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Delete all menu that are bind to certain architecture gameboject.
	/// </summary>
	/// <param name="transform">Transform.</param>
	public void DeleteMenu(Transform transform)
	{
		foreach(ArchitectureMenuType type in _idToMenu[transform.GetInstanceID()].Keys)
		{
			NGUITools.Destroy(_idToMenu[transform.GetInstanceID()][type].gameObject);
		}

		_idToMenu.Remove (transform.GetInstanceID ());
	}

	/// <summary>
	/// Delete all menu that are bind to certain architecture gameboject except certain menu type.
	/// </summary>
	/// <param name="transform">Transform.</param>
	/// <param name="menuType">Menu type.</param>
	public void DeleteMenu(Transform transform, ArchitectureMenuType menuType)
	{
		foreach(ArchitectureMenuType type in _idToMenu[transform.GetInstanceID()].Keys)
		{
			if(type == menuType)
			{
				continue;
			}

			NGUITools.Destroy(_idToMenu[transform.GetInstanceID()][type].gameObject);
		}
	}

	void OnGameObjectSelected(EventInputOnObjectSelected e)
	{
		if(e.selectedObject.GetComponent<Architecture>() == null)
		{
			HideAllMenu();
		}
	}
}
