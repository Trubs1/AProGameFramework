--- Copyright (c) 2019 WangQiang(279980661@qq.com)
--- Description: 面板管理器
--- Author:Trubs (WQ)
--- Date: 2019/03/05


--[=[
未完成内容:
	监视:监视每个面板的情况,记录状态
	使用频率:频率等级高的,考虑不销毁

]=]

local cPanelManager = LuaFramework.LuaHelper.GetPanelManager()

PanelManager = {
	panels = {}
}

function PanelManager:Init()
end

function PanelManager:Show(panelCfg)
	local panel = self:GetPanel(panelCfg)
	if panel.isShow then return end
	panel:Show()
	panel.isShow = true
end

function PanelManager:Hide(panelCfg)
	local panel = self:GetPanel(panelCfg)
	panel:Hide()
	panel.isShow = false
end

function PanelManager:GetPanel(panelCfg)
	local name = panelCfg[2]
	local panel = self.panels[name]
	if nil ~= panel then return panel end

    panel = require(panelCfg[1]):New()
    local gObj = cPanelManager:CreatePanelBySync(name,panel)
    panel.gameObject = gObj
    panel:Init()
    self.panels[name] = panel
    return panel
end

function PanelManager:IsShowing(panelCfg)
	local name = panelCfg[2]
	local panel = self.panels[name]
	if nil ~= panel then return panel.isShow end
	return false
end


return PanelManager