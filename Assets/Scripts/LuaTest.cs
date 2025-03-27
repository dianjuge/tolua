using UnityEngine;
using LuaInterface;
using System;

public enum TIMER_TYPE
{
    SIMPETIMER,
    TIMERWHEEL,
}

public class LuaTest : MonoBehaviour
{
    public TIMER_TYPE timerType = TIMER_TYPE.SIMPETIMER;
    public static LuaState s_LuaState; 
    
    public void Start()
    {
        //启动lua虚拟机
        s_LuaState = new LuaState();
        s_LuaState.Start();

        //绑定lua函数
        LuaBinder.Bind(s_LuaState);
        
        //初始化lua委托工厂
        DelegateFactory.Init();
        
        //读取并执行lua脚本
        s_LuaState.AddSearchPath(Application.dataPath + "/Lua");  
        if(timerType == TIMER_TYPE.SIMPETIMER)
            s_LuaState.DoFile("SimpleTimerTest.lua");
        else if(timerType == TIMER_TYPE.TIMERWHEEL)
            s_LuaState.DoFile("TimerTest.lua");
        
        s_LuaState.CheckTop();
    }

    public void OnDestroy()
    {
        //释放lua虚拟机
        s_LuaState.Dispose();
        s_LuaState = null;
    }
}
