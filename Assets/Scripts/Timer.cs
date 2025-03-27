using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Timer
{
	public int Id { get; set; }
	public long Delay { get; set; }
	public long Interval { get; set; }
	public int Repeat { get; set; }
	public object Param1 { get; set; }
	public object Param2 { get; set; }
	public Action<object, object> Callback { get; set; }
	
    public int SlotIndex { get; internal set; }
	public int WheelIndex { get; internal set; }
	public LinkedListNode<Timer> ListNode { get; set; }
}
