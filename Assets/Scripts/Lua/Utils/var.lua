--
-- Copyright (C) GoodTeamStudio, All rights reserved!
--
-- Author: Zaxbbun.Du
--

--
-- @brief 本地数据存取
--

local pprint = require 'util.pprint'

local filepath = nil
local var = { }

--
-- isKeep 是否保留当前数据   默认不保存
-- @brief 打开本地存储文件
--
function var.init(_filepath, isKeep)
    if(isKeep == nil and filepath ~= nil and filepath ~= '' and _filepath ~= nil and _filepath ~= '' and _filepath ~= filepath) then
       for k, v in pairs(var) do
            if type(v) ~= 'function'then
                var[k] = nil
            end
        end
    end

    filepath = _filepath
    pcall(function ()
        local fp = io.open(filepath)
        if(fp == nil)then return end
        local data = fp:read('*all')
        fp:close()

        local fn = loadstring(data)
        if fn then
            data = fn()
            for k, v in pairs(data) do
                if k ~= 'init' and k ~= 'clearData' then
                    var[k] = v
                end
            end
        end
    end)
end


--
-- @brief: 清空数据
-- @param none.
-- @returns: none.
-- @note none.
--
function var.clearData(path)
    local fp = io.open(path, 'w')
    if(fp == nil)then return end
    fp:write('')
    fp:close()
end

--
-- 返回本地存储表
--
return setmetatable(var, {
    __call = function ()
        -- pcall(function ()
            if(filepath == nil or filepath == '') then return end
            -- require 'core.log'.error('filepath=%s',filepath)
            -- print(debug.traceback())
            local data = pprint.pformat(var)
            local fn = loadstring('return ' .. data)
            local fp = io.open(filepath, 'w')
            if(fp == nil)then return end
            fp:write(string.dump(fn))
            --fp:write('return ' .. data)
            fp:close()
        -- end)
    end
})