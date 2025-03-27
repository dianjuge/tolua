--创建计时管理器
local m_GameObject = UnityEngine.GameObject
local m_go = m_GameObject('TimerManager')
local m_TimerMgr = m_go:AddComponent(typeof(SimpleTimer))
local m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

--10万次计时器执行测试

--计时器执行计数
timerExecuteCounter = 0

local m_StartTime = os.clock()
for i=0,100000 do
m_TimerMgr:AddTimer(
100 * (1 + i % 30),
500,
0,
function(p1,p2)
	timerExecuteCounter = timerExecuteCounter + 1
	if(timerExecuteCounter == 1) then
		m_StartTime10w = os.clock()
		print('10w timer execute triggered! start time:',m_StartTime10w)
	end
	if(timerExecuteCounter == 100000) then
		m_EndTime10w = os.clock()
		print('10w timer execute triggered! time:',m_EndTime10w-m_StartTime10w)
	end
	--print('timer4 triggered!',p1,p2)
end,
4,44,i
)
end

local m_EndTime = os.clock()

print('10w timer Added! time:',m_EndTime-m_StartTime,m_DebugTickCount)

-- --100万次计时器添加测试
-- local m_StartTime = os.clock()
-- for i=0,1000000 do
-- m_TimerMgr:AddTimer(
-- 3000,
-- 500,
-- 0,
-- function(p1,p2)
-- end,
-- 4,44
-- )
-- end

-- local m_EndTime = os.clock()

-- print('100w timer Added! time:',m_EndTime-m_StartTime,m_DebugTickCount)