--- Copyright (c) 2019 WangQiang(279980661@qq.com)
--- description: 面板管理器
--- author:Trubs (WQ)
--- date: 2019/03/05

local cPanelManager = LuaFramework.LuaHelper.GetPanelManager()

PanelManager = {
	panels = {}
}


function PanelManager:Init()
end

function PanelManager:Show(panelCfg)
	if self:IsShowing(panelCfg) then return end
	local panel = self:GetPanel(panelCfg)
	panel:Show()
end

function PanelManager:Hide(panelCfg)
end

function PanelManager:GetPanel(panelCfg)
	local name = panelCfg[2]
	local panel = self.panels[name]
	if nil ~= panel then return panel end

    panel = require(panelCfg[1]).New()
    local gObj = cPanelManager:CreatePanelBySync(name,panel)
    panel.gameObject = gObj
    panel:Init()
    self.panels[name] = panel
    return panel
end

function PanelManager:IsShowing(panelCfg)
end


return PanelManager