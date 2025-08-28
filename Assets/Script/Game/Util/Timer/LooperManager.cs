using System;
using System.Collections.Generic;
using UnityEngine;

public class LooperManager : MonoBehaviour
{

    private static LooperManager _Instance;

    /// <summary>
    /// 实体
    /// </summary>
    public static LooperManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject go = new GameObject("LooperManager");
                DontDestroyOnLoad(go);

                _Instance = go.AddComponent<LooperManager>();
            }

            return _Instance;
        }
    }

    //帧循环
    public interface Looper
    {
        void OnLoopUpdate();

        void OnApplicationQuit();
    }

    List<Looper> mLoopAbles = new List<Looper>();

    /// <summary>
    /// 添加looper
    /// </summary>
    /// <param name="looper"></param>
    public void AddLoopAble(Looper looper)
    {
        mLoopAbles.Add(looper);
    }

    void Update()
    {
        for (int i = 0; i < mLoopAbles.Count; i++)
        {
#if !UNITY_EDITOR
			try {
#endif
			mLoopAbles[i].OnLoopUpdate();
#if !UNITY_EDITOR
			} catch (Exception err) {
				LogManager.LogError(err.Message, err.StackTrace);
			}
#endif
        }
    }

    void OnApplicationQuit()
    {
        for (int i = mLoopAbles.Count - 1; i >= 0; --i)
        {
            mLoopAbles[i].OnApplicationQuit();
            mLoopAbles.RemoveAt(i);
        }
    }
}
