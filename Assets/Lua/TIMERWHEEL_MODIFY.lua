--创建计时管理器
local m_GameObject = UnityEngine.GameObject
local m_go = m_GameObject('TimerManager')
local m_TimerMgr = m_go:AddComponent(typeof(TimerManager))
local m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

--计时器修改测试
startTime3 = os.clock()
timer3 = m_TimerMgr:AddTimer(
2000,
500,
2,
function(p1,p2)
	m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

	m_TimerMgr:ModifyTimer(
	timer3,
	3000,
	500,
	3,
	function(p1,p2)
		m_DebugTickCount = m_TimerMgr.debugTotalTickTimes
		print('modified timer3 execute triggered! tickMs:  p1, p2', p1, p2 , os.clock() - startTime3, m_DebugTickCount)
	end,
	30,330
	)

	print('timer3 modified!')
	print('timer3 execute triggered! tickMs:  p1, p2', p1, p2, os.clock() - startTime3, m_DebugTickCount)
end,
3,33
)

print('timer3 added! tickMs: ',m_DebugTickCount)