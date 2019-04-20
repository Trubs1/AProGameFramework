using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using LuaFramework;

namespace LuaFramework
{
    public class View : Base, IView
    {
        public virtual void OnMessage(IMessage message)
        {
        }
    }
}