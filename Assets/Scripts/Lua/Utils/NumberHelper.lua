---
--- 数字帮助类
--- @version: 1.0.0
--- @author cuishaojie
--- @date: 2017/10/23
--- @remarks Copyright (c) 2017, Magicell.
--- All rights reserved
---

local LOG                            = require 'utils.log'
local REG                            = require 'wrap.registry'
local NET                            = require 'wrap.network.NetworkManager'

---@class NumberHelper @数字帮助类
local NumberHelper = {}

local isInited = false;
local function Init(mt)
end

---
--- 将数字转化为带逗号分隔的形式
--- @param 
--- @return
---
function NumberHelper:number_format(num,deperator)
    local str1 =""
    local str = tostring(num)
    local strLen = string.len(str)

    if deperator == nil then
        deperator = ","
    end
    deperator = tostring(deperator)

    for i=1,strLen do
        str1 = string.char(string.byte(str,strLen+1 - i)) .. str1
        if math.mod(i,3) == 0 then
            --下一个数 还有
            if strLen - i ~= 0 then
                str1 = ","..str1
            end
        end
    end
    return str1
end

---
--- 将数字保留n位小数
--- @param 
--- @return
---
function NumberHelper.GetPreciseDecimal(nNum, n)
    if type(nNum) ~= "number" then
        return nNum;
    end

    n = n or 0;
    n = math.floor(n)
    local fmt = '%.' .. n .. 'f'
    local nRet = tonumber(string.format(fmt, nNum))
    return nRet;
end

---
--- 获取圆周上的点
--- @param vector Vector3 @目标点
--- @param radius number @半径
--- @param degree number @角度 以正右方为0开始，逆时针
--- @return
---
function NumberHelper.GetPointOnCircle(vector , radius, degree)
    local radians = degree / 180 * 3.1415;
    return vector + (Vector3(math.cos(radians), math.sin(radians), 0) * radius);
end

local function ResFun()
    if(not isInited) then
        isInited = true;
        Init();
    end
    return NumberHelper;
end

return ResFun();