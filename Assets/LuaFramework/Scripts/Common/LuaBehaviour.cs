///Copyright (c) 2019 WangQiang(279980661@qq.com)
///description: lua对象的行为类
///author:Trubs (WQ)
///Date:2019/03/05

using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace LuaFramework
{
    /// <summary>
    /// lua中的对象(GameObject)所有的特征
    /// </summary>
    public class LuaBehaviour : View
    {
        [System.Serializable]
        public struct Compnt
        {
            public string key;
            public Component cpnt;
        }
        protected LuaTable table_;
        public Compnt[] compnts;
        protected LuaFunction m_updateFunc;
        protected LuaFunction m_fixedUpdateFunc;

        public void InitCompnts()
        {
            if (null == compnts)
            {
                Debug.LogWarning("此脚本似乎没有获取任何组件:" + this.gameObject.ToString());
                return;
            }
            foreach (Compnt compnt in compnts)
            {
                if (0 != compnt.key.Length)
                {
                    table_[compnt.key] = compnt.cpnt;
                }
                else
                {
                    string name = compnt.cpnt.name;
                    if (name.IndexOf("(Clone)") > -1)
                    {
                        name = name.Replace("(Clone)", string.Empty);
                    }
                    table_[name] = compnt.cpnt;
                }
            }
        }

        public void Initiate(LuaTable table)
        {
            table_ = table;
            InitCompnts();
            table_["transform"] = transform;
            m_updateFunc = table_["Update"] as LuaFunction;
            m_fixedUpdateFunc = table_["FixedUpdate"] as LuaFunction;
        }

        protected void Start()
        {
            if (table_ != null)
            {
                LuaFunction func = table_["Start"] as LuaFunction;
                if (func != null)
                {
                    func.Call(table_);
                }
            }
        }

        protected void Update()
        {
            if (null == m_updateFunc) return;
            m_updateFunc.Call(table_, Time.deltaTime);
        }

        protected void FixedUpdate()
        {
            if (null == m_fixedUpdateFunc) return;
            m_fixedUpdateFunc.Call(table_);
        }

        protected void OnEnable()
        {
            if (table_ != null)
            {
                LuaFunction func = table_["OnEnable"] as LuaFunction;
                if (func != null)
                {
                    func.Call(table_);
                }
            }
        }

        protected void OnDisable()
        {
            if (table_ != null)
            {
                LuaFunction func = table_["OnDisable"] as LuaFunction;
                if (func != null)
                {
                    func.Call(table_);
                }
            }
        }

        protected void OnDestroy()
        {
            if (table_ != null)
            {
                LuaFunction func = table_["OnDestroy"] as LuaFunction;
                if (func != null)
                {
                    func.Call(table_);
                }

                table_.Dispose(true);
                table_ = null;
            }
#if ASYNC_MODE
            string abName = name.ToLower().Replace("panel", "");
            ResManager.UnloadAssetBundle(abName + AppConst.ExtName);
#endif
            Util.ClearMemory();
        }
    }
}