using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerWheel 
{
	private readonly int m_TickMs = 0;
	private readonly int m_SlotCount = 0;
	private readonly List<Timer>[] m_SlotsArray;

	private int m_CurrentSlot = 0;
	private int m_TimerIdCounter = 1;
	private Dictionary<int, Timer> m_TimerMap = new Dictionary<int, Timer>();
	
	private List<Timer> m_SlotListWaitForAdd = new List<Timer>();
	private List<int> m_SlotListWaitForRemove = new List<int>();
	public TimerWheel(int tickMs, int slotCount)
	{
		m_TickMs = tickMs;
		m_SlotCount = slotCount;

		m_SlotsArray = new List<Timer>[slotCount];
		for (int i = 0; i < slotCount; i++)
		{
			m_SlotsArray[i] = new List<Timer>();
		}
	}

	public void Tick()
	{
		m_CurrentSlot = (m_CurrentSlot + 1) % m_SlotCount;

		foreach (var timer in m_SlotsArray[m_CurrentSlot])
		{
			if(timer.RemainingRounds > 0)
			{
				timer.RemainingRounds--;
				continue;
			}

			timer.Callback.Invoke(timer.Param1, timer.Param2);

			if(timer.Repeat != 0)
			{
				if(timer.Repeat > 0)
				{
					timer.Repeat--;
				}
				if(timer.Repeat != 0)
				{
					m_SlotListWaitForRemove.Add(timer.Id);
					m_SlotListWaitForAdd.Add(timer);
					//RemoveTimer(timer.Id);
					//AddTimerInternal(timer.Delay, timer.Interval, timer.Repeat,timer.Callback, timer.Param1, timer.Param2, timer.Id);
				}
			}
			else
			{
				m_SlotListWaitForRemove.Add(timer.Id);
				//RemoveTimer(timer.Id);
			}
		}

		foreach (var id in m_SlotListWaitForRemove)
		{
			RemoveTimer(id);
		}

		foreach (var timer in m_SlotListWaitForAdd)
		{
			AddTimerInternal(timer.Delay, timer.Interval, timer.Repeat,timer.Callback, timer.Param1, timer.Param2, timer.Id);
		}
	}

	public int AddTimer(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2)
	{
		return AddTimerInternal(delay, interval, repeat, callback, param1, param2, m_TimerIdCounter++);
	}

	private int AddTimerInternal(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2, int id)
	{
		var totalTicks = delay / m_TickMs;
		var rounds = (int) (totalTicks / m_SlotCount);
		var slotIndex = (m_CurrentSlot + (int)(totalTicks % m_SlotCount)) % m_SlotCount;

		var timer = new Timer
		{
			Id = id,
			Delay = delay,
			Interval = interval,
			Repeat = repeat,
			Callback = callback,
			Param1 = param1,
			Param2 = param2,
			RemainingRounds = rounds,
			SlotIndex = slotIndex
		};
		m_SlotsArray[slotIndex].Add(timer);
		m_TimerMap[id] = timer;
		//Debug.Log("AddTimerInternal Id : {id}, total ticks : {TimerManager.debugTotalTickTimes}");
		
		return id;
	}

	public bool RemoveTimer(int id)
	{
		Timer timer;
		if(!m_TimerMap.TryGetValue(id, out timer))
		{
			return false;
		}
		m_SlotsArray[timer.SlotIndex].Remove(timer);
		return m_TimerMap.Remove(id);
	}

	public bool ModifyTimer(int id, long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2)
	{
		if(!RemoveTimer(id))
		{
			return false;
		}
		AddTimer(delay, interval, repeat, callback, param1, param2);
		return true;
	}
}
