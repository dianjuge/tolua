print("This is a script from a utf8 file")
print("tolua: 你好! こんにちは! 안녕하세요!")

local timer1 = TimerManager.AddTimer(1000,1000, 0, function(a,b) print("TestTimer, params :", a, b) end, 1, 11);