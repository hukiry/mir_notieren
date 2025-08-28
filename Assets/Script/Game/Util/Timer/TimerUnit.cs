using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 计时器
/// </summary>
public class TimerUnit
{
    //自增Id
    private static int mIncreaseId = 0;

    //计时器列表
    private static List<AbsTimerData> mTimerList = new List<AbsTimerData>();

    //原子锁
    private static readonly object mLock = new object();


    /**
     * 清理所有定时器
     */
    public static void Reset()
    {
        mIncreaseId = 0;
        mTimerList.Clear();
    }

    /**
     * 移除计时器
     */
    public static void DeleteTimer(int timerId)
    {
        for (int i = 0; i < mTimerList.Count; ++i)
        {
            if (mTimerList[i].id == timerId) { 
                mTimerList.RemoveAt(i); break; 
            }
        }
    }

    //事件全局LoopAble
    public static LooperManager.Looper Looper = new ThisLooper();

    //自身Looper
    private class ThisLooper : LooperManager.Looper
    {
        public static float nowtime = Time.time;

        public void OnLoopUpdate()
        {
            int index = mTimerList.Count;

            ThisLooper.nowtime = Time.time;

            while (index > 0)
            {
                AbsTimerData timer = mTimerList[--index];

                if (ThisLooper.nowtime >= timer.start)
                {
                    timer.DoAction(); timer.start += timer.interval;

                    //-1表示repeat, 小于0删除
                    if (timer.repeat != -1 && --timer.repeat <= 0)
                    {
                        mTimerList.Remove(timer);
                    }
                }
            }
        }

        /// <summary>
        /// 应用程序退出
        /// </summary>
        public void OnApplicationQuit()
        {
            mTimerList.Clear();
        }
    }

    #region 添加计时器
    /// <summary>
    /// 添加定时对象
    /// </summary>
    /// <param name="start">延迟启动时间。（毫秒）</param>
    /// <param name="interval">重复间隔，为零不重复。（毫秒）</param>
    /// <param name="handler">定时处理方法</param>
    /// <returns>定时对象Id</returns>
    public static int AddTimer(float delay, float interval, Action callback)
    {
        return AddTimer(delay, interval, 1, new TimerData(callback));
    }

    /// <summary>
    /// 添加定时对象
    /// </summary>
    public static int AddTimer(float delay, float interval, int repeat, Action callback)
    {
        return AddTimer(delay, interval, repeat, new TimerData(callback));
    }

    /// <summary>
    /// 添加定时对象
    /// </summary>
    public static int AddTimer<T>(float delay, float interval, Action<T> callback, T arg1)
    {
        return AddTimer(delay, interval, 1, new TimerData<T>(arg1, callback));
    }

    /// <summary>
    /// 添加定时对象
    /// </summary>
    public static int AddTimer<T, U>(float delay, float interval, Action<T, U> callback, T arg1, U arg2)
    {
        return AddTimer(delay, interval, 1, new TimerData<T, U>(arg1, arg2, callback));
    }

    /// <summary>
    /// 添加定时对象
    /// </summary>
    public static int AddTimer<T, U, V>(float delay, float interval, Action<T, U, V> callback, T agr1, U agr2, V agr3)
    {
        return AddTimer(delay, interval, 1, new TimerData<T, U, V>(agr1, agr2, agr3, callback));
    }

    /// <summary>
    /// 添加定时对象
    /// </summary>
    /// <param name="start">延迟启动时间。（毫秒）</param>
    /// <param name="interval">重复间隔，为零不重复。（毫秒）</param>
    public static int AddTimer(float delay, float interval, int repeat, AbsTimerData timer, bool safe = false)
    {
        timer.id        = ++mIncreaseId;
        timer.interval  = interval;
        timer.repeat    = repeat;
        timer.start     = (!safe ? Time.time : ThisLooper.nowtime) + delay + interval;

        mTimerList.Insert(0, timer);

        return timer.id;
    }

    #endregion
}
