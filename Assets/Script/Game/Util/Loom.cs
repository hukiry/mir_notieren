using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;


/// <summary>
/// 多线程和主线程交互使用类
/// </summary>
public class Loom : MonoBehaviour
{
	public static int maxThreads = 8;
	static int numThreads;

	private static Loom _current;
	public static Loom Current
	{
		get
		{
			Initialize();
			return _current;
		}
	}

	void Awake()
	{
		_current = this;
		initialized = true;
	}

	static bool initialized;

	public static void Initialize()
	{
		if (!initialized)
		{
			if (!Application.isPlaying)
				return;
			initialized = true;
			var g = new GameObject("Loom");
			_current = g.AddComponent<Loom>();
			GameObject.DontDestroyOnLoad(g);
		}
	}

	private List<Action> _actions = new List<Action>();
	private struct DelayedQueueItem
	{
		public float time;
		public Action action;
	}
	private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

	List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

	public static void QueueOnMainThread(Action action)
	{
		QueueOnMainThread(0, action);
	}
	public static void QueueOnMainThread(float time, Action action)
	{
		if (time != 0)
		{
			lock (Current._delayed)
			{
				Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
			}
		}
		else
		{
			if (Current)
			{
				lock (Current._actions)
				{
					Current._actions.Add(action);
				}
			}
		}
	}

	//public static Thread RunAsync(Action a)
	//{
	//	Initialize();
	//	while (numThreads >= maxThreads)
	//	{
	//		Thread.Sleep(1);
	//	}
	//	Interlocked.Increment(ref numThreads);
	//	ThreadPool.QueueUserWorkItem(RunAction, a);
	//	return null;
	//}

	//private static void RunAction(object action)
	//{
	//	try
	//	{
	//		((Action)action)();
	//	}
	//	catch
	//	{
	//	}
	//	finally
	//	{
	//		Interlocked.Decrement(ref numThreads);
	//	}

	//}

	void OnDisable()
	{
		if (_current == this)
		{
			_current = null;
		}
	}

	List<Action> _currentActions = new List<Action>();

	// Update is called once per frame
	void Update()
	{
		if (_actions.Count > 0)
		{
			lock (_actions)
			{
				_currentActions.Clear();
				_currentActions.AddRange(_actions);
				_actions.Clear();
			}
			foreach (var a in _currentActions)
			{
				a();
			}
		}
		if (_delayed.Count > 0)
		{
			lock (_delayed)
			{
				_currentDelayed.Clear();
				_currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
				foreach (var item in _currentDelayed)
					_delayed.Remove(item);
			}
			foreach (var delayed in _currentDelayed)
			{
				delayed.action();
			}
		}
	}

	public void Destroy()
	{
		Update();
		_currentActions.Clear();
		_actions.Clear();
		initialized = false;
		GameObject.Destroy(gameObject);
	}
}
