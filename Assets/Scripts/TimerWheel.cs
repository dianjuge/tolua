using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 多层时间轮
public class TimerWheel 
{
	// 计时器映射表
	public static Dictionary<int, Timer> s_TimerMap = new Dictionary<int, Timer>();
	// 下层时间轮
	public TimerWheel preWheel;
	// 上层时间轮
	public TimerWheel nextWheel;
	// 当前时间轮槽位
	public int CurrentSlot { get; private set; }

	// 计时器ID自增计数器
    private static int m_TimerIdCounter = 1;
	// 单槽时间精度
	private readonly int m_TickMs = 0;
	// 槽位数量
	private readonly int m_SlotCount = 0;
	// 当前时间轮索引
	private readonly int m_WheelIndex = 0;
	// 当前时间轮槽位计时器链表
	private readonly LinkedList<Timer>[] m_SlotsArray;
	
	// 时间轮构造函数
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

	// 更新当前时间轮
	public void Tick()
	{
		// 推进到下一槽位
		CurrentSlot = (CurrentSlot + 1) % m_SlotCount;

		// 轮询当前槽位的计时器链表
		var node = m_SlotsArray[CurrentSlot].First;
		while (node != null)
		{
			var next = node.Next;
			var timer = node.Value;

			// 判断是否需要添加到下层时间轮
			if(IsNeedAddToPreWheel(timer.Delay))
			{
				RemoveTimer(timer.Id);
				AddTimerToPreWheel(timer);
				node = next;
				continue;
			}

			// 尝试执行回调函数
			TryDoCallBack(timer);

			// 判断计时器是否需要重复执行
			if(!TryDoTimerRepetition(timer))
			{
				RemoveTimer(timer.Id);
			}

			node = next;
		}
	}

	// 尝试执行回调函数
	private void TryDoCallBack(Timer timer)
	{
		// 判断是否已经被释放
		if(timer.IsDisposed)
		{
			return;
		}

		// 执行回调函数
		if(timer.Callback != null)
		{
			timer.Callback.Invoke(timer.Param1, timer.Param2);
		}
	}

	// 尝试执行计时器重复
	private bool TryDoTimerRepetition(Timer timer)
	{
		// 判断是否已经被释放
		if(timer.IsDisposed)
		{
			return false;
		}
		
		// 检测剩余重复次数
		if(timer.Repeat <= 0)
		{
			return false;
		}
		
		timer.Repeat--;
		RemoveTimer(timer.Id);
		AddTimer(timer.Interval, timer.Interval, timer.Repeat,timer.Callback, timer.Param1, timer.Param2, timer.Id);
		
		return true;
	}

	// 添加计时器
	public int AddTimer(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2)
	{
		var newId = m_TimerIdCounter++;
		AddTimer(delay,interval,repeat,callback,param1, param2, newId);
		return newId;
	}

	// 添加计时器
	private void AddTimer(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2,int id)
	{
		// 检测参数合法性
		if((delay < 0) || (delay > 3600000 * 24) || (interval < 0) || (interval > 3600000 * 24) || (repeat < 0))
		{
			Debug.LogError("TimerWheel.AddTimer: invalid parameter, delay = " + delay + ", interval = " + interval + ", repeat = " + repeat);
			return;
		}
		
		// 尝试添加计时器到上层时间轮
		if(TryAddTimerToNextWheel(delay, interval, repeat, callback, param1, param2, id))
		{
			return;
		}

		// 尝试添加计时器到下层时间轮
		if(TryAddTimerToPreWheel(delay, interval, repeat, callback, param1, param2, id))
		{
			return;
		}

		// 添加计时器到当前时间轮
		AddTimerToCurrentWheel(delay, interval, repeat, callback, param1, param2, id);
		return;
	}

	// 判断是否需要添加到上层时间轮
	private bool IsNeedAddToNextWheel(long delay)
	{
		// 判断是否超过当前时间轮的最大延迟
		return delay > (m_TickMs * m_SlotCount);
	}

	// 判断是否需要添加到下层时间轮
	private bool IsNeedAddToPreWheel(long delay)
	{
		return (delay > 0) 				
			&& (delay < m_TickMs) 
			&& (m_WheelIndex > 0);
	}

	// 添加计时器到上层时间轮
	private bool TryAddTimerToNextWheel(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2, int id)
	{
		if(IsNeedAddToNextWheel(delay))
		{
			nextWheel.AddTimer(delay, interval, repeat, callback, param1, param2, id);
			return true;
		}
		return false;
	}

	// 尝试添加计时器到下层时间轮
	private bool TryAddTimerToPreWheel(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2, int id)
	{
		if(IsNeedAddToPreWheel(delay))
		{
			preWheel.AddTimer(delay, interval, repeat, callback, param1, param2, id);
			return true;
		}
		return false;
	}

	// 添加计时器到下层时间轮
	private void AddTimerToPreWheel(Timer timer)
	{
		preWheel.AddTimer(timer.Delay, timer.Interval, timer.Repeat, timer.Callback, timer.Param1, timer.Param2, timer.Id);
	}

	// 添加计时器到当前时间轮
	private int AddTimerToCurrentWheel(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2, int id)
	{
		// 计算延迟槽位
		var delaySlot = (int)(delay / m_TickMs);
		// 计算目标槽位
		var slotIndex = (CurrentSlot + delaySlot) % m_SlotCount;
		// 创建计时器对象
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
			IsDisposed = false
		};

		// 将计时器添加到当前时间轮槽位链表值
		timer.ListNode = m_SlotsArray[slotIndex].AddLast(timer);
		// 将计时器添加到计时器映射表
		s_TimerMap[id] = timer;
		//Debug.Log("AddTimerToCurrentWheel " + timer.Id + " " + timer.Repeat+ " " + timer.IsDisposed);
		
		return id;
	}

	// 移除计时器
	public bool RemoveTimer(int id)
	{
		// 检查计时器是否存在映射表中
		Timer timer;
		if(!TimerWheel.s_TimerMap.TryGetValue(id, out timer))
		{
			return false;
		}

		// 从槽位移除计时器节点
		m_SlotsArray[timer.SlotIndex].Remove(timer.ListNode);
		// 从计时器映射表中移除计时器
		s_TimerMap.Remove(id);
		// 释放计时器对象
		timer.IsDisposed = true;
		//Debug.Log("RemoveTimer " + id + " " + timer.Repeat);
		return true;
	}

	// 修改计时器
	public bool ModifyTimer(int id, long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2)
	{
		
		// 检查计时器是否存在映射表中
		Timer timer;
		if(!TimerWheel.s_TimerMap.TryGetValue(id, out timer))
		{
			return false;
		}
		
		// 修改计时器参数
		// 如果参数为-1，则不修改对应参数
		timer.Delay = delay == -1 ? timer.Delay : delay;
		timer.Interval = interval == -1 ? timer.Interval : interval;
		timer.Repeat = repeat == -1 ? timer.Repeat : repeat;
		timer.Callback = callback == null ? timer.Callback : callback;
		timer.Param1 = param1 == null ? timer.Param1 : param1;
		timer.Param2 = param2 == null ? timer.Param2 : param2;
		//timer.ListNode.Value = timer;
		
		// 移除旧计时器
		if(!RemoveTimer(id))
		{
			return false;
		}
		
		// 添加修改后的新计时器
		AddTimer(timer.Delay, timer.Interval, timer.Repeat, timer.Callback, timer.Param1, timer.Param2);
		
		return true;
	}
}
