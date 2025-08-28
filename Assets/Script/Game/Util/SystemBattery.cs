using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBattery : MonoBehaviour
{
	public bool monitorBattery = true;
	//private GUIStyle labelStyle, batteryStyle, batteryBackgroundStyle;
	private string NetworkReachabilityState
	{
		get
		{
			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
			{
				return "WiFi";
			}
			else if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				return "";
			}
			else
			{
				return "4G";
			}

		}
	}

	private float batteryLevel => Mathf.Abs( SystemInfo.batteryLevel>1?1: SystemInfo.batteryLevel);
	Texture2D aTexture,bgTexture;

	// Start is called before the first frame update
	void Start()
    {
		bgTexture = new Texture2D(130, 30);
		for (int i = 0; i < bgTexture.width; i++)
		{
			for (int j = 0; j < bgTexture.height; j++)
			{
				if (i > 1 && j > 1 && i < bgTexture.width - 2 && j < bgTexture.height - 2)
				{
					bgTexture.SetPixel(i, j, new Color(0, 1, 0));
				}
				else
				{
					bgTexture.SetPixel(i, j, new Color(0, 1, 0, 0.5f));
				}
			}
		}
		bgTexture.Apply();


        aTexture = new Texture2D(130, 28, TextureFormat.RGBA32, false);
		for (int i = 0; i < aTexture.width; i++)
		{
			for (int j = 0; j < aTexture.height; j++)
			{
				aTexture.SetPixel(i, j, new Color(0, 10, 0, 0.5f));
			}
		}
		aTexture.Apply();
    }

    // Update is called once per frame
    void OnGUI()
    {
		if (monitorBattery)
		{
			const int w = 200;
			//电池框+电池fill
			GUI.contentColor = batteryLevel < 0.5 ? Color.yellow : Color.green;
			GUI.Box(new Rect(Screen.width - w, 5, 100, 30), bgTexture);
            GUI.Box(new Rect(Screen.width - w+1, 6, 98* batteryLevel, 28), aTexture);


            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.contentColor = Color.green;
			GUI.Label(new Rect(Screen.width - 80, 3, 80, 35), NetworkReachabilityState);
			//电池文本+网络
			GUI.contentColor = Color.white;
			GUI.skin.label.fontSize = 20;
			GUI.Label(new Rect(Screen.width - w+6, 4, 80, 30), (batteryLevel * 100).ToString() + "%");
			
		}
	}
}
