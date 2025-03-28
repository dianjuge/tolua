--创建计时管理器
local m_GameObject = UnityEngine.GameObject
local m_go = m_GameObject('TimerManager')
local m_TimerMgr = m_go:AddComponent(typeof(TimerManager))
local m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

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