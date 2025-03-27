using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerWheel 
{
	public TimerWheel preWheel;
	public TimerWheel nextWheel;
	public int CurrentSlot { get; private set; }
	public static Dictionary<int, Timer> s_TimerMap = new Dictionary<int, Timer>();

	private readonly int m_TickMs = 0;
	private readonly int m_SlotCount = 0;
	private readonly int m_WheelIndex = 0;
	private readonly LinkedList<Timer>[] m_SlotsArray;
	
	public TimerWheel(int tickMs, int slotCount, int wheelIndex)
	{
		m_TickMs = tickMs;
		m_SlotCount = slotCount;
		m_WheelIndex = wheelIndex;

		m_SlotsArray = new LinkedList<Timer>[slotCount];
		for (int i = 0; i < slotCount; i++)
		{
			m_SlotsArray[i] = new LinkedList<Timer>();
		}
	}

	public void Tick()
	{
		CurrentSlot = (CurrentSlot + 1) % m_SlotCount;

		//Debug.Log("Tick wheel: " + m_WheelIndex + ", slot: " + CurrentSlot + ", count: " + m_SlotsArray[CurrentSlot].Count);
		var node = m_SlotsArray[CurrentSlot].First;
		while (node != null)
		{
			var next = node.Next;
			var timer = node.Value;

			if(IsNeedAddToPreWheel(timer.Delay))
			{
				RemoveTimer(timer.Id);
				AddTimerToPreWheel(timer);
				node = next;
				continue;
			}
			//Debug.Log("Tick wheel: " + m_WheelIndex + ", slot: " + CurrentSlot + ", count: " + m_SlotsArray[CurrentSlot].Count + ", id: " + timer.Id + ", delay: " + timer.Delay + ", interval: " + timer.Interval + ", repeat: " + timer.Repeat);
			
			if(timer.Repeat > 0)
			{
				timer.Repeat--;
				RemoveTimer(timer.Id);
				AddTimer(timer.Interval, timer.Interval, timer.Repeat,timer.Callback, timer.Param1, timer.Param2, timer.Id);
			}
			else
			{
				RemoveTimer(timer.Id);
			}

			timer.Callback.Invoke(timer.Param1, timer.Param2);
			node = next;
		}
	}

	public void AddTimer(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2,int id)
	{
		if(TryAddTimerToNextWheel(delay, interval, repeat, callback, param1, param2, id))
		{
			return;
		}

		if(TryAddTimerToPreWheel(delay, interval, repeat, callback, param1, param2, id))
		{
			return;
		}

		AddTimerToCurrentWheel(delay, interval, repeat, callback, param1, param2, id);
		return;
	}

	private bool IsNeedAddToNextWheel(long delay)
	{
		return delay > (m_TickMs * m_SlotCount);
	}

	private bool IsNeedAddToPreWheel(long delay)
	{
		return (delay > 0) 
			&& (delay < m_TickMs) 
			&& (m_WheelIndex > 0);
	}

	private bool TryAddTimerToNextWheel(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2, int id)
	{
		if(IsNeedAddToNextWheel(delay))
		{
			//Debug.Log("TryAddTimerToNextWheel delay: " + delay + ", id: " + id + ", wheelIndex: " + m_WheelIndex + ", slot: " + CurrentSlot + ", count: " + m_SlotsArray[CurrentSlot].Count);
			nextWheel.AddTimer(delay, interval, repeat, callback, param1, param2, id);
			return true;
		}
		return false;
	}

	private bool TryAddTimerToPreWheel(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2, int id)
	{
		if(IsNeedAddToPreWheel(delay))
		{
			//Debug.Log("AddTimerToPreWheel delay: " + delay + ", id: " + id + ", wheelIndex: " + m_WheelIndex + ", slot: " + CurrentSlot + ", count: " + m_SlotsArray[CurrentSlot].Count);	
			preWheel.AddTimer(delay, interval, repeat, callback, param1, param2, id);
			return true;
		}
		return false;
	}

	private void AddTimerToPreWheel(Timer timer)
	{
		//Debug.Log("AddTimerToPreWheel delay: " + timer.Delay + ", id: " + timer.Id + ", wheelIndex: " + m_WheelIndex + ", slot: " + CurrentSlot + ", count: " + m_SlotsArray[CurrentSlot].Count);	
		preWheel.AddTimer(timer.Delay, timer.Interval, timer.Repeat, timer.Callback, timer.Param1, timer.Param2, timer.Id);
	}

	private int AddTimerToCurrentWheel(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2, int id)
	{
		var delaySlot = (int)(delay / m_TickMs);
		var slotIndex = (CurrentSlot + delaySlot) % m_SlotCount;

		var timer = new Timer
		{
			Id = id,
			Delay = delay % m_TickMs,
			Interval = interval,
			Repeat = repeat,
			Callback = callback,
			Param1 = param1,
			Param2 = param2,
			WheelIndex = m_WheelIndex,
			SlotIndex = slotIndex,
			ListNode = m_SlotsArray[slotIndex].AddLast(new Timer()),
		};
		timer.ListNode.Value = timer;
		s_TimerMap[id] = timer;
		//Debug.Log("AddTimerInternal Id : {id}, total ticks : {TimerManager.debugTotalTickTimes}");
		//Debug.Log("AddTimer: " + m_WheelIndex + ", slot: " + CurrentSlot + ", count: " + m_SlotsArray[CurrentSlot].Count + ", id: " + timer.Id + ", delay: " + timer.Delay + ", interval: " + timer.Interval + ", repeat: " + timer.Repeat);
			
		return id;
	}

	public bool RemoveTimer(int id)
	{
		Timer timer;
		if(!s_TimerMap.TryGetValue(id, out timer))
		{
			return false;
		}
		m_SlotsArray[timer.SlotIndex].Remove(timer.ListNode);
		return s_TimerMap.Remove(id);
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
