using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SwitchPanels : MonoBehaviour 
{
	public List<GameObject> Panels;
	private int activePanel = 0;

	private void Awake()
	{
		var toggle = GetComponent<Toggle> ();
		toggle.onValueChanged.AddListener (OnToggleClick);
	}

	public void OnToggleClick(bool isActive)
	{
		activePanel++;
		activePanel %= Panels.Count;
		for (int i = 0; i < Panels.Count; i++)
		{
			Panels[i].SetActive(i==activePanel);
		}
	}

}
