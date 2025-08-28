
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hukiry
{
    
    [ExecuteInEditMode][DrawIcon(typeof(Text), null)]
    public class HukirySupperText : Text, IPointerClickHandler
#if UNITY_EDITOR
        , IPointerEnterHandler, IPointerExitHandler
#endif
    {
        public int LineCount { get; private set; }//行数
        [SerializeField] private float characterspacing;
        //颜色渐变
        [SerializeField] private bool applyGradient;
        [SerializeField] private Color m_gradientTop = new Color(1f, 1f, 1f, 1f);
        [SerializeField] private Color m_gradientBottom = new Color(0f, 0f, 0f, 1f);

        [Tooltip("Shadow effect style.")]//特效部分
        [SerializeField] private ShadowStyle m_Style = ShadowStyle.Shadow;
        [SerializeField] private Color m_effectColor = new Color(0f, 0f, 0f, 0.5f);
        [SerializeField] private Vector2 m_EffectDistance = new Vector2(1f, -1f);
        [SerializeField] private bool m_UseGraphicAlpha = true;

        public static bool m_EnableHelp = false;


        //超链接部分
        /// <summary>
        /// 委托事件
        /// </summary>
        /// <param name="id">超链接的唯一ID</param>
        /// <param name="hrefValue">超链接的值</param>
        public delegate void UnityEventHandler(int id, string hrefValue);
        //[任意字符#任意字符#00FFFC] 
        private static readonly Regex _inputTagRegex = new Regex(@"\[(.+?|\-{0,1}|\d+)#(.+?)#?([0-9a-fA-F]{8}|[0-9a-fA-F]{6}|[0-9a-fA-F]{4}|[0-9a-fA-F]{3})?\]", RegexOptions.Singleline);
        private StringBuilder m_textBuilder = new StringBuilder();
        private readonly List<HrefInfo> m_listHrefInfos = new List<HrefInfo>();
        private readonly UIVertex[] m_TempVerts = new UIVertex[4];
        [SerializeField] private bool m_isEnableUnderLine = false;
        public event UnityEventHandler OnHrefHandler = default;
        private int hrefLightTag = int.MaxValue;//超链接高亮显示

        //文本部分
        private readonly UIVertex[] tempVerts = new UIVertex[4];
        List<UIVertex> s_Verts = new List<UIVertex>();
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            _PopulateMesh(toFill);
            UpdateFontSpace(toFill, this, this.characterspacing); //字符间距控制

            //特效控制
            toFill.GetUIVertexStream(s_Verts);
            int start = 0, end = s_Verts.Count;
            ApplyShadow(s_Verts, m_effectColor, ref start, ref end, m_EffectDistance, m_Style, m_UseGraphicAlpha);
            toFill.Clear();
            toFill.AddUIVertexTriangleStream(s_Verts);
            s_Verts.Clear();
        }
        protected void _PopulateMesh(VertexHelper toFill)
        {
            if (this.font == null)
            {
                return;
            }
            string txt = MatchText(this.text);
            this.m_DisableFontTextureRebuiltCallback = true;
            TextGenerationSettings generationSettings;
            generationSettings = this.GetGenerationSettings(base.rectTransform.rect.size);
            this.cachedTextGenerator.PopulateWithErrors(txt, generationSettings, base.gameObject);
            IList<UIVertex> verts;
            verts = this.cachedTextGenerator.verts;
            float num;
            num = 1f / this.pixelsPerUnit;
            int count;
            count = verts.Count;
            if (count <= 0)
            {
                toFill.Clear();
                return;
            }

            Vector2 vector;
            vector = new Vector2(verts[0].position.x, verts[0].position.y) * num;
            vector = base.PixelAdjustPoint(vector) - vector;
            toFill.Clear();
            if (vector != Vector2.zero)
            {
                for (int i = 0; i < count; i++)
                {
                    int num2;
                    num2 = i & 3;
                    ref UIVertex reference = ref this.tempVerts[num2];
                    reference = verts[i];
                    this.tempVerts[num2].position *= num;
                    this.tempVerts[num2].position.x += vector.x;
                    this.tempVerts[num2].position.y += vector.y;
                    if (num2 == 3)
                    {
                        DrawGradientColor(this.tempVerts);
                        toFill.AddUIVertexQuad(this.tempVerts);
                    }
                }
            }
            else
            {
                for (int j = 0; j < count; j++)
                {
                    int num3;
                    num3 = j & 3;
                    ref UIVertex reference2 = ref this.tempVerts[num3];
                    reference2 = verts[j];
                    this.tempVerts[num3].position *= num;

                    if (num3 == 3)
                    {
                        DrawGradientColor(this.tempVerts);
                        toFill.AddUIVertexQuad(this.tempVerts);
                    }
                }
            }
            this.m_DisableFontTextureRebuiltCallback = false;

            //toFill.FillMesh(Graphic.workerMesh);//this.canvasRenderer.SetMesh(Graphic.workerMesh);
            this.LineCount = this.cachedTextGenerator.lineCount;
            this.DealHrefInfo(toFill);
        }
#if UNITY_EDITOR
        [ContextMenu("UnderLine Help")]
        private void EnableHelp()
        {
            m_EnableHelp = true;
        }
#endif

        #region 公共方法
        public void SetGradient(bool applyGradient, Color top, Color bottom)
        {
            this.applyGradient = applyGradient;
            this.m_gradientTop = top;
            this.m_gradientBottom = bottom;
            this.UpdateGeometry();
        }

        #endregion

        #region 超链接
        private struct HrefInfo
        {
            /// <summary>
            /// 超链接id
            /// </summary>
            public int Id;
            /// <summary>
            /// 超链接标签
            /// </summary>
            public string tag;
            /// <summary>
            /// 超链接文本
            /// </summary>
            public string HrefValue;
            /// <summary>
            /// 颜色
            /// </summary>
            public Color color;
            /// <summary>
            /// 顶点开始索引值
            /// </summary>
            public int StartIndex;
            /// <summary>
            /// 顶点结束索引值
            /// </summary>
            public int EndIndex;
            /// <summary>
            /// 碰撞盒范围
            /// </summary>
            public List<Rect> RectBox;

            public string GetLabel() => this.Id == 0 ? this.tag : this.HrefValue;

        }
        private string MatchText(string inputText)
        {
            m_listHrefInfos.Clear();
            m_textBuilder.Clear();
            (int textIndex, int charIndex) = (0, 0);
            MatchCollection matchArray = _inputTagRegex.Matches(inputText);
            foreach (Match match in matchArray)
            {
                string preMatchStr = inputText.Substring(textIndex, match.Index - textIndex);
                m_textBuilder.Append(preMatchStr);

                string tag = match.Groups[1].Value;
                int.TryParse(tag, out int tagId);
                //int tagId = int.Parse(match.Groups[1].Value);
#if UNITY_2019_1_OR_NEWER || UNITY_2018_4_36
                m_textBuilder.Append((hrefLightTag == tagId ? $"<color=#00F8>" : $"<color=#{match.Groups[3].Value}>") + match.Groups[2].Value + "</color>");
                charIndex += ReplaceRichText(preMatchStr).Length * 4;
                int startIndex = charIndex;
                charIndex += match.Groups[2].Value.Length * 4;
                int endIndex = charIndex - 1;
#else
                m_textBuilder.Append(isHrefLight? $"<color=#00F8>" : $"<color=#{match.Groups[3].Value}>");
                int startIndex = m_textBuilder.Length * 4;
				m_textBuilder.Append(match.Groups[2].Value);
				int endIndex = m_textBuilder.Length * 4 - 1;
				m_textBuilder.Append("</color>");
#endif
                HrefInfo hrefInfo = new HrefInfo();
                hrefInfo.RectBox = new List<Rect>();
                hrefInfo.Id = tagId;
                ColorUtility.TryParseHtmlString("#" + match.Groups[3].Value, out hrefInfo.color);
                hrefInfo.HrefValue = match.Groups[2].Value;
                hrefInfo.StartIndex = startIndex;
                hrefInfo.EndIndex = endIndex;
                hrefInfo.tag = tag;
                m_listHrefInfos.Add(hrefInfo);

                textIndex = match.Index + match.Length;
            }
            m_textBuilder.Append(inputText.Substring(textIndex, inputText.Length - textIndex));
            return m_textBuilder.ToString();
        }

        private void DealHrefInfo(VertexHelper toFill)
        {
            const int OffsetX_WIDTH = 5;
            UIVertex tempVertex = UIVertex.simpleVert;
            int length = m_listHrefInfos.Count;
            if (length <= 0) return;
            for (int i = 0; i < length; i++)
            {
                var info = m_listHrefInfos[i];
                if (info.StartIndex >= toFill.currentVertCount) continue;
                toFill.PopulateUIVertex(ref tempVertex, info.StartIndex);
                var bounds = new Bounds(tempVertex.position, Vector3.zero);
                for (int j = info.StartIndex; j < info.EndIndex; j++)//超链接的开始索引到结束索引
                {
                    if (j >= toFill.currentVertCount) break;
                    toFill.PopulateUIVertex(ref tempVertex, j);
                    if (tempVertex.position.x < bounds.min.x)
                    {
                        info.RectBox.Add(new Rect(bounds.min, bounds.size));// 换行重新添加包围框  
                        bounds = new Bounds(tempVertex.position, Vector3.zero);
                    }
                    else
                        bounds.Encapsulate(tempVertex.position); // 扩展包围框  
                }
                info.RectBox.Add(new Rect(bounds.min, bounds.size));//添加包围盒

                if (m_isEnableUnderLine)
                {
                    TextGenerator underlineText = new TextGenerator();
                    underlineText.Populate("_", GetGenerationSettings(rectTransform.rect.size));
                    IList<UIVertex> verts = underlineText.verts;
                    for (int k = 0; k < info.RectBox.Count; k++)
                    {
                        Vector3[] tempPos = new Vector3[4];//计算下划线的位置
                        tempPos[0] = info.RectBox[k].position + new Vector2(-OffsetX_WIDTH, fontSize * 0.1f);
                        tempPos[1] = tempPos[0] + new Vector3(info.RectBox[k].width + 10, 0.0f);
                        tempPos[2] = info.RectBox[k].position + new Vector2(info.RectBox[k].width + OffsetX_WIDTH * 2, 0.0f);
                        tempPos[3] = info.RectBox[k].position + new Vector2(-OffsetX_WIDTH, 0);
                        for (int n = 0; n < 4; n++)
                        {
                            m_TempVerts[n] = verts[n];
                            m_TempVerts[n].color = hrefLightTag == info.Id ? Color.blue * 0.5F : info.color;
                            m_TempVerts[n].position = tempPos[n];
                            if (n == 3)
                                toFill.AddUIVertexQuad(m_TempVerts);  //添加下划线
                        }
                    }
                }
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
            foreach (var hrefInfo in m_listHrefInfos)
            {
                for (var i = 0; i < hrefInfo.RectBox.Count; ++i)
                {
                    if (hrefInfo.RectBox[i].Contains(localPoint))
                    {
                        this.OnHrefHandler?.Invoke(hrefInfo.Id, hrefInfo.GetLabel());
                        return;
                    }
                }
            }
        }
#if UNITY_EDITOR
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
            foreach (var hrefInfo in m_listHrefInfos)
            {
                for (var i = 0; i < hrefInfo.RectBox.Count; ++i)
                {
                    if (hrefInfo.RectBox[i].Contains(localPoint))
                    {
                        hrefLightTag = hrefInfo.Id;
                        goto HREF_TAG;
                    }
                }
            }
        HREF_TAG:
            this.UpdateGeometry();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            hrefLightTag = int.MaxValue;
            this.UpdateGeometry();

        }
#endif
        #endregion

        #region 颜色渐变
        private void DrawGradientColor(UIVertex[] tempVerts)
        {
            if (applyGradient)
            {
                (Color s_c0, Color s_c1) = ((color * m_gradientBottom), (color * m_gradientTop));
                for (int i = 0; i < 4; i++)
                {
                    ref UIVertex reference2 = ref tempVerts[i];
                    reference2.color = i >= 2 ? s_c0 : s_c1;
                }
            }
        }
        #endregion

        #region 特效(阴影，描边)
        private enum ShadowStyle
        {
            None = 0,
            Shadow,
            Outline,
            Outline8,
            Shadow3,
        }

        private void ApplyShadow(List<UIVertex> verts, Color color, ref int start, ref int end, Vector2 distance, ShadowStyle style, bool alpha)
        {
            if (style == ShadowStyle.None || color.a <= 0)
                return;

            var x = distance.x;
            var y = distance.y;
            // Append Shadow.
            ApplyShadowZeroAlloc(verts, color, ref start, ref end, x, y, alpha);

            switch (style)
            {
                case ShadowStyle.Shadow3:
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, x, 0, alpha);
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, 0, y, alpha);
                    break;
                case ShadowStyle.Outline:
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, x, -y, alpha);
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, -x, y, alpha);
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, -x, -y, alpha);
                    break;
                case ShadowStyle.Outline8:
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, x, -y, alpha);
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, -x, y, alpha);
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, -x, -y, alpha);
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, -x, 0, alpha);
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, 0, -y, alpha);
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, x, 0, alpha);
                    ApplyShadowZeroAlloc(verts, color, ref start, ref end, 0, y, alpha);
                    break;
            }
        }
        private void ApplyShadowZeroAlloc(List<UIVertex> verts, Color color, ref int start, ref int end, float x, float y, bool alpha)
        {
            // Check list capacity.
            var count = end - start;
            var neededCapacity = verts.Count + count;
            if (verts.Capacity < neededCapacity)
                verts.Capacity *= 2;
            var normalizedIndex = -1;
            // Add
            var vt = default(UIVertex);
            for (var i = 0; i < count; i++) verts.Add(vt);
            // 拷贝值
            for (var i = verts.Count - 1; count <= i; i--) verts[i] = verts[i - count];
            //先显示特效，后显示文字
            // Append shadow vertices to the front of list.* The original vertex is pushed backward.
            for (var i = 0; i < count; ++i)
            {
                vt = verts[i + start + count];

                var v = vt.position;
                vt.position.Set(v.x + x, v.y + y, v.z);

                var vertColor = m_effectColor;
                vertColor.a = alpha ? color.a * vt.color.a / 255 : color.a;
                vt.color = vertColor;

                // Set UIEffect parameters
                if (0 <= normalizedIndex)
                {
                    vt.uv0 = new Vector2(
                        vt.uv0.x,
                        normalizedIndex
                    );
                }

                verts[i] = vt;
            }

            // Update next shadow offset.
            start = end;
            end = verts.Count;
        }

        #endregion

        #region 字符间距

        private static void UpdateFontSpace(VertexHelper vh, Text richText, float characterspacing)
        {
            if (!richText.IsActive() || vh.currentVertCount == 0 || characterspacing == 0 || richText == null)
                return;
            // 水平对齐方式
            HorizontalAligmentType alignment;
            if (richText.alignment == TextAnchor.LowerLeft || richText.alignment == TextAnchor.MiddleLeft || richText.alignment == TextAnchor.UpperLeft)
                alignment = HorizontalAligmentType.Left;
            else if (richText.alignment == TextAnchor.LowerCenter || richText.alignment == TextAnchor.MiddleCenter || richText.alignment == TextAnchor.UpperCenter)
                alignment = HorizontalAligmentType.Center;
            else
                alignment = HorizontalAligmentType.Right;
            Calculatecharacterspacing(vh, characterspacing, richText.text, alignment);
        }
        private static void Calculatecharacterspacing(VertexHelper vh, float characterspacing, string text, HorizontalAligmentType alignment)
        {
            (var vertexs, List<Line> lines) = (new List<UIVertex>(), new List<Line>());
            vh.GetUIVertexStream(vertexs);
            var lineTexts = text.Split('\n');
            // 根据lines数组中各个元素的长度计算每一行中第一个点的索引，每个字、字母、空格均占6个顶点
            for (var i = 0; i < lineTexts.Length; i++)
            {
                int characterLen = ReplaceRichText(lineTexts[i]).ToCharArray().Count();
                if (characterLen > 0)
                {
                    var last = lines.Count > 0 ? lines[lines.Count - 1] : null;
                    int endIndex = last != null ? last.EndVertexIndex + 1 : 0;
                    lines.Add(new Line(endIndex, characterLen));
                }
            }
            UIVertex vt;

            for (var i = 0; i < lines.Count; i++)
            {
                int endIndex = lines[i].EndVertexIndex;
                for (var j = lines[i].StartVertexIndex; j <= endIndex; j++)
                {
                    if (j < 0 || j >= vertexs.Count) continue;
                    vt = vertexs[j];
                    var charCount = lines[i].EndVertexIndex - lines[i].StartVertexIndex;
                    if (alignment == HorizontalAligmentType.Left)
                        vt.position += new Vector3(characterspacing * ((j - lines[i].StartVertexIndex) / 6), 0, 0);
                    else if (alignment == HorizontalAligmentType.Right)
                        vt.position += new Vector3(characterspacing * (-(charCount - j + lines[i].StartVertexIndex) / 6), 0, 0);
                    else if (alignment == HorizontalAligmentType.Center)
                    {
                        var offset = (charCount / 6) % 2 == 0 ? 0.5f : 0f;
                        vt.position += new Vector3(characterspacing * ((j - lines[i].StartVertexIndex) / 6 - charCount / 12 + offset), 0, 0);
                    }
                    vertexs[j] = vt;
                    // 以下注意点与索引的对应关系
                    if (j % 6 <= 2)
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);

                    if (j % 6 == 4)
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                }
            }
        }
        private static string ReplaceRichText(string str)
        {
            str = Regex.Replace(str, @"<color=(.+?)>", "");
            str = str.Replace("</color>", "");
            str = str.Replace("<b>", "");
            str = str.Replace("</b>", "");
            str = str.Replace("<i>", "");
            str = str.Replace("</i>", "");
            str = str.Replace("\t", "");
            str = str.Replace(" ", "");
            return str;
        }

        /// <summary>
        /// 字符间距
        /// </summary>
        private class Line
        {
            private int _startVertexIndex = 0;
            private int _endVertexIndex = 0;
            public int StartVertexIndex => _startVertexIndex;
            public int EndVertexIndex => _endVertexIndex;
            public Line(int startVertexIndex, int length)
            {
                _startVertexIndex = startVertexIndex;
                //每个字符有6个顶点
                int endIndex = length * 6 - 1;
                _endVertexIndex = startVertexIndex + endIndex;
            }
        }
        private enum HorizontalAligmentType { Left, Center, Right }

        #endregion


        public void SetShadowStyle()
        {
            this.m_Style = ShadowStyle.None;
        }

    }
}