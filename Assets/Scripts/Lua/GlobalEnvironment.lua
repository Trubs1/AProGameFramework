--- Description: 环境变量初始化	按照可能最先调用的时序来初始化 注意:设置为全局变量必须满足: 1 不同的地方使用频繁, 2 整个游戏过程都可能用到
--- Author:Trubs (WQ)
--- Date: 2019/03/05

LOG = require("util.log");
Debug = LOG.Ndebug;
PanelsCfg = require("Common.PanelsCfg");
PanelMgr = require("Managers.PanelManager");
LuaMsgMgr = require("util.LuaMsgMgr");
