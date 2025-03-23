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
                
                timerMgr:Start()

                local timer1 = timerMgr:AddTimer(
                1000,
                500,
                0,
                function(p1,p2)
                    print('timer1 triggered!',p1,p2)
                end,
                1,
                11
                )

                timerMgr:ModifyTimer(
                timer1,
                5000,
                500,
                2,
                function(p1,p2)
                    print('timer1 triggered!',p1,p2)
                end,
                1m,
                11m
                )

                local timer2 = timerMgr:AddTimer(
                2000,
                500,
                0,
                function(p1,p2)
                    print('timer2 triggered!',p1,p2)
                end,
                2,
                22
                )

                local timer3 = timerMgr:AddTimer(
                3000,
                500,
                0,
                function(p1,p2)
                    print('timer3 triggered!',p1,p2)
                end,
                3,
                33
                )

                timerMgr:RemoveTimer(timer3)

                for i=0,100000,1
                local timer4 = timerMgr:AddTimer(
                3000,
                500,
                0,
                function(p1,p2)
                    print('timer4 triggered!',p1,p2)
                end,
                4,
                44
                )
                end
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
                --print('Tick#')              
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
