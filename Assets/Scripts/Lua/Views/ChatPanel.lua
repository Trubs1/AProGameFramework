--- Description: 登录面板
--- Author:Trubs (WQ)
--- Date: 2019/03/17

local ChatPanel = ChatPanel or BasePanel:New()

function ChatPanel:New (o)
	o = o or {}
    setmetatable(o, self)
    self.__index = self

	return o
end

function ChatPanel:Init()
	Log("~~~~ ChatPanel:Init",self.loginBtn)
	self.sendBtn.onClick:AddListener(function () self:OnSend() end)
	BasePanel.Init(self)
end

function ChatPanel:Show()
	BasePanel.Show(self)
end

function ChatPanel:OnSend()
	Log("<color=yellow>谁点我???? self:</color>",self)
end


return ChatPanel