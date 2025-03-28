--创建计时管理器
local m_GameObject = UnityEngine.GameObject
local m_go = m_GameObject('TimerManager')
local m_TimerMgr = m_go:AddComponent(typeof(TimerManager))
local m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

--计时器添加测试

local m_Timer1 = m_TimerMgr:AddTimer(
	3000,
	500,
	2,
	function(p1,p2)
		m_DebugTickCount = m_TimerMgr.debugTotalTickTimes
		print('timer1 execute triggered! p1, p2', p1, p2, Time.time, m_DebugTickCount)
	end,
	1,11
	)
	
print('timer1 added! tickMs',m_DebugTickCount)
startTime1 = Time.time