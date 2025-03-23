print("This is a script from a utf8 file")
print("tolua: 你好! こんにちは! 안녕하세요!")

local timerMgr = TimerManager()

local timerMgrStart = timerMgr:Start()

local function timerCallback(p1, p2)
    print('timer triggered!', p1, p2)
end

local timerId = timerMgr:AddTimer(
1000,
500,
0,
timerCallback,
"11",
"22"
)

       