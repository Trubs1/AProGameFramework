--- Description: lua逻辑层的门户入口
--- Author:Trubs (WQ)
--- Date: 2019/03/15


require("GlobalEnvironment")

--初始化完成 游戏框架(大部分C#+少许lua 基础)，发送链接服务器信息--
local function FirstTouchLua()
    print("<color=yellow>~~~开始lua逻辑 FirstTouchLua:</color>", os.time() )--这是唯一个print,其他地方请用Log,虽然这里也可以用,但
    PanelMgr:Show(PanelsCfg.LoginPanel)
    Log("<color=yellow>~~~~OnInitOK_初始化完成:</color>")
end

FirstTouchLua()--初摸