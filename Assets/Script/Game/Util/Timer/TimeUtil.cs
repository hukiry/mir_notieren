using System;


public class TimeUtil
{

	/// <summary>
	/// 服务器时间(不含毫秒)
	/// </summary>
	public static int serverTime
	{
		get
		{
			return (int)(serverMescTime / 1000);
		}
	}

	/// <summary>
	/// 服务器时间(含毫秒)
	/// </summary>
	public static long serverMescTime
	{
		get
		{
			long t = 0;// LuaFunManager.Instance.GetServerMescTime();
			if (t == 0)
			{
				t = GetTotalMilliseconds();
			}
			return t;
		}
	}

    /// <summary>
    /// 利用两个时间DateTime天数差
    /// </summary>
    /// <param name="dt1"></param>
    /// <param name="dt2"></param>
    /// <returns></returns>
    public static int GetSubDays(DateTime dt1, DateTime dt2)
    {
        TimeSpan span = dt2.Subtract(dt1);
        if (span.Days == 0)
        {
            if (dt1.Month != dt2.Month || dt2.Day != dt1.Day)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return span.Days;
        }
    }

    //获取本地时间戳（秒）
    public static int GetTotalSeconds()
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return (int)ts.TotalSeconds;
	}

	//获取本地时间戳（毫秒）
	public static long GetTotalMilliseconds()
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return (long)ts.TotalMilliseconds;
	}

	//获取本地时间戳（毫秒）
	public static string GetTotalMillisecondsToString()
	{
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return ts.TotalMilliseconds.ToString("f0");
	}

	/// <summary>
	/// 返回当前时间   15:25:36-880
	/// </summary>
	/// <returns></returns>
	public static string GetNowTimer()
	{
		DateTime time = GetServerMescTime();
		return string.Format("{0}:{1}:{2}-{3}", time.Hour.ToString().PadLeft(2, '0'), time.Minute.ToString().PadLeft(2, '0'),
			time.Second.ToString().PadLeft(2, '0'), time.Millisecond.ToString().PadLeft(3, '0'));
	}

	/// <summary>
	/// 返回本地当前时间   03 24 15 25
	/// </summary>
	/// <returns></returns>
	public static string GetNowTimer1()
	{
		DateTime time = DateTime.Now;
		return string.Format("{0}.{1} {2}:{3}", time.Month.ToString(), time.Day.ToString(),
			time.Hour.ToString().PadLeft(2, '0'), time.Minute.ToString().PadLeft(2, '0'));
	}

	/// <summary>
	/// 返回本地当前时间   03_24-15_25_808
	/// </summary>
	/// <returns></returns>
	public static string GetNowTimer2()
	{
		DateTime time = DateTime.Now;
		return string.Format("{0}.{1} {2}:{3}:{4}", time.Month.ToString(), time.Day.ToString(),
			time.Hour.ToString().PadLeft(2, '0'), time.Minute.ToString().PadLeft(2, '0'), time.Millisecond.ToString().PadLeft(3, '0'));
	}

	/// <summary>
	/// 返回本地当前时间   03.24 15.25.808
	/// </summary>
	/// <returns></returns>
	public static string GetNowTimer3()
	{
		DateTime time = DateTime.Now;
		return string.Format("{0}.{1} {2}.{3}.{4}", time.Month.ToString(), time.Day.ToString(),
			time.Hour.ToString().PadLeft(2, '0'), time.Minute.ToString().PadLeft(2, '0'), time.Millisecond.ToString().PadLeft(3, '0'));
	}

	/// <summary>
	/// 返回本地当前时间   03 24 15 25
	/// </summary>
	/// <returns></returns>
	public static string GetNowTimer4()
	{
		DateTime time = DateTime.Now;
		return string.Format("{0}-{1} {2}.{3}", time.Month.ToString(), time.Day.ToString(),
			time.Hour.ToString().PadLeft(2, '0'), time.Minute.ToString().PadLeft(2, '0'));
	}

	/// <summary>
	/// 返回 00:00:00 格式时间
	/// </summary>
	public static string toTimeString(float time_seconds)
	{
		int h = (int)(time_seconds) / 3600;
		int m = (int)(time_seconds) % 3600 / 60;
		int s = (int)(time_seconds) % 3600 % 60;
		return string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
	}
	/// <summary>
	/// 返回 00:00 格式时间
	/// </summary>
	public static string ToTimeStringMS(float time_seconds)
	{
		int m = (int)(time_seconds) / 60;
		int s = (int)(time_seconds) % 60;
		return string.Format("{0:D2}:{1:D2}", m, s);
	}

	//返回秒 时间差 00 格式时间
	public static string timeDifference(long endTime)
	{
		return string.Format("{0:D2}", timeDifferenceInt(endTime));
	}

	public static int timeDifferenceInt(long endTime)
	{
		return (int)(endTime - serverTime);
	}

	public static DateTime GetServerTime()
	{
		return ToDateTime(serverTime);
	}

	public static DateTime GetServerMescTime()
	{
		return ToDateTime(serverMescTime);
	}

	/// <summary>
	/// 返回00:00:00
	/// </summary>
	public static string GetServerTimeString()
	{
		return GetServerTime().ToString("HH:mm:ss");
	}

	public static DateTime ToDateTime(int unix)
	{
		DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		return dtStart.AddSeconds(unix);
	}

	public static DateTime ToDateTime(long unix)
	{
		DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		return dtStart.AddMilliseconds(unix);
	}

	/// <summary>
	/// 获取当前时间13位时间戳
	/// </summary>
	/// <returns></returns>
	public static long GetTimeStamp()
	{
		long time = (long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime()).TotalSeconds;
		return time;
	}

	/// <summary>
	/// 获取服务器13位时间戳
	/// </summary>
	/// <returns></returns>
	public static long GetServerTimeStamp()
	{
		return long.Parse(GetServerTime().ToLongTimeString());
	}
}

