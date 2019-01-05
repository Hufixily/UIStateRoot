using System;
using System.Collections.Generic;
using UnityEngine;


namespace game
{
    public class TimeMgr : SingletonMonoBehaviour<TimeMgr>
    {
        //定时器
        public class Timer
        {
            public static int s_idCounter = 0;
            public int id;
            public float interval;
            public float delay;//-1表明和interval一样
            public int num;//-1表明一直循环
            public Action<System.Object> onTimer;
            public System.Object param;

            public System.DateTime lastTime;
            public bool isRemove;
            private bool isPause;
            public bool IsPause
            {
                get
                {
                    return isPause;
                }
                set
                {
                    isPause = value;
                    if (isPause)
                    {
                        System.TimeSpan span = System.DateTime.Now - lastTime;
                        interval -= (float)span.TotalSeconds;
                    }
                    else
                    {
                        lastTime = System.DateTime.Now;
                    }
                }
            }
            public int counter;

            public Timer(float interval, Action<System.Object> onTimer, System.Object param, float delay = -1, int num = -1)
            {
                this.id = ++s_idCounter;
                this.interval = interval;
                this.delay = delay;
                this.num = num;
                this.onTimer = onTimer;
                this.param = param;
                isRemove = false;
                lastTime = System.DateTime.Now;//Time.unscaledTime 只能在主线程中调用
                counter = -1;
            }

            public void Release()
            {
                if (!TimeMgr.s_debugDestroy)
                    TimeMgr.Instance.RemoveTimer(this);
            }

            public override string ToString()
            {
                return string.Format("{0} 首次延迟:{1} 间隔:{2} 执行到第几次:{3}", lastTime, delay, interval, num);
            }
        }

        #region Fields
        public int m_pauseCounter = 0;//暂停计数，设置成public界面上可以看到

        float m_logicTime = 0;
        float m_logicTimeDelta = 0;
        LinkedList<Timer> m_timers = new LinkedList<Timer>();
        List<Timer> m_tempTimers = new List<Timer>();//用于定时器触发时回调的临时变量
        System.Object m_lock = new System.Object();
        System.DateTime m_startTime;
        #endregion

        #region Properties
        public bool IsPause { get { return m_pauseCounter > 0; } }
        public bool IsStop { get { return Time.timeScale == 0; } }

        //逻辑时间，用于游戏逻辑(ai、移动)
        public float logicTime { get { return this.m_logicTime; } }
        public float logicDelta { get { return this.m_logicTimeDelta; } }
        //服务器时间
        public float srvTime { get; private set; }

        //普通时间，受缩放影响
        public float time { get { return Time.time; } }
        public float delta { get { return Time.deltaTime; } }

        //真实时间,不受缩放影响
        public float realTime { get { return Time.realtimeSinceStartup; } }
        public float realDelta { get { return Time.unscaledDeltaTime; } }


        #endregion

        protected override void Init()
        {
            base.Init();
            m_startTime = System.DateTime.Now;//由于要支持其他线程可以调用定时器，所以不能用unity的Time的接口
        }

