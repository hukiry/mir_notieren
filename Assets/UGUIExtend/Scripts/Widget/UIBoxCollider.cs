using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/UI Box Collider", 10), RequireComponent(typeof(RectTransform),typeof(CanvasRenderer))]
[DrawIcon(typeof(BoxCollider2D), null, HierarchyIconLayout.After)]
public sealed class UIBoxCollider : Graphic
{
    private static Material m_materialBoxCollider = null;
    private static Material MaterialBoxCollider
    {
        get
        {
            if (m_materialBoxCollider == null)
            {
                m_materialBoxCollider = new Material(Shader.Find("UI/Default"));
            }
            return m_materialBoxCollider;
        }
    }

	protected override void OnEnable()
	{
		base.OnEnable();
		this.material = MaterialBoxCollider;
		this.material.hideFlags = HideFlags.HideInInspector | HideFlags.DontSave;
		this.color = new Color(0, 0, 0, 0);
		this.ShowCanvasRenderer();
	}

    /// <summary>
    /// 隐藏透明渲染，可以降到draw
    /// </summary>
    void ShowCanvasRenderer()
	{
		CanvasRenderer canvasRenderer = GetComponent<CanvasRenderer>();
		if (canvasRenderer)
			canvasRenderer.cullTransparentMesh = true;
	}

//#if UNITY_EDITOR
//	private Color drawColor;
//	private Vector3[] worldCorners = new Vector3[4];
//	private Vector2 lastAnchored;
//	private void OnDrawGizmos()
//	{
//        if (this.raycastTarget)
//        {
//            this.DrawRect(true);

//            Gizmos.color = drawColor;
//            Gizmos.DrawLine(worldCorners[0], worldCorners[1]);
//            Gizmos.DrawLine(worldCorners[1], worldCorners[2]);
//            Gizmos.DrawLine(worldCorners[2], worldCorners[3]);
//            Gizmos.DrawLine(worldCorners[3], worldCorners[0]);
//            Gizmos.color = new Color(0, 0, 1, 0.2F);
//            //Gizmos.DrawIcon(space.position, "UI/1", true);//根据特性来绘制
//            Gizmos.color = Color.white;
//        }
//    }

//	private void DrawRect(Color drawColor,Color drawCube)
//	{	
//		if (this.raycastTarget)
//		{
//			RectTransform rectTransform = this.transform as RectTransform;
//			rectTransform.GetWorldCorners(worldCorners);
			
//			for (int i = 0; i < 4; i++)
//			{
//				Gizmos.DrawLine(worldCorners[i], worldCorners[(i + 1) % 4]);
//			}
//		}
//	}
//	private void DrawRect(bool isDraw=false)
//	{

//		if (lastAnchored != rectTransform.anchoredPosition|| isDraw)
//		{
//			lastAnchored = rectTransform.anchoredPosition;
//			ColorUtility.TryParseHtmlString("#3DE64B", out drawColor);
//			worldCorners[0] = this.transform.TransformPoint(new Vector2(this.rectTransform.rect.x, this.rectTransform.rect.y));
//			worldCorners[1] = this.transform.TransformPoint(new Vector2(this.rectTransform.rect.x, this.rectTransform.rect.yMax));
//			worldCorners[2] = this.transform.TransformPoint(new Vector2(this.rectTransform.rect.xMax, this.rectTransform.rect.yMax));
//			worldCorners[3] = this.transform.TransformPoint(new Vector2(this.rectTransform.rect.xMax, this.rectTransform.rect.y));
//		}
//#endif
	//}
}