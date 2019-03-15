--
-- Copyright (C) GoodTeamStudio, All rights reserved!
--
-- Author: Zaxbbun.Du
--

local pprint = require 'utils.pprint'

pprint.defaults.show_all = true

local pformat = pprint.pformat

local levels = {
    debug = 1,
    info  = 2,
    warn  = 3,
    error = 4,

    Ndebug = 100,
    Ninfo  = 200,
    Nwarn  = 300,
    Nerror = 400,
}

local loggers
local logpath
local progname

---@class LOG @打印工具类
---@field debug function
---@field info function
---@field warn function
---@field error function
---@field Ndebug function
---@field Ninfo function
---@field Nwarn function
---@field Nerror function
local LOG = { }

local function console_logger(threhold)
    local Util = LuaFramework.Util
    return function (lv, msg)
        if lv >= threhold then
            local logger
            if lv == levels.debug then
                logger = Util.Log
            elseif lv == levels.info then
                logger = Util.LogWarning
            elseif lv == levels.warn then
                logger = Util.LogWarning
            elseif lv == levels.Ndebug then
                logger = Util.Log
            elseif lv == levels.Ninfo then
                logger = Util.LogWarning
            elseif lv == levels.Nwarn then
                logger = Util.LogWarning
            else

                if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.WindowsEditor or UnityEngine.Application.platform == UnityEngine.RuntimePlatform.OSXEditor ) then
                    logger = Util.LogError
                else
                    logger = Util.LogWarning
                end


            end
            logger(msg)
        end
    end
end

local function file_logger(name, level)
    --local fp
    --
    --local function create_file()
    --    local filename = string.format('%s/%s-%s.%s', logpath, progname, os.date '%Y%m%d%H%M%S', name)
    --    return io.open(filename, 'w')
    --end
    --
    return function (lv, msg)
        --if lv >= level then
        --    fp = fp or create_file()
        --    fp:write(msg)
        --    fp:flush()
        --end
    end
end

local function Log(level, msg)
    for _, logger in pairs(loggers) do
        logger(level, msg)
    end
   -- print(debug.traceback());
end

function LOG.init(prog, path, threhold)
    progname = prog
    logpath  = path
    loggers  = { }
    threhold = levels[threhold or 'debug']

    --打开移动版本的打印
    table.insert(loggers, console_logger(threhold))   --非移动设备才打印log

    --if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.WindowsEditor or UnityEngine.Application.platform == UnityEngine.RuntimePlatform.OSXEditor ) then
    --    table.insert(loggers, console_logger(threhold))   --非移动设备才打印log
    --end

    for level, value in pairs(levels) do
        table.insert(loggers, file_logger(level, value))
    end
end

function LOG.free()
    loggers = nil
end

local function format_args( ... )
    local buff = { }

    for i = 1, select('#', ...) do
        local arg = select(i, ...)
        table.insert(buff, pformat(arg) .. ' ')
    end

    return buff
end

local string_format = string.format
local string_upper  = string.upper
LOG = setmetatable(LOG,{
    __index = function (LOG, key)
        local level = assert(levels[key])

        local date   = os.date
        local unpack = table.unpack or unpack

        return function(fmt, ...)
            local msg

            if level % 100 > 0 then
                msg = string_format('[%s] %s', date('%m-%d %H:%M:%S'), string_format(fmt, unpack(format_args(...))))
            else
                msg = string_format('[%s] %s', date('%m-%d %H:%M:%S'), table.concat(format_args(fmt, ...)))
            end
            Log(level, msg)
        end
    end
})


return LOG;