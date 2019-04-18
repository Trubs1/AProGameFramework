
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework {
    public class AppConst {
        public const bool UpdateMode = false;                       //更新模式-默认关闭 
        public const bool LuaByteMode = true;                       //Lua字节码模式-默认关闭 
#if UNITY_EDITOR
        public const bool LuaBundleMode = false;                    //编辑器下默认AssetBundle为false lua代码即改即现
#else
        public const bool LuaBundleMode = true;                     //Lua代码AssetBundle模式
#endif
        public const int TimerInterval = 1;
        public const int GameFrameRate = 60;                        //游戏帧频

        public const string AppName = "APro";                       //应用程序名称
        public const string LuaTempDir = "Lua/";                    //临时目录
        public const string AppPrefix = AppName + "_";              //应用程序前缀
        public const string ExtName = ".unity3d";                   //素材扩展名
        public const string AssetDir = "StreamingAssets";           //素材目录 
        //public const string WebUrl = "http://localhost:6688/";      //测试更新地址
        public const string WebUrl = "http://192.168.21.148/StreamingAssets/";      //测试更新地址

        public static string UserId = string.Empty;                 //用户ID
        public static int SocketPort = 0;                           //Socket服务器端口
        public static string SocketAddress = string.Empty;          //Socket服务器地址



        public static string FrameworkRoot {
            get {
                return Application.dataPath + "/LuaFramework";
            }
        }

        public static string[] luaDirs = {
            FrameworkRoot + "/ToLua/Lua",//框架 tolua lua文件目录
            Application.dataPath + "/Scripts/Lua",//游戏 lua逻辑目录
        };


        public const bool DebugMode = false;                       //调试模式-用于内部测试
        /// <summary>
        /// 如果想删掉框架自带的例子，那这个例子模式必须要
        /// 关闭，否则会出现一些错误。
        /// </summary>
        public const bool ExampleMode = false;                       //例子模式 
    }
}