---
--- 玩家数据帮助类
--- @version: 1.0.0
--- @author cuishaojie
--- @date: 2017/11/9
--- @remarks Copyright (c) 2017, Magicell.
--- All rights reserved
---

local LOG                            = require 'util.log'
local REG                            = require 'wrap.registry'
local NET                            = require 'wrap.network.NetworkManager'
local UnityHelper                    = require 'util.UnityHelper'
local ResMgr                         = require 'wrap.resource.ResourceManager'
local Config                         = require 'etc.Config'

---@class PlayerHelper @玩家数据帮助类
local PlayerHelper = {}

local isInited = false;
local function Init(mt)
end

---
--- 设置玩家头像
--- @param image UnityEngine.UI.Image
--- @return
---
function PlayerHelper.SetPlayerImage(user, image)
    if (user.uid == nil) then
        return;
    end

    local iconUrl;
    if (user.uid > 0) then
        iconUrl = user.icon
        if(user.icon == nil or user.icon == "") then
            image.sprite = ResMgr:GetSpriteFullName("Common/head");
        end
    elseif (user.uid == 0) then
        iconUrl = Config.guest_icon_url .. '0' .. '.png'
    elseif (user.uid < 0) then
        iconUrl = Config.guest_icon_url .. (math.abs(user.uid) % 1121 + 1) .. '.png';
    end

    if (iconUrl ~= nil and iconUrl ~= "") then
        UnityHelper.LoadOnlineImage(image, iconUrl);
    end
end


local function ResFun()
    if(not isInited) then
        isInited = true;
        Init();
    end
    return PlayerHelper;
end

return ResFun();