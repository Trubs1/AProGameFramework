--- Description: 登录面板
--- Author:Trubs (WQ)
--- Date: 2019/03/05

local LoginPanel = LoginPanel or BasePanel:New();--这儿相对于将BasePanel设置为自身的metatable
--local LoginPanel = {}

function LoginPanel:New (o)
	o = o or {}
    setmetatable(o, self)
    self.__index = self

	return o
end

function LoginPanel:Init()
	Log("~~~~ LoginPanel:Init",self.loginBtn)
	self.loginBtn.onClick:AddListener(function ()
		Log("<color=yellow>谁点我????:</color>",self)
			self.titleTxt.text = "Loading .. ."
			self.titleBgImg.color = Color.yellow
			self:OnLogin()
		end)
	BasePanel.Init(self);
end

function LoginPanel:Show()
	--self.gameObject:SetActive(true);
	BasePanel.Show(self)
end

function LoginPanel:OnLogin()
    coroutine.start(
        function()
	        --coroutine.wait(1);
	        Log("<color=yellow>开始加载场景::</color>","Demo");
			UnityEngine.SceneManagement.SceneManager.LoadScene("Demo");		
        end);
end


return LoginPanel