--- Description: 登录面板
--- Author:Trubs (WQ)
--- Date: 2019/03/17
local canUseGM = true
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
	local inputStr = self.inputField.text
	if '' == inputStr then return end
	self.contentTxt.text = self.contentTxt.text .. "\n" .. inputStr--先显示到本地聊天框,待真正发送到服务器时候取消缓冲
	if not canUseGM then
		--直接广播到其他接听者
		self.inputField.text = ''
	else
		if "^GM:" == string.sub(inputStr,1,4) then
			self:GMHandler(string.sub(inputStr,5))
		else
			self.inputField.text = ''
			--广播到其他接听者
		end
	end
end

function ChatPanel:GMHandler(gmCode)
	Log("<color=yellow>gmCode:</color>",gmCode)
	loadstring(gmCode)()
end

--正式上线包需要注释的函数
function ChatPanel:LateUpdate()
	if UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftControl) and UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Return) then
		self:OnSend()
	end
end


return ChatPanel