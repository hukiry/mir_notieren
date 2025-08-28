using System;


/// <summary>
/// 定时器抽象实体
/// </summary>
public abstract class AbsTimerData
{
    //唯一id
    public int id;

    //延迟秒数
    public float delay;

    //计时开始时间
    public float start;

    //时间间隔
    public float interval;

    //重复次数
    public int repeat;

    //回调
    public abstract void DoAction();
}

/// <summary>
/// 无参数定时器实体
/// </summary>
internal class TimerData : AbsTimerData
{
    public Action m_action;

    public TimerData(Action action)
    {
        this.m_action = action;
    }

    public override void DoAction()
    {
        if (m_action != null) m_action();
    }
}

/// <summary>
/// 1个参数定时器实体
/// </summary>
/// <typeparam name="T">参数1</typeparam>
internal class TimerData<T> : AbsTimerData
{
    public T Param1 { get; set; }

    public Action<T> m_action;

    public TimerData(T agr1, Action<T> action)
    {
        this.Param1 = agr1;
        this.m_action = action;
    }

    public override void DoAction()
    {
        if (m_action != null) m_action(Param1);
    }
}

/// <summary>
/// 2个参数定时器实体
/// </summary>
/// <typeparam name="T">参数1</typeparam>
/// <typeparam name="U">参数2</typeparam>
internal class TimerData<T, U> : AbsTimerData
{
    public T Param1 { get; set; }

    public U Param2 { get; set; }

    public Action<T, U> m_action;

    public TimerData(T agr1, U agr2, Action<T, U> action)
    {
        this.Param1 = agr1;
        this.Param2 = agr2;
        this.m_action = action;
    }

    public override void DoAction()
    {
        if (m_action != null) m_action(Param1, Param2);
    }
}

/// <summary>
/// 3个参数定时器实体
/// </summary>
/// <typeparam name="T">参数1</typeparam>
/// <typeparam name="U">参数2</typeparam>
/// <typeparam name="V">参数3</typeparam>
internal class TimerData<T, U, V> : AbsTimerData
{
    public T Param1 { get; set; }

    public U Param2 { get; set; }

    public V Param3 { get; set; }

    public Action<T, U, V> m_action;

    public TimerData(T agr1, U agr2, V agr3, Action<T, U, V> action)
    {
        this.Param1 = agr1;
        this.Param2 = agr2;
        this.Param3 = agr3;
        this.m_action = action;
    }

    public override void DoAction()
    {
        if (m_action != null) m_action(Param1, Param2, Param3);
    }
}