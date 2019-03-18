using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LuaFramework
{
    public class TimerInfo
    {
        public long tick;
        public bool stop;
        public bool delete;
        public string name;
        public ITimerBehaviour target;
        public int interval = 1;

        public TimerInfo(string name, ITimerBehaviour target, int interval = 1)
        {
            this.name = name;
            this.target = target;
            delete = false;
            if (interval <= 0)
            {
                interval = 1;
            }
            this.interval = interval;
        }

        public void Destory()
        {
            target.OnDestory();
        }
    }

    public class TimerManager : Manager
    {
        private float interval = 0;
        private Dictionary<string, TimerInfo> nameDict = new Dictionary<string, TimerInfo>();
        private List<TimerInfo> objects = new List<TimerInfo>();

        public float Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        // Use this for initialization
        void Start()
        {
            StartTimer(AppConst.TimerInterval);
        }

        /// <summary>
        /// 启动计时器
        /// </summary>
        /// <param name="interval"></param>
        public void StartTimer(float value)
        {
            interval = value;
            InvokeRepeating("Run", 0, interval);
        }

        /// <summary>
        /// 停止计时器
        /// </summary>
        public void StopTimer()
        {
            CancelInvoke("Run");
        }

        /// <summary>
        /// 添加计时器事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="o"></param>
        public void AddTimerEvent(string name, TimerInfo info)
        {
            if (info == null)
            {
                return;
            }
            if (!nameDict.ContainsKey(name))
            {
                nameDict.Add(name, info);
                objects.Add(info);
            }
            else
            {
                Debug.Log("timer " + name + " has already added");

                nameDict[name].delete = true;
                nameDict[name] = info;
                objects.Add(info);
            }
        }

        /// <summary>
        /// 删除计时器事件
        /// </summary>
        /// <param name="name"></param>
        public void RemoveTimerEvent(string name)
        {
            TimerInfo info;
            if (nameDict.TryGetValue(name, out info))
            {
                if (info != null)
                {
                    info.delete = true;
                }
            }
        }

        /// <summary>
        /// 停止计时器事件
        /// </summary>
        /// <param name="info"></param>
        public void StopTimerEvent(string name)
        {
            TimerInfo info;
            if (nameDict.TryGetValue(name, out info))
            {
                if (info != null)
                {
                    info.stop = true;
                }
            }
        }

        /// <summary>
        /// 继续计时器事件
        /// </summary>
        /// <param name="info"></param>
        public void ResumeTimerEvent(string name)
        {
            TimerInfo info;
            if (nameDict.TryGetValue(name, out info))
            {
                if (info != null)
                {
                    info.stop = false;
                }
            }
        }

        /// <summary>
        /// 检查是否已经包含计时器事件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckTimerEvent(string name)
        {
            if (!nameDict.ContainsKey(name))
            {
                return false;
            }
            else
            {
                return !nameDict[name].delete;
            }
        }

        /// <summary>
        /// 计时器运行
        /// </summary>
        void Run()
        {
            if (objects.Count == 0) return;
            for (int i = 0; i < objects.Count; i++)
            {
                TimerInfo o = objects[i];
                if (o.delete || o.stop) { continue; }
                ITimerBehaviour timer = o.target;
                if (timer.IsDiscard())
                {
                    o.delete = true;
                }
                else
                {
                    o.tick++;
                    if (o.tick % o.interval == 0)
                    {
                        timer.TimerUpdate();
                    }
                }
            }
            /////////////////////////清除标记为删除的事件///////////////////////////
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].delete) { objects[i].Destory(); objects.Remove(objects[i]); }
            }
        }
    }
}