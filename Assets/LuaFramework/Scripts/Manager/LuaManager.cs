using UnityEngine;
using System.Collections;
using LuaInterface;
using System.IO;

namespace LuaFramework {
    public class LuaManager : Manager {
        private LuaState lua;
        private LuaLoader loader;
        private LuaLooper loop = null;

        // Use this for initialization
        void Awake() {
        }

        public void InitStart() {
            if (lua != null) Close();//为了重启lua虚拟机
            loader = new LuaLoader();
            lua = new LuaState();
            this.OpenLibs();
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, this);


            InitLuaPath();
            InitLuaBundle();
            this.lua.Start();    //启动LUAVM
            this.StartMain();
            this.StartLooper();
        }

        void StartLooper() {
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson() {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        void StartMain() {
            lua.DoFile("Main.lua");//第一次访问Lua层,初始化,布置全局变量

            //LuaFunction main = lua.GetFunction("Main");
            //main.Call();//第一次访问Lua层,初始化,布置全局变量
            //main.Dispose();
            //main = null;
        }
        
        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs() {
            lua.OpenLibs(LuaDLL.luaopen_pb);      
            lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);
            lua.OpenLibs(LuaDLL.luaopen_socket_core);

            this.OpenCJson();
        }

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        void InitLuaPath() {
            if (AppConst.DebugMode) {
                string rootPath = AppConst.FrameworkRoot;
                lua.AddSearchPath(rootPath + "/Scripts/Lua");
                lua.AddSearchPath(rootPath + "/ToLua/Lua");
            } else {
                lua.AddSearchPath(Util.DataPath + "lua");
            }
        }

        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        void InitLuaBundle() {
            if (loader.beZip) {
                string lua_root = Util.DataPath + "lua/";
                string[] files = Directory.GetFiles(lua_root, "lua*.unity3d", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; ++i)
                {
                    loader.AddBundle(files[i]);
                }

                //以前居然是这样枚举,坑!!!
                //loader.AddBundle("lua/lua.unity3d");
                //loader.AddBundle("lua/lua_unityengine.unity3d");
                //loader.AddBundle("lua/lua_common.unity3d");
                //loader.AddBundle("lua/lua_logic.unity3d");
            }
        }

        public void DoFile(string filename) {
            lua.DoFile(filename);
        }

        // Update is called once per frame
        public object[] CallFunction(string funcName, params object[] args) {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) {
                return func.LazyCall(args);
            }
            return null;
        }

        public void CallFunction(string funcName, string module = null)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if(null == func && null != module)
            {
                lua.DoFile(module);
                func = lua.GetFunction(funcName);
            }
            if(null == func)
            {
                Debug.LogError("当前Lua模块没有此方法"+ funcName+ "  module:" + module);
                return;
            }
            func.Call();
            func.Dispose();
            func = null;
        }

        public void LuaGC() {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close() {
            loop.Destroy();
            loop = null;

            lua.Dispose();
            lua = null;
            loader = null;
        }
    }
}