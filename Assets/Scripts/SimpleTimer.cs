using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleTimer : MonoBehaviour
{
	public int debugTotalTickTimes = 0;
	private List<Timer> m_Timers = new List<Timer>();
	private List<int> m_SlotListWaitForRemove = new List<int>();
	public static Dictionary<int, Timer> s_TimerMap = new Dictionary<int, Timer>();

	public void Update()
	{
		debugTotalTickTimes += 1;
		m_SlotListWaitForRemove.Clear();
		foreach (var timer in m_Timers)
		{
			//timer.Delay = (long)((float)timer.Delay - Time.deltaTime * 1000);

			//if(timer.Delay <= 0)
			{
				timer.Callback.Invoke(timer.Param1, timer.Param2);
				//m_SlotListWaitForRemove.Add(timer.Id);
			}
		}

		// foreach (var id in m_SlotListWaitForRemove)
		// {
		// 	RemoveTimer(id);
		// }
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
			Callback = callback
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
