/*
 * Copyright (C) GoodTeamStudio, All rights reserved!
 * Author: Zaxbbun.Du
 */

using UnityEngine.EventSystems;
using LuaInterface;

namespace LuaFramework
{
    public class LuaEventTrigger : EventTrigger
    {
        private LuaTable table_;

        public void Initiate(LuaTable table)
        {
            table_ = table;
        }

        public override void OnInitializePotentialDrag(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnInitializePotentialDrag"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnBeginDrag(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnBeginDrag"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnDrag(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnDrag"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnEndDrag(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnEndDrag"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnPointerEnter(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnPointerEnter"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnPointerClick(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnPointerClick"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnPointerDown(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnPointerDown"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnPointerUp(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnPointerUp"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnPointerExit(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnPointerExit"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnSelect(BaseEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnSelect"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnDeselect(BaseEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnDeselect"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnUpdateSelected(BaseEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnUpdateSelected"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnScroll(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnScroll"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnDrop(PointerEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnDrop"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnCancel(BaseEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnCancel"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnMove(AxisEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnMove"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        public override void OnSubmit(BaseEventData ev)
        {
            if (table_ == null) return;
            LuaFunction func = table_["OnSubmit"] as LuaFunction;
            if (func != null)
            {
                func.Call(table_, ev);
            }
        }

        protected void OnDestroy()
        {
            if (table_ != null)
            {
                table_.Dispose();
                table_ = null;
            }
        }
    }
}