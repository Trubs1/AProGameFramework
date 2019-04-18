--- Description: 面板基类
--- Author:Trubs (WQ)
--- Date: 2019/03/22

BasePanel = {}

function BasePanel:New (o)
	o = o or {}
	print("where", debug.traceback() )
	setmetatable(o, self)
	self.__index = self
  return o
end

function BasePanel:Init()
	--Log("~~~~ OnInted",self)
end

function BasePanel:Show()
	self.isShow = true
	self.gameObject:SetActive(true)
end

function BasePanel:Hide()
	self.isShow = false
	self.gameObject:SetActive(false)
end



return BasePanel





--[[
对元表不是很清楚的同学请细读:
setmetatable(table, metatable): 对指定 table 设置元表(metatable) 如果 metatable 是 nil，将指定表的元表移除。 如果原来那张元表有 "__metatable" 域，抛出一个错误。这个函数返回 table。
（你不能在Lua中改变其它类型值(非表)的元表，那些只能在 C 里做。）

Lua查找一个表元素时的规则:
1.在表中查找，若找到，返回该元素，找不到则继续
2.判断该表是否有元表，若没有元表，返回 nil，有元表则继续。
3.判断元表有没有 __index 方法，若没有，则返回 nil；
若 __index是一个表，则重复 1、2、3；
若 __index 是一个函数,lua会将table和键会作为参数传递给函数并调用，最后返回该函数的返回值。
若还不明白请查看:https://blog.csdn.net/xocoder/article/details/9028347

注意、Lua 从元表中直接获取元方法； 访问元表中的元方法永远不会触发另一次元方法。 下面的代码模拟了 Lua 从一个对象 obj 中获取一个元方法的过程：
rawget(tb, k)==>rawget(getmetatable(obj) or {}, "__" .. event_name)
rawset(t, k, v)
]]