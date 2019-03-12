--- Description: 纯lua层的消息管理器
--- Author:Trubs (WQ)
--- Date: 2018/06/26
--

LuaMsgMgr = LuaMsgMgr or 
{
	funcs = {},
	funcKeys = {},
	aheadArgs = {}
}

local function GetMixTLength(T)
	local length = 0
	for k,v in pairs(T)do
		length = length + 1
	end
	return length
end

--添加消息监听 aheadArg一般为self 可不传
function LuaMsgMgr:AddListener( str, func, aheadArg )
	local funcKeyT = self.funcKeys[str]
	if funcKeyT then
		if funcKeyT[func] then
			LuaFramework.utils.LogWarning("重复添加消息监听:"..str..debug.traceback())
			return 
		end
		funcKeyT[func] = GetMixTLength(funcKeyT) +1
	else
		self.funcKeys[str] = {[func] = 1}
	end

	local funcT = self.funcs[str]
	if funcT then
		funcT[#funcT +1] = func
	else
		self.funcs[str] = {func}
	end

	if aheadArg then
		local aheadT = self.aheadArgs[str]
		if aheadT then
			aheadT[func] = aheadArg
		else
			self.aheadArgs[str] = {[func] = aheadArg}
		end
	end
end

local function RemoveKey(funcKeyT,key)
	local theValue = funcKeyT[key]
	for k,v in pairs(funcKeyT)do
		if v > theValue then
			funcKeyT[k] = v - 1
		end
	end
	funcKeyT[key] = nil
end

function LuaMsgMgr:RemoveListener(str,func)

	local funcT = self.funcs[str]
	local funcKeyT = self.funcKeys[str]
	if not funcT or not funcKeyT or not funcKeyT[func] then return end

	table.remove(funcT,funcKeyT[func])
	--funcT[func] = nil
	--funcKeyT[func] = nil
	RemoveKey(funcKeyT,func)
	local aheadT = self.aheadArgs[str]
	if aheadT and aheadT[func] then
		aheadT[func] = nil
	end
end

function LuaMsgMgr:Notify(str,T,...)
	local funcT = self.funcs[str]
	--local args = {...}
	if funcT then
		for _,func in pairs(funcT)do
			local aheadT = self.aheadArgs[str]
			--local unpack = unpack or table.unpack--兼容不同的lua版本
			if aheadT and aheadT[func] then
			    func(aheadT[func],T,str, ... )
			else
			    func(T,str,...)
			end
		end
	end
end

return LuaMsgMgr
