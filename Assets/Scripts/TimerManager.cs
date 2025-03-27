using System;
using UnityEngine;
using System.Timers;
using System.Collections;
using System.Collections.Generic;

public class TimerManager : MonoBehaviour
{
	public int debugTotalTickTimes = 0;

	private float m_debugTickTime = 0;
    private int m_TimerIdCounter = 1;
	private List<TimerWheel> m_TimerWheels;

	public void Awake()
    {
		m_TimerWheels = new List<TimerWheel>();
		
        m_TimerWheels.Add(new TimerWheel(100, 10, 0)); 			// 100毫秒
        m_TimerWheels.Add(new TimerWheel(1000, 60, 1)); 		// 1秒
		m_TimerWheels.Add(new TimerWheel(60000, 60, 2)); 		// 1分钟
		m_TimerWheels.Add(new TimerWheel(3600000, 24, 3)); 		// 1小时

		for (int i = 0; i < m_TimerWheels.Count - 1; i++)
		{
			m_TimerWheels[i].nextWheel = m_TimerWheels[i + 1];
		}

		for (int i = 1; i < m_TimerWheels.Count; i++)
		{
			m_TimerWheels[i].preWheel = m_TimerWheels[i - 1];
		}
    }

	public void Update()
    {
		m_debugTickTime += Time.deltaTime;
			
		debugTotalTickTimes += 1;
		TickTimerWheel(0);
    }

	private void TickTimerWheel(int index)
	{
		if(index >= m_TimerWheels.Count)
		{
			return;
		}

		m_TimerWheels[index].Tick();
		if(m_TimerWheels[index].CurrentSlot == 0)
		{
			TickTimerWheel(index + 1);
		}
	}

    public int AddTimer(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2)
	{
		m_TimerWheels[0].AddTimer(delay, interval, repeat, callback, param1, param2, m_TimerIdCounter++);
		return m_TimerIdCounter;
	}

	public bool RemoveTimer(int id)
	{
		var timer = TimerWheel.s_TimerMap[id];
		var wheelIndex = timer.WheelIndex;
		return m_TimerWheels[wheelIndex].RemoveTimer(id);
	}

	public bool ModifyTimer(int id, long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2)
	{
		var timer = TimerWheel.s_TimerMap[id];
		var wheelIndex = timer.WheelIndex;
		return m_TimerWheels[wheelIndex].ModifyTimer(id, delay, interval, repeat, callback, param1, param2);
	}

	public void OnDestroy()
	{
		m_TimerWheels.Clear();
	}
}
