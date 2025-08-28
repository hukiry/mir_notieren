using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneViewMenu
{
	struct Menu_data
	{
		public string text;
		public string tip;
		public int index;
	}


	static List<Menu_data> menuList = new List<Menu_data>()
	{
		new Menu_data (){ text = "改为普通", tip = "",index = 0},
		new Menu_data (){ text = "改为死地", tip = "",index = 1},
		new Menu_data (){ text = "改为超级死地", tip = "", index = 2},
		new Menu_data (){ text = "取消格子", tip = "",index = -1},
		new Menu_data (){ text = "修改参数", tip = "修改物品id, 云ID，格子治愈值", index = 101},
	};

	public static void DrawDropMenu(int value, Action<int, int> action)
	{
		GenericMenu gm = new GenericMenu();
		foreach (var item in menuList)
		{
			gm.AddItem(new GUIContent(item.text, item.tip), value == item.index, index =>
			{
				action?.Invoke(value, (int)index);
			}, item.index);
		}
		gm.ShowAsContext();

	}

}
