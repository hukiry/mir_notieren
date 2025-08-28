using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Hukiry.UI {
	//窗口选择器中绘制工具
    internal class SpriteDrawUtility
    {
        static Texture2D s_ContrastTex;

        // Returns a usable texture that looks like a high-contrast checker board.
        static Texture2D contrastTexture
        {
            get
            {
                if (s_ContrastTex == null)
                    s_ContrastTex = CreateCheckerTex(
                            new Color(0f, 0.0f, 0f, 0.5f),
                            new Color(1f, 1f, 1f, 0.5f));
                return s_ContrastTex;
            }
        }

        // Create a checker-background texture.
        static Texture2D CreateCheckerTex(Color c0, Color c1)
        {
            Texture2D tex = new Texture2D(16, 16);
            tex.name = "[Generated] Checker Texture";
            tex.hideFlags = HideFlags.DontSave;

            for (int y = 0; y < 8; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c1);
            for (int y = 8; y < 16; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c0);
            for (int y = 0; y < 8; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c0);
            for (int y = 8; y < 16; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c1);

            tex.Apply();
            tex.filterMode = FilterMode.Point;
            return tex;
        }

        // Create a gradient texture.
        static Texture2D CreateGradientTex()
        {
            Texture2D tex = new Texture2D(1, 16);
            tex.name = "[Generated] Gradient Texture";
            tex.hideFlags = HideFlags.DontSave;

            Color c0 = new Color(1f, 1f, 1f, 0f);
            Color c1 = new Color(1f, 1f, 1f, 0.4f);

            for (int i = 0; i < 16; ++i)
            {
                float f = Mathf.Abs((i / 15f) * 2f - 1f);
                f *= f;
                tex.SetPixel(0, i, Color.Lerp(c0, c1, f));
            }

            tex.Apply();
            tex.filterMode = FilterMode.Bilinear;
            return tex;
        }

        // Draws the tiled texture. Like GUI.DrawTexture() but tiled instead of stretched.
        static void DrawTiledTexture(Rect rect, Texture tex)
        {
            float u = rect.width / tex.width;
            float v = rect.height / tex.height;

            Rect texCoords = new Rect(0, 0, u, v);
            TextureWrapMode originalMode = tex.wrapMode;
            tex.wrapMode = TextureWrapMode.Repeat;
            GUI.DrawTextureWithTexCoords(rect, tex, texCoords);
            tex.wrapMode = originalMode;
        }

        // Draw the specified Image.
        public static void DrawSprite(Sprite sprite, Rect drawArea, Color color)
        {
            if (sprite == null)
                return;

            Texture2D tex = sprite.texture;
            if (tex == null)
                return;

            Rect outer = sprite.rect;
            Rect inner = outer;
            inner.xMin += sprite.border.x;
            inner.yMin += sprite.border.y;
            inner.xMax -= sprite.border.z;
            inner.yMax -= sprite.border.w;

            Vector4 uv4 = UnityEngine.Sprites.DataUtility.GetOuterUV(sprite);
            Rect uv = new Rect(uv4.x, uv4.y, uv4.z - uv4.x, uv4.w - uv4.y);
            Vector4 padding = UnityEngine.Sprites.DataUtility.GetPadding(sprite);
            padding.x /= outer.width;
            padding.y /= outer.height;
            padding.z /= outer.width;
            padding.w /= outer.height;

            DrawSprite(tex, drawArea, padding, outer, inner, uv, color, null);
        }

        // Draw the specified Image.
        public static void DrawSprite(Texture tex, Rect drawArea, Rect outer, Rect uv, Color color)
        {
            DrawSprite(tex, drawArea, Vector4.zero, outer, outer, uv, color, null);
        }

        // Draw the specified Image.
        private static void DrawSprite(Texture tex, Rect drawArea, Vector4 padding, Rect outer, Rect inner, Rect uv, Color color, Material mat)
        {
            // Create the texture rectangle that is centered inside rect.
            Rect outerRect = drawArea;
            outerRect.width = Mathf.Abs(outer.width);
            outerRect.height = Mathf.Abs(outer.height);

            if (outerRect.width > 0f)
            {
                float f = drawArea.width / outerRect.width;
                outerRect.width *= f;
                outerRect.height *= f;
            }

            if (drawArea.height > outerRect.height)
            {
                outerRect.y += (drawArea.height - outerRect.height) * 0.5f;
            }
            else if (outerRect.height > drawArea.height)
            {
                float f = drawArea.height / outerRect.height;
                outerRect.width *= f;
                outerRect.height *= f;
            }

            if (drawArea.width > outerRect.width)
                outerRect.x += (drawArea.width - outerRect.width) * 0.5f;

            // Draw the background
            //EditorGUI.DrawTextureTransparent(outerRect, null, ScaleMode.ScaleToFit, outer.width / outer.height);
            
            // Draw the Image
            GUI.color = color;
            
            Rect paddedTexArea = new Rect(
                    outerRect.x + outerRect.width * padding.x,
                    outerRect.y + outerRect.height * padding.w,
                    outerRect.width - (outerRect.width * (padding.z + padding.x)),
                    outerRect.height - (outerRect.height * (padding.w + padding.y))
                    );

            if (mat == null)
            {
                //GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
                 GUI.DrawTextureWithTexCoords(paddedTexArea, tex, uv, true);
                //GL.sRGBWrite = true;
            }
            else
            {
                // NOTE: There is an issue in Unity that prevents it from clipping the drawn preview
                // using BeginGroup/EndGroup, and there is no way to specify a UV rect...
                EditorGUI.DrawPreviewTexture(paddedTexArea, tex, mat);
            }
            
            // Draw the border indicator lines
            GUI.BeginGroup(outerRect);
            {
                tex = contrastTexture;
                GUI.color = Color.white;

                if (inner.xMin != outer.xMin)
                {
                    float x = (inner.xMin - outer.xMin) / outer.width * outerRect.width - 1;
                    DrawTiledTexture(new Rect(x, 0f, 1f, outerRect.height), tex);
                }

                if (inner.xMax != outer.xMax)
                {
                    float x = (inner.xMax - outer.xMin) / outer.width * outerRect.width - 1;
                    DrawTiledTexture(new Rect(x, 0f, 1f, outerRect.height), tex);
                }

                if (inner.yMin != outer.yMin)
                {
                    // GUI.DrawTexture is top-left based rather than bottom-left
                    float y = (inner.yMin - outer.yMin) / outer.height * outerRect.height - 1;
                    DrawTiledTexture(new Rect(0f, outerRect.height - y, outerRect.width, 1f), tex);
                }

                if (inner.yMax != outer.yMax)
                {
                    float y = (inner.yMax - outer.yMin) / outer.height * outerRect.height - 1;
                    DrawTiledTexture(new Rect(0f, outerRect.height - y, outerRect.width, 1f), tex);
                }
            }
            

            GUI.EndGroup();
        }
    }
}

