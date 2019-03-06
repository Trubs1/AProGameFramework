

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
	print("~~~~Awake",self.loginBtn)
	self.loginBtn.onClick:AddListener(function ()
			print("~~~~谁点我?")
		end)
end

function LoginPanel:Show()
	print("~~~~ LoginPanel:Show")
	self.gameObject:SetActive(true);
end

return LoginPanel