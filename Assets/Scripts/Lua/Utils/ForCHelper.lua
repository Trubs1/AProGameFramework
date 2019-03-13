--C#访问Lua的助理


ForCHelper = {}

local function TestFunc(T,msg)
	Log("<color=yellow>ForCHelper_TestFunc</color>",T,msg)
	print("T.CurHp",T.curHp,T.id)
end

function ForCHelper.Init()
    ForLuaHelper.AddLuaFuncForC(ForCHelper.SendMsgToLua,"SendMsgToLua");

	--LuaMsgMgr:AddListener("UpdateLifeData",TestFunc)
end

function ForCHelper.SendMsgToLua(msg,T)
	--print("~~~##ForCHelper.SendMsgToLua",msg,T)
	LuaMsgMgr:Notify(msg,T)
end

ForCHelper.Init()

return ForCHelper