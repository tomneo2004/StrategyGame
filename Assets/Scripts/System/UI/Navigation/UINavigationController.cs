using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class UINavigationContent : MonoBehaviour
{
	[Tooltip("Title of this content and it should be localized")]
	public string contentTitle = "Content Title";

	public string SetContentTitle
	{
		set
		{
			contentTitle = value;
		}
	}

	public string GetLocalizedContentTitle
	{
		get
		{
			return Localization.Get(contentTitle);
		}
	}

	/// <summary>
	/// Reference to UINavigationController
	/// </summary>
	protected UINavigationController _navigationController;

	/// <summary>
	/// Get navigation controller.
	/// </summary>
	/// <value>The nav controller.</value>
	protected UINavigationController NavController
	{
		get
		{
			return _navigationController;
		}
	}

	protected virtual void Awake()
	{

	}

	protected virtual void Start()
	{

	}

	protected virtual void OnEnable()
	{
	}

	protected virtual void OnDisable()
	{

	}

	protected virtual void Update()
	{
	}

	/// <summary>
	/// Called on Start.
	/// </summary>
	public virtual void InitContent(UINavigationController controller)
	{

		if(controller != null)
		{
			_navigationController = controller;
		}
		else
		{
			Debug.LogError(gameObject.name+" "+this.GetType().Name+" can not find "+typeof(UINavigationController).Name+" in parent gameobject");
		}
	}

	/// <summary>
	/// Called when navigation controller push this content
	/// </summary>
	public virtual void OnContentPushed()
	{
	}

	/// <summary>
	/// Called when navigation controller pop this content
	/// </summary>
	public virtual void OnContentPoped()
	{
	}

	/// <summary>
	/// Called when navigation controller about to display this content
	/// </summary>
	public virtual void OnContentBeginDisplay()
	{
	}

	/// <summary>
	/// Called when navigation controller display this content
	/// </summary>
	public virtual void OnContentDisplay()
	{
	}

	/// <summary>
	/// Called when navigation controller about to hide this content
	/// </summary>
	public virtual void OnContentBeginHide()
	{

	}

	/// <summary>
	/// Called when navigation controller hide this content
	/// </summary>
	public virtual void OnContentHide()
	{
	}

	/// <summary>
	/// Called when navigation controller's back button click
	/// </summary>
	public virtual void OnBackButtonClick()
	{
	}
}

public class UINavigationController : MonoBehaviour 
{
	/// <summary>
	/// The navigation container.
	/// Contatin all content in navigation
	/// </summary>
	[Tooltip("You need to create gameobject that act as container and contain all navigatable contents")]
	public GameObject navigationContainer;

	/// <summary>
	/// The navigation title.
	/// </summary>
	[Tooltip("Navigation controller's title. Do not assign if there is no title")]
	public UILabel navigationTitle;

	/// <summary>
	/// The navigation back button.
	/// </summary>
	[Tooltip("Navigation controller's button that can go back to previous content")]
	public UIButton navigationBackBtn;

	/// <summary>
	/// The auto pop.
	/// </summary>
	[Tooltip("Tell navigation controller whether to auto pop content when back button click or not")]
	public bool autoPop = true;

	/// <summary>
	/// The custom title.
	/// </summary>
	[Tooltip("If true title will not be change by navigation's content, otherwise it will be changed by content")]
	public bool customTitle = false;

	/// <summary>
	/// The root content that will be display at first time.
	/// </summary>
	[Tooltip("The root content that will be display when navigation controller start")]
	public UINavigationContent rootContent;

	public UINavigationContent GetRootContent
	{
		get
		{
			return rootContent;
		}
	}

	/// <summary>
	/// Any designed content should be put in here within inspector,
	/// so navigation controller know the content
	/// </summary>
	[Tooltip("Put all designed content that gameobjec has UINavigationContent")]
	public List<UINavigationContent> DesignedContents;

	/// <summary>
	/// The stack of contents that can be navigated.
	/// </summary>
	private Stack<UINavigationContent> _navigatableContents;

	public UINavigationContent GetCurrentContent
	{
		get
		{
			if(_navigatableContents != null)
			{
				return _navigatableContents.Peek();
			}

			return null;
		}
	}

	protected virtual void Awake()
	{
		if(rootContent == null)
		{
			Debug.LogError(gameObject.name+" "+this.GetType().Name+" has no rootContent");
		}

		if(navigationBackBtn == null)
		{
			Debug.LogWarning(gameObject.name+" "+this.GetType().Name+" back button was not referenced");
		}

		_navigatableContents = new Stack<UINavigationContent> ();

	}

	// Use this for initialization
	protected virtual void Start () 
	{
		//init all contents
		InitAllContents ();

		//push root content
		//PushContent (rootContent);
		_navigatableContents.Push (rootContent);
	}

	protected virtual void  OnEnable()
	{

	}

	protected virtual void OnDisable()
	{

	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
	
	}

	/// <summary>
	/// Register the new content.
	/// </summary>
	/// <param name="content">Content.</param>
	public void RegisterContent(UINavigationContent content)
	{
		if(DesignedContents == null)
		{
			DesignedContents = new List<UINavigationContent>();
		}

		DesignedContents.Add (content);

		content.InitContent (this);
	}

