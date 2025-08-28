using UnityEngine;
using System.Collections;

public class TimerScaleManager : TimerBaseManager<TimerScaleManager>, IAnimatable
{

    public TimerScaleManager()
    {
        TimerManager.timerList.Add(this);
    }

    public override long currentTime
    {
        get { return (long)(Time.time * 1000); }
    }
}
