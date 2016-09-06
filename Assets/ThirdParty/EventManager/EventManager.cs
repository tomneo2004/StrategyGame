using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Game event.
/// Any event must subclass from this one
/// </summary>
public abstract class GameEvent
{

}

/// <summary>
/// Event manager.
/// </summary>
public class EventManager 
{
	/// <summary>
	/// The instance of EventManager.
	/// </summary>
	private static EventManager instance = null;

	//Delegate 
	public delegate void EventDelegate<T>(T e) where T : GameEvent;

	/// <summary>
	/// Contain delegates that is sort by event type.
	/// </summary>
	private Dictionary<System.Type, System.Delegate> delegates = new Dictionary<System.Type, System.Delegate> ();

	/// <summary>
	/// Contain delegate that only one shot and then removed
	/// </summary>
	private List<System.Delegate> shots = new List<System.Delegate>();

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <returns>The instance.</returns>
	public static EventManager GetInstance()
	{
		if(instance == null)
		{
			instance = new EventManager();
		}
		
		return instance;
	}

	/// <summary>
	/// Adds the listener.
	/// </summary>
	/// <param name="del">Del.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
	{

		if(delegates.ContainsKey(typeof(T)))
		{
			System.Delegate tempDel = delegates[typeof(T)];

			delegates[typeof(T)] = System.Delegate.Combine(tempDel, del);
		}
		else
		{
			delegates[typeof(T)] = del;
		}
	}

	/// <summary>
	/// Adds the listerner with one shot, delegate will be remove after it is called.
	/// </summary>
	/// <param name="del">Del.</param>
	/// <param name="oneShot">If set to <c>true</c> one shot.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public void AddListener<T>(EventDelegate<T> del, bool oneShot) where T : GameEvent
	{
		this.AddListener<T> (del);

		if(oneShot)
		{
			shots.Add(del);
		}

	}

	/// <summary>
	/// Removes the listener.
	/// </summary>
	/// <param name="del">Del.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
	{
		if(delegates.ContainsKey(typeof(T)))
		{
			System.Delegate mDel = System.Delegate.Remove(delegates[typeof(T)], del);

			//if delegate is null in this event type then remove this event type...otherwise set modified delegates 
			if(mDel == null)
			{
				delegates.Remove(typeof(T));
			}
			else
			{
				delegates[typeof(T)] = mDel;
			}

			//remove from one shot if needed
			if(shots.Contains(del))
			{
				shots.Remove(del);
			}
		}
	}

	/// <summary>
	/// Executes the event.
	/// </summary>
	/// <param name="e">E.</param>
	public void ExecuteEvent<T>(T e) where T : GameEvent
	{
		if (e == null)
		{
			Debug.LogError("Invalid event argument: " + e.GetType().ToString());
			return;
		}


		System.Delegate d;

		//invoke delegate faster
		if(delegates.TryGetValue(e.GetType(), out d))
		{
			EventDelegate<T> callbacks = d as EventDelegate<T>;

			if(callbacks != null)
			{
				callbacks(e);
			}

			//contain delegates that will be removed in this event type
			System.Delegate rDel = null;

			//get list of delegates in this event type
			System.Delegate[] listDels = delegates[typeof(T)].GetInvocationList();
			
			//find any delegate that is one shot in this event type
			for(int i=0; i<listDels.Length; i++)
			{
				System.Delegate del = listDels[i];

				if(shots.Contains(del))
				{
					shots.Remove(del);
					
					if(rDel == null)
					{
						rDel = del;
					}
					else
					{
						rDel = System.Delegate.Combine(rDel, del);
					}
					
				}
			}
			
			//remove one shot delegate from this event type
			System.Delegate mDel = System.Delegate.Remove (delegates [typeof(T)], rDel);

			//if delegate is null in this event type then remove this event type...otherwise set modified delegates 
			if(mDel == null)
			{
				delegates.Remove(typeof(T));
			}
			else
			{
				delegates [typeof(T)] = mDel;
			}

		}
		else
		{
			Debug.LogWarning("Execute event type: "+ e.GetType().Name+" event was not registered and no match delegates was found");
		}

		/*
		//invoke delegate slow 
		if(delegates.ContainsKey(e.GetType()))
		{
			delegates[e.GetType()].DynamicInvoke(e);
		}
		*/



	}
}