	/// <summary>
	/// Gets the content by type.
	/// </summary>
	/// <returns>The content.</returns>
	/// <param name="content">Content.</param>
	/// <typeparam name="UINavigationContent">The 1st type parameter.</typeparam>
	public UINavigationContent GetContent<T>() where T : UINavigationContent
	{
		if(DesignedContents != null)
		{
			for(int i=0; i<DesignedContents.Count; i++)
			{
				if(DesignedContents[i].GetType() == typeof(T))
				{
					return DesignedContents[i];
				}
			}
		}

		Debug.LogError(gameObject.name+" "+this.GetType().Name+" unable to find "+typeof(T).Name+" did you forgot to register design content");

		return null;
	}

	/// <summary>
	/// Set navigation's title only if customTitle is true.
	/// </summary>
	/// <param name="title">Title.</param>
	public void SetTitle(string title)
	{
		if(customTitle)
		{
			if(navigationTitle != null)
			{
				navigationTitle.text = title;
			}
		}
		else
		{
			Debug.LogWarning(gameObject.name+" customTitle must be true to set title");
		}
	}

	/// <summary>
	/// Init all contents.
	/// </summary>
	void InitAllContents()
	{
		if(DesignedContents != null)
		{
			for(int i=0; i<DesignedContents.Count; i++)
			{
				DesignedContents[i].InitContent(this);
				DesignedContents[i].gameObject.SetActive(false);
			}
		}

	}

	/// <summary>
	/// Check if it should display back button.
	/// </summary>
	void ShouldDisplayBackButton()
	{
		if(navigationBackBtn == null)
		{
			Debug.LogWarning(gameObject.name+" "+this.GetType().Name+" back button was not referenced");

			return;
		}

		//if current content is not root and there are at least 2 contents
		if((GetCurrentContent != rootContent) && (_navigatableContents.Count > 1))
		{
			navigationBackBtn.gameObject.SetActive(true);
		}
		else
		{
			navigationBackBtn.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Updates the title.
	/// </summary>
	void UpdateTitle()
	{
		if(customTitle)
		{
			return;
		}

		if(navigationTitle != null)
		{
			navigationTitle.text = GetCurrentContent.GetLocalizedContentTitle;
		}
	}

	/// <summary>
	/// Shows the content.
	/// </summary>
	void ShowContent()
	{
		navigationContainer.SetActive (true);

		_navigatableContents.Peek ().OnContentBeginDisplay ();
		_navigatableContents.Peek ().gameObject.SetActive (true);
		_navigatableContents.Peek ().OnContentDisplay ();

		UpdateTitle ();
		
		ShouldDisplayBackButton ();
	}

	/// <summary>
	/// Hides the content.
	/// </summary>
	void HideContent()
	{
		_navigatableContents.Peek ().OnContentBeginHide ();
		_navigatableContents.Peek ().gameObject.SetActive (false);
		_navigatableContents.Peek ().OnContentHide ();

		navigationContainer.SetActive (false);
	}

	/// <summary>
	/// Pushs new content.
	/// </summary>
	/// <param name="pushedContent">Pushed content.</param>
	public void PushContent(UINavigationContent pushedContent)
	{
		if(pushedContent != null)
		{
			//if there is at least one content then hide content
			if(_navigatableContents.Count > 0)
			{
				GetCurrentContent.gameObject.SetActive(false);
				GetCurrentContent.OnContentHide();
			}

			//push new content
			_navigatableContents.Push(pushedContent);

			pushedContent.OnContentPushed();

			pushedContent.OnContentBeginDisplay();

			//set content to display
			pushedContent.gameObject.SetActive(true);

			pushedContent.OnContentDisplay();

			UpdateTitle();

			ShouldDisplayBackButton();

		}
		else
		{
			Debug.LogError(gameObject.name+" "+this.GetType().Name+" content param is null");
		}
	}

	/// <summary>
	/// Pop current content.
	/// </summary>
	public void PopContent()
	{
		//if current content is root don't pop
		if(GetCurrentContent == rootContent)
		{
			Debug.LogError(gameObject.name+" "+this.GetType().Name+" can not pop root content");
			return;
		}

		UINavigationContent content = GetCurrentContent;

		//pop current content
		_navigatableContents.Pop ();

		content.OnContentPoped ();

		content.OnContentBeginHide ();

		//set current content not to display
		content.gameObject.SetActive (false);

		content.OnContentHide ();

		//get previous content and set it to display
		GetCurrentContent.gameObject.SetActive (true);
		GetCurrentContent.OnContentDisplay ();

		UpdateTitle ();

		ShouldDisplayBackButton ();
	}

	/// <summary>
	/// Pop content to root.
	/// </summary>
	public void PopContentToRoot()
	{
		if(GetCurrentContent == rootContent)
		{
			return;
		}
		else
		{
			while(true)
			{
				//pop content if it is not root
				if(GetCurrentContent != rootContent)
				{
					PopContent();
				}
				else
				{
					break;
				}
			}
		}
	}

	/// <summary>
	/// Raises the back button click event.
	/// </summary>
	public void OnBackButtonClick()
	{
		GetCurrentContent.OnBackButtonClick ();

		if(autoPop)
		{
			PopContent();
		}
	}

	/// <summary>
	/// Closes the navigation controller.
	/// </summary>
	public virtual void CloseNavigationController()
	{
		while(true)
		{
			//pop content if it is not root
			if(GetCurrentContent != rootContent)
			{
				_navigatableContents.Peek().OnContentBeginHide();
				_navigatableContents.Peek().gameObject.SetActive(false);
				_navigatableContents.Peek().OnContentHide();
				_navigatableContents.Pop();
			}
			else
			{
				break;
			}
		}

		HideContent ();
	}

	/// <summary>
	/// Shows the navigation controller.
	/// </summary>
	public virtual void ShowNavigationController()
	{
		ShowContent ();
	}
}
