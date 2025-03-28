using System;
using UnityEngine;
using System.Timers;
using System.Collections;
using System.Diagnostics;
using UnityEngine.Profiling;
using System.Collections.Generic;

// 多层时间轮管理器
public class TimerManager : MonoBehaviour
{	
	// 计时器管理器实例
	public static TimerManager s_Instance = null;
	// 调试用帧次数计数
	public int debugTotalTickTimes = 0;
	
	
	// 时间轮列表
	private List<TimerWheel> m_TimerWheels;
	// 当前时间
	private float m_CurrentTime;
	// 下次刷新时间
	private float m_NextTime;
	// 刷新间隔
	private const float k_TickInterval = 0.1f;
	
	// 初始化
	public void Awake()
    {
		// 初始化单例
		if (s_Instance != null)
		{
			UnityEngine.Debug.LogError("TimerManager already exists!");
			return;
		}
		s_Instance = this;
		
		// 初始化时间轮
		InitializeTimerWheels();
    }

	// 帧更新
	public void Update()
    {
		// 按照100毫秒每帧更新时间轮
		m_CurrentTime += Time.deltaTime;
		while(m_CurrentTime >= m_NextTime)
		{
			m_NextTime += k_TickInterval;
			debugTotalTickTimes += 1;
			// 更新最下层时间轮
			TickTimerWheel(0);
		}
    }

	// 更新指定层时间轮
	private void TickTimerWheel(int index)
	{
		// 如果索引越界，则返回
		if(index >= m_TimerWheels.Count)
		{
			return;
		}

		// 更新当前时间轮
		m_TimerWheels[index].Tick();

		// 如果当前时间轮的槽位为0，则更新上层时间轮
		if(m_TimerWheels[index].CurrentSlot == 0)
		{
			TickTimerWheel(index + 1);
		}
	}

	// 添加计时器，返回计时器ID
    public int AddTimer(long delay, long interval = 0, int repeat = 0, Action<object, object> callback = null, object param1 = null, object param2 = null)
	{
		return m_TimerWheels[0].AddTimer(delay, interval, repeat, callback, param1, param2);
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

		// 从计时器所在轮中移除计时器
		return m_TimerWheels[timer.WheelIndex].RemoveTimer(id);
	}

	// 修改计时器，-1表示不修改对应参数
	public bool ModifyTimer(int id, long delay = -1, long interval = -1, int repeat = -1, Action<object, object> callback = null, object param1 = null, object param2 = null)
	{
		// 检查计时器是否存在映射表中
		Timer timer;
		if(!TimerWheel.s_TimerMap.TryGetValue(id, out timer))
		{
			return false;
		}

		// 从计时器所在轮中修改计时器参数
		return m_TimerWheels[timer.WheelIndex].ModifyTimer(id, delay, interval, repeat, callback, param1, param2);
	}

	// 销毁计时器管理器
	public void OnDestroy()
	{
		s_Instance = null;
		m_TimerWheels.Clear();
		m_TimerWheels = null;
		debugTotalTickTimes = 0;
	}

	// 初始化多层时间轮
	private void InitializeTimerWheels()
	{
		// 新建时间轮列表
		m_TimerWheels = new List<TimerWheel>();
		
		// 添加时间轮
        m_TimerWheels.Add(new TimerWheel(100, 10, 0)); 			// 100毫秒
        m_TimerWheels.Add(new TimerWheel(1000, 60, 1)); 		// 1秒
		m_TimerWheels.Add(new TimerWheel(60000, 60, 2)); 		// 1分钟
		m_TimerWheels.Add(new TimerWheel(3600000, 24, 3)); 		// 1小时

		// 链接上层时间轮
		for (int i = 0; i < m_TimerWheels.Count - 1; i++)
		{
			m_TimerWheels[i].nextWheel = m_TimerWheels[i + 1];
		}

		// 链接下层时间轮
		for (int i = 1; i < m_TimerWheels.Count; i++)
		{
			m_TimerWheels[i].preWheel = m_TimerWheels[i - 1];
		}
	}
}
