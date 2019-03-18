---
--- 对象池
--- @version: 1.0.0
--- @author cuishaojie
--- @date: 2017/11/13
--- @remarks Copyright (c) 2017, Magicell.
--- All rights reserved
---

local LOG                            = require 'util.log'
local REG                            = require 'wrap.registry'
local NET                            = require 'wrap.network.NetworkManager'
local ResourceMgr                    = REG.LuaFramework.LuaHelper.GetResManager()
local ObjectPoolMgr                  = REG.LuaFramework.LuaHelper.GetObjectPoolManager()
local UnityHelper                    = require 'util.UnityHelper'

---@class ObjectPoolManager @对象池
local ObjectPoolManager = {
    bundleDict = {};

}

local isInited = false;
local function Init(mt)
end


---
--- 通过已有对象创建资源池
--- @param
--- @return
---
function ObjectPoolManager.CreatePoolByExist(name, initSize, maxSize, object)
    ObjectPoolMgr:CreatePool(name,initSize,maxSize,object);
end

---
--- 创建资源池
--- @param 
--- @return
---
function ObjectPoolManager.CreatePool(name, initSize, maxSize, path)
    local bundle, abName = UnityHelper.GetFileName(path,'/');
    local prefab = ResourceMgr:LoadPrefabSync(bundle, abName);

    ObjectPoolManager.bundleDict[name] = bundle;
    ObjectPoolMgr:CreatePool(name,initSize,maxSize,prefab);
end

---
--- 销毁资源池
--- @param 
--- @return
---
function ObjectPoolManager.DestroyPool(name, unloadBundle)
    ObjectPoolMgr:DestroyPool(name);

    if(unloadBundle) then
        if(ObjectPoolManager.bundleDict[name] ~= nil) then
            ResourceMgr:UnloadAssetBundle(ObjectPoolManager.bundleDict[name], false);
            ObjectPoolManager.bundleDict[name] = nil;
        end
    end
end

---
--- 从资源池中获取对象
--- @param 
--- @return UnityEngine.GameObject
---
function ObjectPoolManager.Get(name)
    return ObjectPoolMgr:Get(name);
end

---
--- 将对象返回到资源池中
--- @param 
--- @return
---
function ObjectPoolManager.release(name, go)
    if(UnityHelper.NotNil(go)) then
        ObjectPoolMgr:Release(name, go);
    end
end

local function ResFun()
    if(not isInited) then
        isInited = true;
        Init();
    end
    return ObjectPoolManager;
end

return ResFun();