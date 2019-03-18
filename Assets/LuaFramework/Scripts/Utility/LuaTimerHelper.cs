using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuaFramework
{
    class LuaTimerBehaviour : ITimerBehaviour
    {
        private LuaFunction func;

        public LuaTimerBehaviour(LuaFunction func)
        {
            this.func = func;
        }

        public void TimerUpdate()
        {
            if (func != null)
            {
                func.Call();
            }
        }

        public bool IsDiscard()
        {
            return func == null || !func.IsAlive;
        }

        public void OnDestory()
        {
            this.func.Dispose();
            this.func = null;
        }
    }

    public static class LuaTimerHelper
    {
        public static void AddTimerEvent(string name, LuaFunction func, int interval)
        {
            if (interval <= 0)
            {
                interval = 1;
            }
            TimerManager timerManager = AppFacade.Instance.GetManager<TimerManager>(ManagerName.Timer);
            timerManager.AddTimerEvent(name, new TimerInfo(name, new LuaTimerBehaviour(func), interval));
        }

        public static void RemoveTimerEvent(string name)
        {
            TimerManager timerManager = AppFacade.Instance.GetManager<TimerManager>(ManagerName.Timer);
            timerManager.RemoveTimerEvent(name);
        }

        public static void StopTimerEvent(string name)
        {
            TimerManager timerManager = AppFacade.Instance.GetManager<TimerManager>(ManagerName.Timer);
            timerManager.StopTimerEvent(name);
        }

        public static void ResumeTimerEvent(string name)
        {
            TimerManager timerManager = AppFacade.Instance.GetManager<TimerManager>(ManagerName.Timer);
            timerManager.ResumeTimerEvent(name);
        }

        public static bool CheckTimerEvent(string name)
        {
            TimerManager timerManager = AppFacade.Instance.GetManager<TimerManager>(ManagerName.Timer);
            return timerManager.CheckTimerEvent(name);
        }
    }
}
