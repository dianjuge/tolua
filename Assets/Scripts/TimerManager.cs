using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerManager
{
	private System.Timers.Timer m_Timer;
	private TimerWheel m_TimerWheel;

	public int debugTotalTickTimes = 0;
    
	public void Start()
    {
        m_TimerWheel = new TimerWheel(20, 10);

		// m_Timer = new System.Timers.Timer(1);
		// m_Timer.Elapsed += (sender, e) => Tick();
		// m_Timer.Start();
		
		// var timer1 = AddTimer(1000,1000, 0, (a,b) => Debug.Log("TestTimer, param1 : {a}, param2 : {b}, total ticks : {0}"), 1, 11);
		// var timer2 = AddTimer(3000,1000, 0, (a,b) => Debug.Log("TestTimer2, param1 : {a}, param2 : {b}, total ticks : {TimerManager.debugTotalTickTimes}"), 3, 33);
		
		// var timer3 = AddTimer(2100,1000, 0, (a,b) => {
		// 	RemoveTimer(timer2);
		// 	Debug.Log("TestTimer3, param1 : {a}, param2 : {b}, total ticks : {TimerManager.debugTotalTickTimes}");
		// }, 2, 22);
		// var timer4 = AddTimer(5000,1000, 0, (a,b) => Debug.Log("TestTimer2, param1 : {a}, param2 : {b}, total ticks : {TimerManager.debugTotalTickTimes}"), 3, 33);
		
		
    }

    public int AddTimer(long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2)
	{
		return m_TimerWheel.AddTimer(delay, interval, repeat, callback, param1, param2);
	}

	public bool RemoveTimer(int id)
	{
		return m_TimerWheel.RemoveTimer(id);
	}

	public bool ModifyTimer(int id, long delay, long interval, int repeat, Action<object, object> callback, object param1, object param2)
	{
		return m_TimerWheel.ModifyTimer(id, delay, interval, repeat, callback, param1, param2);
	}

	public void Tick()
    {
		debugTotalTickTimes+=1;
		m_TimerWheel.Tick();
    }

	public void OnDestroy()
	{
		m_Timer.Stop();
		m_Timer.Dispose();
	}
}
