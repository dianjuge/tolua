--创建计时管理器
local m_GameObject = UnityEngine.GameObject
local m_go = m_GameObject('TimerManager')
local m_TimerMgr = m_go:AddComponent(typeof(TimerManager))
local m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

--计时器移除测试
startTime2 = os.clock()
timer2 = m_TimerMgr:AddTimer(
2000,
500,
2,
function(p1,p2)
	m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

	m_TimerMgr:RemoveTimer(timer2)
	print('timer2 removed!')

	print('timer2 execute triggered!', os.clock() - startTime2, m_DebugTickCount)
end,
2,22
)
print('timer2 added! tickMs: ',m_DebugTickCount)