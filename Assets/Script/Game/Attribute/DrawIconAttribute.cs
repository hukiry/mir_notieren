
using System;

namespace UnityEngine
{
	/// <summary>
	/// 图标放在编辑文件夹下 size:64x64 , 32X32 或 16x16
	/// </summary>
	[AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
	public sealed class DrawIconAttribute : Attribute
	{
		public string IconNameOfScript = null;
		public string IconNameOfHierarchy=null;
		/// <summary>
		/// 脚本图标是否在检测面板上显示	 只对脚本图标有效
		/// </summary>
		public bool IsShowOnHierarchy = false;
		public Type typeNameOfScript;
		public HierarchyIconLayout hierarchyDrawIconLayout;

		/// <summary>
		/// 脚本图标显示
		/// </summary>
		public DrawIconAttribute(string IconNameOfScript)
		{
			this.IconNameOfScript = IconNameOfScript;
		}

		/// <summary>
		/// 脚本图标是否显示在检测面板上
		/// </summary>
		/// <param name="IconNameOfScript"></param>
		/// <param name="isShowOnHierarchy">只对脚本图标有效</param>
		///  <param name="layout">isShowOnHierarchy=true时，生效</param>
		public DrawIconAttribute(string IconNameOfScript,bool isShowOnHierarchy, HierarchyIconLayout layout = HierarchyIconLayout.None) :this(IconNameOfScript)
		{
			this.IsShowOnHierarchy = isShowOnHierarchy;
			this.hierarchyDrawIconLayout = layout;
		}

		/// <summary>
		/// 脚本图标 和 检测面板图标分开显示
		/// </summary>
		/// <param name="IconNameOfScript"></param>
		/// <param name="IconNameOfHierarchy"></param>
		public DrawIconAttribute(string IconNameOfScript, string IconNameOfHierarchy, HierarchyIconLayout layout=HierarchyIconLayout.None) : this(IconNameOfScript, false, layout)
		{
			this.IconNameOfHierarchy = IconNameOfHierarchy;
		}



		/// <summary>
		/// 脚本图标 和 检测面板图标分开显示
		// </summary>
		/// <param name="typeNameOfScript">Unity的脚本图标</param>
		/// <param name="IconNameOfHierarchy">显示面板的图标</param>
		/// <param name="layout">显示后</param>
		public DrawIconAttribute(Type typeNameOfScript, string IconNameOfHierarchy, HierarchyIconLayout layout = HierarchyIconLayout.None)
		{
			this.typeNameOfScript = typeNameOfScript;
			this.IconNameOfHierarchy = IconNameOfHierarchy;
			this.hierarchyDrawIconLayout = layout;
		}


		public static implicit operator bool(DrawIconAttribute drawIconAttribute)
		{
			return drawIconAttribute != null;
		}
	}

	public enum HierarchyIconLayout
	{
		None,
		Before,
		After
	}
}
