using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleTimer : MonoBehaviour
{
	public int debugTotalTickTimes = 0;

	private float m_debugTickTime = 0;
	private List<Timer> m_Timers = new List<Timer>();
	public static Dictionary<int, Timer> s_TimerMap = new Dictionary<int, Timer>();

	public void Update()
	{
		m_debugTickTime += Time.deltaTime;

		while(m_debugTickTime > 0.1f)
		{
			m_debugTickTime -= 0.1f;
			debugTotalTickTimes += 1;
			Tick();
		}
	}

	public void Tick()
	{
		debugTotalTickTimes += 1;
		foreach (var timer in m_Timers)
		{
			if(timer.IsDisposed)
			{
				continue;
			}

			timer.Delay -= 100;

			if(timer.Delay <= 0)
			{
				if(timer.Callback != null)
				{
					timer.Callback.Invoke(timer.Param1, timer.Param2);
				}

				timer.IsDisposed = true;
			}
		}
	}

	public int AddTimer(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2, int id)
	{
		m_Timers.Add(new Timer()
		{
			Id = id,
			Delay = delay,
			Interval = interval,
			Repeat = repeat,
			Param1 = param1,
			Param2 = param2,
			Callback = callback,
			IsDisposed = false,
		});
		s_TimerMap[id] = m_Timers[m_Timers.Count - 1];
		return id;
	}

	public bool RemoveTimer(int id)
	{
		if(!s_TimerMap.ContainsKey(id))
		{
			return false;
		}
		var timer = s_TimerMap[id];
		m_Timers.Remove(timer);
		s_TimerMap.Remove(id);
		return true;
	}

	public bool ModifyTimer(int id, long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2)
	{
		if(!RemoveTimer(id))
		{
			return false;
		}
		AddTimer(delay, interval, repeat, callback, param1, param2,id);
		return true;
	}
}
