--- Description: 登录面板
--- Author:Trubs (WQ)
--- Date: 2019/03/05

local LoginPanel = LoginPanel or BasePanel:New()--这儿相对于将BasePanel设置为自身的metatable

function LoginPanel:New (o)
	o = o or {}
    setmetatable(o, self)
    self.__index = self

	return o
end

function LoginPanel:Init()
	Log("~~~~ LoginPanel:Init",self.loginBtn)
	--若像直接添加监听函数,那么函数里面不能直接访问self. 若本对象是唯一,且不会有派生,那么可以在new里用this解决
	self.loginBtn.onClick:AddListener(function () self:OnLogin() end)
	BasePanel.Init(self)
end

function LoginPanel:Show()
	BasePanel.Show(self)
end

function LoginPanel:OnLogin()
	Log("<color=yellow>谁点我???? self:</color>",self)
	self.titleTxt.text = "Loading .. ."
	self.titleBgImg.color = Color.yellow

    coroutine.start(
        function()
	        --coroutine.wait(1)
	        Log("<color=yellow>开始加载场景::</color>",PanelMgr)
			--UnityEngine.SceneManagement.SceneManager.LoadScene("Demo")	
    		--PanelMgr:Hide(PanelsCfg.LoginPanel)
    		self:Hide()
    		PanelMgr:Show(PanelsCfg.ChatPanel)
        end)
end


return LoginPanel