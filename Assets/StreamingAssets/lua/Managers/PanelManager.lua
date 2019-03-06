--- @author Trubs
--- @date: 2019/3/5
local cManager                        = LuaFramework.LuaHelper.GetPanelManager()

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
    local gObj = cManager:CreatePanelBySync(name,panel)
    panel.gameObject = gObj
    -- local behaviour = gObj:GetComponent("LuaBehaviour")
    --     print("~~~~behaviour",behaviour)
    -- if behaviour then
    --     behaviour:Initiate(panel)
    -- end
    panel:Init()
    self.panels[name] = panel
    return panel
end

function PanelManager:IsShowing(panelCfg)
end


return PanelManager