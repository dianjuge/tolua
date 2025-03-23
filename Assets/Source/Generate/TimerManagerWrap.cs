﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class TimerManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(TimerManager), typeof(System.Object));
		L.RegFunction("Start", Start);
		L.RegFunction("AddTimer", AddTimer);
		L.RegFunction("RemoveTimer", RemoveTimer);
		L.RegFunction("ModifyTimer", ModifyTimer);
		L.RegFunction("Tick", Tick);
		L.RegFunction("OnDestroy", OnDestroy);
		L.RegFunction("New", _CreateTimerManager);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("debugTotalTickTimes", get_debugTotalTickTimes, set_debugTotalTickTimes);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateTimerManager(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				TimerManager obj = new TimerManager();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: TimerManager.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Start(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			TimerManager obj = (TimerManager)ToLua.CheckObject<TimerManager>(L, 1);
			obj.Start();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddTimer(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 7);
			TimerManager obj = (TimerManager)ToLua.CheckObject<TimerManager>(L, 1);
			long arg0 = LuaDLL.tolua_checkint64(L, 2);
			long arg1 = LuaDLL.tolua_checkint64(L, 3);
			int arg2 = (int)LuaDLL.luaL_checknumber(L, 4);
			System.Action<object,object> arg3 = (System.Action<object,object>)ToLua.CheckDelegate<System.Action<object,object>>(L, 5);
			object arg4 = ToLua.ToVarObject(L, 6);
			object arg5 = ToLua.ToVarObject(L, 7);
			int o = obj.AddTimer(arg0, arg1, arg2, arg3, arg4, arg5);
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveTimer(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			TimerManager obj = (TimerManager)ToLua.CheckObject<TimerManager>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			bool o = obj.RemoveTimer(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ModifyTimer(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 8);
			TimerManager obj = (TimerManager)ToLua.CheckObject<TimerManager>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			long arg1 = LuaDLL.tolua_checkint64(L, 3);
			long arg2 = LuaDLL.tolua_checkint64(L, 4);
			int arg3 = (int)LuaDLL.luaL_checknumber(L, 5);
			System.Action<object,object> arg4 = (System.Action<object,object>)ToLua.CheckDelegate<System.Action<object,object>>(L, 6);
			object arg5 = ToLua.ToVarObject(L, 7);
			object arg6 = ToLua.ToVarObject(L, 8);
			bool o = obj.ModifyTimer(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Tick(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			TimerManager obj = (TimerManager)ToLua.CheckObject<TimerManager>(L, 1);
			obj.Tick();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDestroy(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			TimerManager obj = (TimerManager)ToLua.CheckObject<TimerManager>(L, 1);
			obj.OnDestroy();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_debugTotalTickTimes(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			TimerManager obj = (TimerManager)o;
			int ret = obj.debugTotalTickTimes;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index debugTotalTickTimes on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_debugTotalTickTimes(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			TimerManager obj = (TimerManager)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.debugTotalTickTimes = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index debugTotalTickTimes on a nil value");
		}
	}
}

