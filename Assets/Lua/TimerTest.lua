--创建计时管理器
local m_GameObject = UnityEngine.GameObject
local m_go = m_GameObject('TimerManager')
local m_TimerMgr = m_go:AddComponent(typeof(TimerManager))
local m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

-- --计时器添加测试
-- startTime1 = os.clock()
-- local m_Timer1 = m_TimerMgr:AddTimer(
-- 	2000,
-- 	500,
-- 	1,
-- 	function(p1,p2)
-- 		m_DebugTickCount = m_TimerMgr.debugTotalTickTimes
-- 		print('timer1 execute triggered! p1, p2', p1, p2, os.clock() - startTime1, m_DebugTickCount)
-- 	end,
-- 	1,11
-- 	)


-- print('timer1 added! tickMs',m_DebugTickCount)

-- --计时器移除测试
-- startTime2 = os.clock()
-- timer2 = m_TimerMgr:AddTimer(
-- 2000,
-- 500,
-- 2,
-- function(p1,p2)
-- 	m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

-- 	m_TimerMgr:RemoveTimer(timer2)
-- 	print('timer2 removed!')

-- 	print('timer2 execute triggered!', os.clock() - startTime2, m_DebugTickCount)
-- end,
-- 2,22
-- )
-- print('timer2 added! tickMs: ',m_DebugTickCount)

-- --计时器修改测试
-- startTime3 = os.clock()
-- timer3 = m_TimerMgr:AddTimer(
-- 2000,
-- 500,
-- 2,
-- function(p1,p2)
-- 	m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

-- 	m_TimerMgr:ModifyTimer(
-- 	timer3,
-- 	3000,
-- 	500,
-- 	3,
-- 	function(p1,p2)
-- 		m_DebugTickCount = m_TimerMgr.debugTotalTickTimes
-- 		print('modified timer3 execute triggered! tickMs:  p1, p2', p1, p2 , os.clock() - startTime3, m_DebugTickCount)
-- 	end,
-- 	30,330
-- 	)
-- 	print('timer3 modified!')
	
-- 	print('timer3 execute triggered! tickMs:  p1, p2', p1, p2, os.clock() - startTime3, m_DebugTickCount)
-- end,
-- 3,33
-- )

-- print('timer3 added! tickMs: ',m_DebugTickCount)



--10万次计时器执行测试

--计时器执行计数
timerExecuteCounter = 0
m_StartTime10w = os.clock()

for i=1,100000 do
m_TimerMgr:AddTimer(
100 * (1 + i % 30),
0,
0,
function(p1,p2)
	timerExecuteCounter = timerExecuteCounter + 1
	if(timerExecuteCounter == 1) then
		--m_StartTime10w = os.clock()
		print('10w timer execute trigger start! time:',os.clock() - m_StartTime10w)
	end
	if(timerExecuteCounter == 100000) then
		print('10w timer execute trigger end! time:',os.clock() - m_StartTime10w)
	end
	
end,
4,44
)
end

print('10w timer Added! time:', os.clock() - m_StartTime10w, m_DebugTickCount)
m_StartTime10w = os.clock()

-- --100万次计时器添加测试
-- local m_StartTime100w = os.clock()

-- for i=0,1000000 do
-- 	m_TimerMgr:AddTimer(
-- 	3000,
-- 	500,
-- 	0,
-- 	function(p1,p2)
-- 	end,
-- 	4,44
-- 	)
-- end

-- print('100w timer Added! time:', os.clock()-m_StartTime100w, m_DebugTickCount)