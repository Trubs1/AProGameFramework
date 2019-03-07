--- description: 登录面板
--- author:Trubs (WQ)
--- date: 2019/03/05

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
	print("~~~~Init",self.loginBtn)
	self.loginBtn.onClick:AddListener(function ()
			print("~~~~谁点我?")
			self.titleTxt.text = "Loading .. ."
			self.titleBgImg.color = Color.yellow
		end)
end

function LoginPanel:Show()
	print("~~~~ LoginPanel:Show")
	self.gameObject:SetActive(true);
end

return LoginPanel