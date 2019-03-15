--- Description: 登录面板
--- Author:Trubs (WQ)
--- Date: 2019/03/05

local LoginPanel = {}

LoginPanel.__index = LoginPanel

---
--- 构造方法
--- @return LoginPanel
---
function LoginPanel.New(target)
    if(target == nil) then
        local self = {};
        setmetatable(self, LoginPanel);
        return self;
    else
        return target;
    end
end

function LoginPanel:Init()
	Log("~~~~ LoginPanel:Init",self.loginBtn)
	self.loginBtn.onClick:AddListener(function ()
		Log("<color=yellow>点我干啥?:</color>",self)
			self.titleTxt.text = "Loading .. ."
			self.titleBgImg.color = Color.yellow
		end)
end

function LoginPanel:Show()
	Log("~~~~ LoginPanel:Show")
	self.gameObject:SetActive(true);
end

return LoginPanel