namespace Hukiry
{
	internal class SpriteDrawUtility
	{
		private static Texture2D s_ContrastTex;

		private static Texture2D contrastTexture
		{
			get
			{
				if (SpriteDrawUtility.s_ContrastTex == null)
				{
					SpriteDrawUtility.s_ContrastTex = SpriteDrawUtility.CreateCheckerTex(new Color(0f, 0f, 0f, 0.5f), new Color(1f, 1f, 1f, 0.5f));
				}
				return SpriteDrawUtility.s_ContrastTex;
			}
		}

		private static Texture2D CreateCheckerTex(Color c0, Color c1)
		{
			Texture2D texture2D = new Texture2D(16, 16);
			texture2D.name = "[Generated] Checker Texture";
			texture2D.hideFlags = HideFlags.DontSave;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					texture2D.SetPixel(j, i, c1);
				}
			}
			for (int k = 8; k < 16; k++)
			{
				for (int l = 0; l < 8; l++)
				{
					texture2D.SetPixel(l, k, c0);
				}
			}
			for (int m = 0; m < 8; m++)
			{
				for (int n = 8; n < 16; n++)
				{
					texture2D.SetPixel(n, m, c0);
				}
			}
			for (int num = 8; num < 16; num++)
			{
				for (int num2 = 8; num2 < 16; num2++)
				{
					texture2D.SetPixel(num2, num, c1);
				}
			}
			texture2D.Apply();
			texture2D.filterMode = FilterMode.Point;
			return texture2D;
		}

		private static Texture2D CreateGradientTex()
		{
			Texture2D texture2D = new Texture2D(1, 16);
			texture2D.name = "[Generated] Gradient Texture";
			texture2D.hideFlags = HideFlags.DontSave;
			Color a = new Color(1f, 1f, 1f, 0f);
			Color b = new Color(1f, 1f, 1f, 0.4f);
			for (int i = 0; i < 16; i++)
			{
				float num = Mathf.Abs((float)i / 15f * 2f - 1f);
				num *= num;
				texture2D.SetPixel(0, i, Color.Lerp(a, b, num));
			}
			texture2D.Apply();
			texture2D.filterMode = FilterMode.Bilinear;
			return texture2D;
		}

		private static void DrawTiledTexture(Rect rect, Texture tex)
		{
			float width = rect.width / (float)tex.width;
			float height = rect.height / (float)tex.height;
			Rect texCoords = new Rect(0f, 0f, width, height);
			TextureWrapMode wrapMode = tex.wrapMode;
			tex.wrapMode = TextureWrapMode.Repeat;
			GUI.DrawTextureWithTexCoords(rect, tex, texCoords);
			tex.wrapMode = wrapMode;
		}

		public static void DrawSprite(Sprite sprite, Rect drawArea, Color color)
		{
			if (!(sprite == null))
			{
				Texture2D texture = sprite.texture;
				if (!(texture == null))
				{
					Rect rect = sprite.rect;
					Rect inner = rect;
					inner.xMin += sprite.border.x;
					inner.yMin += sprite.border.y;
					inner.xMax -= sprite.border.z;
					inner.yMax -= sprite.border.w;
					Vector4 outerUV = UnityEngine.Sprites.DataUtility.GetInnerUV(sprite);
					Rect uv = new Rect(outerUV.x, outerUV.y, outerUV.z - outerUV.x, outerUV.w - outerUV.y);
					Vector4 padding = UnityEngine.Sprites.DataUtility.GetInnerUV(sprite);
					padding.x /= rect.width;
					padding.y /= rect.height;
					padding.z /= rect.width;
					padding.w /= rect.height;
					SpriteDrawUtility.DrawSprite(texture, drawArea, padding, rect, inner, uv, color, null);
				}
			}
		}

		public static void DrawSprite(Texture tex, Rect drawArea, Rect outer, Rect uv, Color color)
		{
			SpriteDrawUtility.DrawSprite(tex, drawArea, Vector4.zero, outer, outer, uv, color, null);
		}

		private static void DrawSprite(Texture tex, Rect drawArea, Vector4 padding, Rect outer, Rect inner, Rect uv, Color color, Material mat)
		{
			Rect position = drawArea;
			position.width = Mathf.Abs(outer.width);
			position.height = Mathf.Abs(outer.height);
			if (position.width > 0f)
			{
				float num = drawArea.width / position.width;
				position.width *= num;
				position.height *= num;
			}
			if (drawArea.height > position.height)
			{
				position.y += (drawArea.height - position.height) * 0.5f;
			}
			else if (position.height > drawArea.height)
			{
				float num2 = drawArea.height / position.height;
				position.width *= num2;
				position.height *= num2;
			}
			if (drawArea.width > position.width)
			{
				position.x += (drawArea.width - position.width) * 0.5f;
			}
			EditorGUI.DrawTextureTransparent(position, null, ScaleMode.ScaleToFit, outer.width / outer.height);
			GUI.color = color;
			Rect position2 = new Rect(position.x + position.width * padding.x, position.y + position.height * padding.w, position.width - position.width * (padding.z + padding.x), position.height - position.height * (padding.w + padding.y));
			if (mat == null)
			{
				GUI.DrawTextureWithTexCoords(position2, tex, uv, true);
			}
			else
			{
				EditorGUI.DrawPreviewTexture(position2, tex, mat);
			}
			GUI.BeginGroup(position);
			tex = SpriteDrawUtility.contrastTexture;
			GUI.color = Color.white;
			if (inner.xMin != outer.xMin)
			{
				float x = (inner.xMin - outer.xMin) / outer.width * position.width - 1f;
				SpriteDrawUtility.DrawTiledTexture(new Rect(x, 0f, 1f, position.height), tex);
			}
			if (inner.xMax != outer.xMax)
			{
				float x2 = (inner.xMax - outer.xMin) / outer.width * position.width - 1f;
				SpriteDrawUtility.DrawTiledTexture(new Rect(x2, 0f, 1f, position.height), tex);
			}
			if (inner.yMin != outer.yMin)
			{
				float num3 = (inner.yMin - outer.yMin) / outer.height * position.height - 1f;
				SpriteDrawUtility.DrawTiledTexture(new Rect(0f, position.height - num3, position.width, 1f), tex);
			}
			if (inner.yMax != outer.yMax)
			{
				float num4 = (inner.yMax - outer.yMin) / outer.height * position.height - 1f;
				SpriteDrawUtility.DrawTiledTexture(new Rect(0f, position.height - num4, position.width, 1f), tex);
			}
			GUI.EndGroup();
		}
	}


	/// <summary>
	///   <para>Helper utilities for accessing Sprite data.</para>
	/// </summary>
	public sealed class DataUtility
	{
		/// <summary>
		///   <para>Inner UV's of the Sprite.</para>
		/// </summary>
		/// <param name="sprite"></param>
		public static Vector4 GetInnerUV(Sprite sprite)
		{
			return  GetVector4(sprite, "GetInnerUVs");
		}

		/// <summary>
		///   <para>Outer UV's of the Sprite.</para>
		/// </summary>
		/// <param name="sprite"></param>
		public static Vector4 GetOuterUV(Sprite sprite)
		{
			return GetVector4(sprite, "GetOuterUVs");
		}

		/// <summary>
		///   <para>Return the padding on the sprite.</para>
		/// </summary>
		/// <param name="sprite"></param>
		public static Vector4 GetPadding(Sprite sprite)
		{
			return GetVector4(sprite, "GetPadding");
		}

		private static Vector4 GetVector4(Sprite sprite,string name)
		{
			var ty=sprite.GetType();
			return (Vector4)ty.InvokeMember(name, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, sprite, null);
		}
		/// <summary>
		///   <para>Minimum width and height of the Sprite.</para>
		/// </summary>
		/// <param name="sprite"></param>
		public static Vector2 GetMinSize(Sprite sprite)
		{
			Vector2 result;
			result.x = sprite.border.x + sprite.border.z;
			result.y = sprite.border.y + sprite.border.w;
			return result;
		}
	}
}