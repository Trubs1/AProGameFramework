---
--- unity工具类
--- @version: 1.0.0
--- @author cuishaojie
--- @date: 2017/10/24
--- @remarks Copyright (c) 2017, Magicell.
--- All rights reserved
---

local LOG                            = require 'utils.log'
local REG                            = require 'wrap.registry'
local ResourceMgr                    = require 'wrap.resource.ResourceManager'

---@class UnityHelper @unity工具类
local UnityHelper = {}

local isInited = false;
local function Init(mt)
end

---
--- 判断GameObject是否不为空
--- @param obj UnityEngine.GameObject
--- @return boolean
---
function UnityHelper.NotNil(obj)
    return obj ~= nil and obj.Equals ~= nil and not obj:Equals(nil);
end

---
--- 销毁全部子对象
--- @param obj UnityEngine.GameObject
--- @return
---
function UnityHelper.DestoryAllChildren(obj)
    for i = 0 , obj.transform.childCount-1  do
        obj.transform:GetChild(obj.transform.childCount-1-i).gameObject:Destroy();
    end
end

---
--- 将世界坐标转化到UI坐标中
--- @param
--- @return
---
function UnityHelper.TransformPosition(transform, postion)
    return REG.magicell.PostionBinder.TransformPosition(transform, postion,nil);
end

---
--- 获取Text组件
--- @param obj UnityEngine.GameObject
--- @return UnityEngine.UI.Text
---
function UnityHelper.GetText(obj)
    return obj:GetComponent(typeof(REG.UnityEngine.UI.Text));
end

---
--- 获取Imaget组件
--- @param obj UnityEngine.GameObject
--- @return UnityEngine.UI.Image
---
function UnityHelper.GetImage(obj)
    return obj:GetComponent(typeof(REG.UnityEngine.UI.Image));
end

---
--- 获取Button组件
--- @param obj UnityEngine.GameObject
--- @return UnityEngine.UI.Button
---
function UnityHelper.GetButton(obj)
    return obj:GetComponent(typeof(REG.UnityEngine.UI.Button));
end

---
--- 获取ProgressBar组件
--- @param obj UnityEngine.GameObject
--- @return magicell.ProgressBar
---
function UnityHelper.GetProgressBar(obj)
    return obj:GetComponent(typeof(REG.magicell.ProgressBar));
end

---
--- 获取CanvasGroup组件
--- @param obj UnityEngine.GameObject
--- @return UnityEngine.CanvasGroup
---
function UnityHelper.GetCanvasGroup(obj)
    return obj:GetComponent(typeof(REG.UnityEngine.CanvasGroup));
end


---
--- 设置图片是否灰显
--- @param image UnityEngine.UI.Image
--- @return
---
function UnityHelper.SetImageGray(image, isGray)
    if(isGray) then
        image.material = ResourceMgr:GetMaterial("gray");
    else
        image.material = nil;
    end
end

---
--- 设置带遮罩的图片灰显，由于有特殊shader，所以与普通图片区别
--- @param 
--- @return
---
function UnityHelper.SetMaskImageGray(image, isGray)
    if(isGray) then
        image.material = ResourceMgr:GetMaterial("DephMaskGray");
    else
        image.material = ResourceMgr:GetMaterial("DephMask");
    end
end

---
--- 读取网络图片
--- @param image UnityEngine.UI.Image
--- @param url string
--- @return
---
function UnityHelper.LoadOnlineImage(image,url)
    ResourceMgr:LoadOnlineImage(url,function(sprite)
        if(UnityHelper.NotNil(image)) then
            image.sprite = sprite
        end
    end)
end

---
--- 文件名帮助方法
--- @param
--- @return
---
function UnityHelper.GetFileName(strurl, strchar)
    local ts = string.reverse(strurl);
    local param1, param2 = string.find(ts, strchar or '/');  -- 这里以"/"为例
    local m = string.len(strurl) - param2 + 1;
    local name = string.sub(strurl, m+1, string.len(strurl))
    local path = string.sub(strurl, 1, m-1);
    return path,name;
end

---
--- 显示特效
--- @param
--- @return UnityEngine.GameObject
---
function UnityHelper.ShowEffect(parent, path, duration)
    local effect = ResourceMgr:CreateFullName(path);
    if(effect ~= nil) then
        effect.transform:SetParent(parent.transform);
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localRotation = Quaternion();
        effect.transform.localScale = Vector3.one;

        if(duration) then
            coroutine.start(function()
                coroutine.wait(duration);
                if(UnityHelper.NotNil(effect)) then
                    effect:Destroy();
                end
            end)
        end
        return effect;
    end
    return nil;
end

local function ResFun()
    if(not isInited) then
        isInited = true;
        Init();
    end
    return UnityHelper;
end

return ResFun();