--创建计时管理器
local m_GameObject = UnityEngine.GameObject
local m_go = m_GameObject('TimerManager')
local m_TimerMgr = m_go:AddComponent(typeof(TimerManager))
local m_DebugTickCount = m_TimerMgr.debugTotalTickTimes

--100万次计时器添加测试
m_StartTime100w = os.clock()

for i=0,1000000 do
	m_TimerMgr:AddTimer(
	3000,
	500,
	0,
	function(p1,p2)
	end,
	4,44
	)
end

print('100w timer Added! time:', os.clock()-m_StartTime100w, m_DebugTickCount)