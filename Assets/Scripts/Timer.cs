using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 计时器类
public class Timer
{
	// 计时器ID
	public int Id { get; set; }
	// 计时器延迟时间
	public long Delay { get; set; }
	// 计时器重复次数
	public int Repeat { get; set; }
	// 计时器间隔时间
	public long Interval { get; set; }
	// 计时器参数1
	public object Param1 { get; set; }
	// 计时器参数2
	public object Param2 { get; set; }
	// 计时器是否已被释放
	public bool IsDisposed { get; set; }
	// 计时器槽位索引
    public int SlotIndex { get; internal set; }
	// 计时器时间轮索引
	public int WheelIndex { get; internal set; }
	// 计时器槽位链表节点
	public LinkedListNode<Timer> ListNode { get; set; }
	// 计时器回调函数
	public Action<object, object> Callback { get; set; }
}
