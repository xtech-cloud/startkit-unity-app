using System.Collections.Generic;
using System;
using XLua;
using System.Reflection;
using System.Linq;

public static class APIConfig
{
    [LuaCallCSharp]
    public static List<Type> mymodule_lua_call_cs_list = new List<Type>(){
        //typeof(XXXAPI),
    };
}
