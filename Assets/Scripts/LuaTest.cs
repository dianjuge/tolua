using System;
using UnityEngine;
using LuaInterface;

// 计时器类型枚举
public enum TIMER_TEST_TYPE
{
    // 简单计时器
    SIMPLETIMER,
    // 时间轮计时器添加
    TIMERWHEEL_ADD,
    // 时间轮计时器删除
    TIMERWHEEL_REMOVE,
    // 时间轮计时器修改
    TIMERWHEEL_MODIFY,
    // 时间轮计时器执行10万次
    TIMERWHEEL_EXECUTE_100K,
    // 时间轮计时器添加100万次
    TIMERWHEEL_ADD_1000K,
}

// LuaTest类用于测试Lua脚本的执行
public class LuaTest : MonoBehaviour
{
    // Lua虚拟机实例
    public static LuaState s_LuaState; 
    // 计时器枚举类型
    public TIMER_TEST_TYPE timerType = TIMER_TEST_TYPE.SIMPLETIMER;
    
    public void Start()
    {
        //启动lua虚拟机
        s_LuaState = new LuaState();
        s_LuaState.Start();

        //绑定lua函数
        LuaBinder.Bind(s_LuaState);
        
        //初始化lua委托工厂
        DelegateFactory.Init();
        
        //读取lua脚本路径
        s_LuaState.AddSearchPath(Application.dataPath + "/Lua");  
        
        // 加载lua脚本
        DoLuaFile(timerType);
    }

    public void DoLuaFile(TIMER_TEST_TYPE type)
    {
        // 获取lua脚本路径
        var fileName = type.ToString() + ".lua";
        //执行lua脚本
        s_LuaState.DoFile(fileName);
    }

    public void OnDestroy()
    {
        //释放lua虚拟机
        s_LuaState.CheckTop();
        s_LuaState.Dispose();
        s_LuaState = null;
    }
}
