using System.Collections.Generic;
using UnityEngine;

public class TimerManager : TimerBaseManager<TimerManager>, IAnimatable
{

    public TimerManager()
    {
        LooperManager.Instance.AddLoopAble(this);
    }

    public static List<IAnimatable> timerList = new List<IAnimatable>();

    public override long currentTime
    {
        get { return (long)(Time.unscaledTime * 1000); }
    }
}
