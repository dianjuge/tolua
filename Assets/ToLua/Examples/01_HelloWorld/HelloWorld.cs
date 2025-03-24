using UnityEngine;
using LuaInterface;
using System;

public class HelloWorld : MonoBehaviour
{
    public static LuaState lua; 
    void Awake()
    {
        lua = new LuaState();
        lua.Start();

        LuaBinder.Bind(lua);
        DelegateFactory.Init();
        
        string hello =
            @"                
                print('hello tolua#')     
                
                timerMgr = TimerManager()

                debugTickCount = 0

                timerMgr:Start()
                
                
                --计时器执行计数
                timerExecuteCounter = 0

                --10万次计时器执行测试
                local m_StartTime = os.clock()
                for i=0,100000 do
                timerMgr:AddTimer(
                3000,
                500,
                0,
                function(p1,p2)
                    timerExecuteCounter = timerExecuteCounter + 1
                    if(timerExecuteCounter == 1) then
                        m_StartTime10w = os.clock()
                        print('10w timer execute triggered! start time:',m_StartTime10w)
                    end
                    if(i == 100000) then
                        m_EndTime10w = os.clock()
                        print('10w timer execute triggered! time:',m_EndTime10w-m_StartTime10w)
                    end
                    --print('timer4 triggered!',p1,p2)
                end,
                4,44
                )
                end

                local m_EndTime = os.clock()

                print('10w timer Added! time:',m_EndTime-m_StartTime,debugTickCount)

                --100万次计时器添加测试
                local m_StartTime = os.clock()
                for i=0,100000 do
                timerMgr:AddTimer(
                3000,
                500,
                0,
                function(p1,p2)
                end,
                4,44
                )
                end

                local m_EndTime = os.clock()

                print('100w timer Added! time:',m_EndTime-m_StartTime,debugTickCount)
            ";
        
        lua.DoString(hello, "HelloWorld.cs");
        lua.CheckTop();
        //lua.Dispose();
        //lua = null;
    }

    public void FixedUpdate()
    {
        string tick =
            @"   
                debugTickCount = debugTickCount + 1         
                --print('Tick#',debugTickCount)    
                timerMgr:Tick();     
            ";
        
        lua.DoString(tick, "HelloWorld.cs");
    }

    public void OnDestroy()
    {
        lua.Dispose();
        lua = null;
    }
}
