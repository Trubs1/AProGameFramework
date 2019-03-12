---
--- 时间工具类
--- @version: 1.0.0
--- @author cuishaojie
--- @date: 2017/12/4
--- @remarks Copyright (c) 2017, Magicell.
--- All rights reserved
---

local LOG                            = require 'utils.log'
local REG                            = require 'wrap.registry'
local NET                            = require 'wrap.network.NetworkManager'
local TimerMgr                       = require 'wrap.timer.TimerManager'

---@class TimeHelper @时间工具类
local TimeHelper = {}

local isInited = false;
local function Init(mt)
end

---
--- 获取剩余时间
--- @param 
--- @return
---
function TimeHelper.GetTimeLeft(timeStamp)
    return TimeHelper.FormatSecTime(timeStamp - TimerMgr.curServerTime);
end

---
--- 将秒格式化显示
--- @param 
--- @return
---
function TimeHelper.FormatSecTime(secTime)
    local isPassed = false;
    if(secTime < 0) then
        isPassed = true;
        secTime = -secTime;
    end

    local hour = math.modf(secTime / 3600)
    secTime = secTime - hour*3600
    local minute = math.modf(secTime / 60);

    local sec = (secTime - minute*60)

    local res = "";
    if(hour > 0)then
        res = string.format('%dh%02dm',hour, minute);
    else
        res = string.format('%dm%02ds',minute, sec);
    end

    if(isPassed) then
        res = res .. " ago";
    end

    return res;
end

---
--- 格式化时间戳
--- @param 
--- @return
---
function TimeHelper.FormatTimeStamp(t)
    return os.date("%Y-%m-%d %H:%M:%S",t)
end

local function ResFun()
    if(not isInited) then
        isInited = true;
        Init();
    end
    return TimeHelper;
end

return ResFun();