        #region Mono Frame
        void Update()
        {
            if (IsPause)
            {
                m_logicTimeDelta = 0;
            }
            else
            {
                m_logicTimeDelta = Time.deltaTime;
                m_logicTime += m_logicTimeDelta;
            }

            System.DateTime now = System.DateTime.Now;
            //定时器回调
            lock (m_lock)
            {
                LinkedListNode<Timer> node = m_timers.First;
                Timer timer;
                LinkedListNode<Timer> cur;
                while (node != null)
                {
                    timer = node.Value;
                    if (timer.IsPause)
                    {
                        node = node.Next;
                        continue;
                    }
                    cur = node;
                    node = node.Next;
                    System.TimeSpan span = now - timer.lastTime;
                    if ((timer.counter != -1 && span.TotalSeconds >= timer.interval) ||
                        (timer.counter == -1 && span.TotalSeconds >= (timer.delay == -1 ? timer.interval : timer.delay)))
                    {
                        ++timer.counter;
                        timer.lastTime = now;
                        //timer.onTimer(timer.param);回调中可能会删除别的定时器，所以移到外面回调
                        m_tempTimers.Add(timer);
                        if (timer.num != -1 && (timer.counter + 1) >= timer.num)
                            RemoveTimer(cur);
                    }
                }
            }

            if (m_tempTimers.Count != 0)
            {
                for (int i = 0; i < m_tempTimers.Count; ++i)
                {
                    try
                    {
                        m_tempTimers[i].onTimer(m_tempTimers[i].param);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }

                m_tempTimers.Clear();
            }

        }
        #endregion

        #region Private Methods
        void OnTimerAdapter(System.Object param)
        {
            Action a = (Action)param;
            a();
        }
        void RemoveTimer(LinkedListNode<Timer> node)
        {
            if (node.Value.isRemove)
            {
                Debug.LogError("[TimeMgr]逻辑错误");
            }

            node.Value.isRemove = true;
            m_timers.Remove(node);

        }
        #endregion

        /// 设置游戏停止:Time.timeScale = 0
        public void SetGameStop(bool stop)
        {
            //这里没有做冲突处理机制，以后再做
            if (stop)
            {
                Time.timeScale = 0;
            }
            else
            {

                Time.timeScale = 1;
            }
        }

        //让游戏逻辑暂停
        public void AddPause()
        {
            if (m_pauseCounter == 0)
            {
            }
            ++m_pauseCounter;
        }

        public void SubPause()
        {
            --m_pauseCounter;
            if (m_pauseCounter < 0)
            {
                m_pauseCounter = 0;
                Debug.LogError("[TimeMgr]逻辑出错，暂停计数小于0");
            }
        }
        public void ResetPause()
        {
            m_pauseCounter = 0;
        }

        //设置变速
        public void SetTimeScale(float scale)
        {
            //这里没有做冲突处理机制，以后再做
            Time.timeScale = scale;
        }


        //delay多少秒后执行第一次，然后每间隔interval多少秒执行一次
        //delay=-1,则和interval一样。num=-1则没有次数限制
        public Timer AddTimer(float interval, Action<System.Object> onTimer, System.Object param, float delay = -1, int num = 1)
        {
            //这里单位从秒转为tick
            Timer t = new Timer(interval, onTimer, param, delay, num);
            lock (m_lock)
            {
                m_timers.AddLast(t);
            }

            return t;
        }

        //delay多少秒后执行第一次，然后每间隔interval多少秒执行一次
        //delay=-1,则和interval一样。num=-1则没有次数限制
        public Timer AddTimer(float interval, Action onTimer, float delay = -1, int num = 1)
        {

            return AddTimer(interval, OnTimerAdapter, onTimer, delay, num);
        }

//         //每日定时器
//         public Timer DailyTimerImplement(int hour, int minute, int second, Action onRuner)
//         {
//             DateTime now = new DateTime(GameApp.my.serviceTime);
//             DateTime timer = new DateTime(now.Year, now.Month, now.Day, hour, minute, second);
//             if (timer < now)
//             {
//                 timer = timer.AddDays(1);//当前时间已过，延后一天
//             }
//             TimeSpan delayTime = timer - now;
//             float delay = (float)delayTime.TotalSeconds;
//             return AddTimer(3600 * 24, onRuner, delay, -1);
//         }
// 
//         //是否超过指定时间
//         public bool OutDailyTime(int hour,int minute,int second)
//         {
//             DateTime now = new DateTime(GameApp.my.serviceTime);
//             DateTime timer = new DateTime(now.Year, now.Month, now.Day, hour, minute, second);
//             return timer < now;
//         }

        public void RemoveTimer(Timer t)
        {
            lock (m_lock)
            {
                if (t.isRemove)
                    return;
                LinkedListNode<Timer> node = m_timers.FindLast(t);
                if (node == null)
                {
                    Debug.Log("[TimeMgr]逻辑错误");
                    return;
                }
                RemoveTimer(node);
            }
        }

        public bool Cancel(int timerId)
        {
            lock (m_lock)
            {
                LinkedListNode<Timer> node = m_timers.First;
                while (node != null)
                {
                    var value = node.Value;
                    if (value.id == timerId)
                    {
                        m_timers.Remove(value);
                        return true;
                    }

                    node = node.Next;
                }
                return false;
            }
        }

        public void RemoveAllTimer()
        {
            lock(m_lock)
            {
               while(m_timers.Count > 0)
                {
                    RemoveTimer(m_timers.First);
                }
                m_timers.Clear();
                Debug.Log("[TimeMgr]RemoveAllTimer");
            }
        }
    }
}


