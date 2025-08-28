using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hukiry
{
    /// <summary>
    /// 游戏中使用
    /// </summary>
    public class HukiryUtil
	{
		/// <summary>
		/// 获取枚举特性字符串
		/// </summary>
		/// <typeparam name="T">枚举类型</typeparam>
		/// <typeparam name="K">特性类型，必须声明 name字段</typeparam>
		public static void GetEnumStrings<T, K>() where T : Enum where K : Attribute
		{

			Dictionary<T, string> dic = new Dictionary<T, string>();
			string[] array = typeof(T).GetEnumNames();
			foreach (var item in array)
			{
				T t = (T)Enum.Parse(typeof(T), item);

				var objs = t.GetType().GetField(t.ToString()).GetCustomAttributes(typeof(K), false);
				if (objs != null && objs.Length > 0)
				{
					K fn = objs[0] as K;
					try
					{
						string name = (string)fn.GetType().GetField("name").GetValue(fn);
						dic[t] = name;
					}
					catch
					{
						dic[t] = t.ToString();
					}
				}
				else
				{
					dic[t] = t.ToString();
				}
			}
		}

		/// <summary>
		/// 正多边形绘制
		/// </summary>
		/// <param name="angle">绘制锐角角度</param>
		/// <param name="scale">多边形缩放</param>
		/// <returns></returns>
		public static Vector2[] GetPolygonPoint(float angle, float scale = 1)
		{
			Vector2[] points = new Vector2[4];
			float planeY = scale * Mathf.Sin(angle * Mathf.Deg2Rad);
			float planeX = scale * Mathf.Cos(angle * Mathf.Deg2Rad);
			points[0] = new Vector2(-planeX, 0);
			points[1] = new Vector2(0, -planeY);
			points[2] = new Vector2(planeX, 0);
			points[3] = new Vector2(0, planeY);

			return points;
		}

		/// <summary>
		/// 根据斜边，一角获取，xy
		/// </summary>
		/// <param name="angle">y 对应的角度</param>
		/// <param name="edgeLen">斜边长度</param>
		/// <returns>(x,y)</returns>
		public static (float x, float y) GetTrangleXY(float angle, float edgeLen)
		{
			float planeY = edgeLen * Mathf.Sin(angle * Mathf.Deg2Rad);
			float planeX = edgeLen * Mathf.Cos(angle * Mathf.Deg2Rad);
			return (planeX, planeY);
		}

		public static Vector2[] RotationalCoordinate(float originAngle, float rotateAngle, Vector2[] point)
		{
			float tagetAngle = originAngle - rotateAngle;
			if (point.Length < 2)
			{
				Debug.LogError("HukiryUtil.RotationalCoordinate 参数异常point");
				return null;
			}
			float edge = Mathf.Abs(Vector2.Distance(point[0], point[1]));
			var (x, y) = GetTrangleXY(tagetAngle, edge);
			float offsetX = Mathf.Abs(point[0].x) - x;//-
			float offsetY = Mathf.Abs(point[0].y) - y;//+

			int length = point.Length;
			Vector2[] array = new Vector2[length];

			for (int i = 0; i < length; i++)
			{
				//向量a,b的夹角,得到的值为弧度，我们将其转换为角度，便于查看！  
				float angle = Mathf.Acos(Vector3.Dot(Vector2.left.normalized, point[i].normalized)) * Mathf.Rad2Deg;
				if (angle < 90)

					array[i] = new Vector2(point[i].x + offsetX, point[i].y - offsetY);
				else
					array[i] = new Vector2(point[i].x + offsetX, point[i].y + offsetY);
			}
			return array;
		}

		public static void InitGameInfo()
		{
			Screen.orientation = ScreenOrientation.AutoRotation;
			Screen.autorotateToLandscapeLeft = false;//横屏
			Screen.autorotateToLandscapeRight = false;
			Screen.fullScreen = true;//填充屏幕
			Screen.autorotateToPortrait = true;//竖屏
			Screen.autorotateToPortraitUpsideDown = false;//竖屏

			System.Net.ServicePointManager.DefaultConnectionLimit = 50; //解决HttpWebRequest超时不起作用问题
			Screen.sleepTimeout = SleepTimeout.NeverSleep;  //屏幕永久亮
			Input.multiTouchEnabled = true;                 //开启多点触摸
			Application.targetFrameRate = 60;               //手机普遍最高45帧
			Application.runInBackground = true;             //允许后台运行


			//推送服务
//#if UNITY_IPHONE && (UNITY_2018 || UNITY_2017 || UNITY_5_6)
//		UnityEngine.iOS.NotificationServices.RegisterForNotifications(
//			UnityEngine.iOS.NotificationType.Alert |
//			UnityEngine.iOS.NotificationType.Badge |
//			UnityEngine.iOS.NotificationType.Sound);
//#endif
		}

		/// <summary>
		/// 精灵转贴图
		/// </summary>
		public static Texture2D SpriteToTexture(Sprite sprite)
		{
			Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
			tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
			(int)sprite.rect.width, (int)sprite.rect.height));
			tex.Apply();
			return tex;
		}

		/// <summary>
		/// 贴图转精灵
		/// </summary>
		public static Sprite TextureToSprite(Texture2D texOut)
		{
			Rect rect = new Rect(0, 0, texOut.width, texOut.height);
			Vector2 pivot = Vector2.one / 2;
			return Sprite.Create(texOut, rect, pivot, 100, 1);
		}
		/// <summary>
		/// 贴图转换灰色贴图
		/// </summary>
		public static Texture2D TextureToGrayTexture(Texture2D inputTexture)
		{
			Color a = new Color(0.3f, 0.56f, 0.11f, 1);

			int w = inputTexture.width;
			int h = inputTexture.height;
			Texture2D texOut = new Texture2D(w, h);

			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					Color b = inputTexture.GetPixel(i, j);
					float gray = a.r * b.r + a.g * b.g + a.b * b.b;
					texOut.SetPixel(i, j, new Color(gray, gray, gray, b.a));
				}
			}
			texOut.name = inputTexture.name;
			texOut.Apply();
			return texOut;
		}

		/// <summary>
		/// 贴图转换灰色精灵
		/// </summary>
		public static Sprite TextureToGraySprite(Texture2D inputTexture)
		{
			Color a = new Color(0.3f, 0.56f, 0.11f, 1);

			int w = inputTexture.width;
			int h = inputTexture.height;
			Texture2D texOut = new Texture2D(w, h);

			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					Color b = inputTexture.GetPixel(i, j);
					float gray = a.r * b.r + a.g * b.g + a.b * b.b;
					texOut.SetPixel(i, j, new Color(gray, gray, gray, b.a));
				}
			}
			texOut.name = inputTexture.name;
			texOut.Apply();

			Rect rect = new Rect(0, 0, texOut.width, texOut.height);
			Vector2 pivot = Vector2.one / 2;
			return Sprite.Create(texOut, rect, pivot, 100);
		}

		/// <summary>
		/// 获取MD5
		/// </summary>
		/// <param name="time">时间搓</param>
		/// <param name="key">前后端定义的Key</param>
		/// <returns></returns>
		public static string GetMD5(long time, string key)
		{
			string id = SystemInfo.deviceUniqueIdentifier + time + key;
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(id);
			return GetMD5(buffer);
		}

		public static string GetMD5(string content)
		{
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
			return GetMD5(buffer);
		}
		/// <summary> 计算字节的MD5值 </summary>
		private static string GetMD5(byte[] buffer)
		{
			var mMD5Provider = System.Security.Cryptography.MD5.Create();
			byte[] hash = mMD5Provider.ComputeHash(buffer);
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < hash.Length; i++)
				sb.Append(hash[i].ToString("x2"));
			return sb.ToString();
		}

		public static float GetLocalTimestamp()
		{
			float t = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / TimeSpan.TicksPerSecond;
			return t;
		}

		/// <summary>
		/// 获取时间搓
		/// </summary>
		/// <returns></returns>
		public static long GetTimestamp()
		{
			TimeSpan ts = new TimeSpan(DateTime.Now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks);
			long t = ts.Ticks / TimeSpan.TicksPerSecond;
			return t;
		}

		/// <summary>
		/// 获取时间搓毫秒
		/// </summary>
		public static long GetTimeMilliseconds()
		{
			TimeSpan ts = new TimeSpan(DateTime.Now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks);
			return (long)ts.TotalMilliseconds;
		}

		public static string ClassAttributeToString<T>()
		{
			var type = typeof(T);
			var objs = type.GetCustomAttributes(typeof(FieldNameAttribute), false);
			if (objs != null && objs.Length > 0)
			{
				FieldNameAttribute fn = objs[0] as FieldNameAttribute;
				return fn.name;
			}

			return null;
		}

		public static string[] ToEnumStringArray<T>() where T : System.Enum
		{
			var names = typeof(T).GetEnumNames();
			int len = names.Length;

			string[] array = new string[len];
			for (int i = 0; i < len; i++)
			{
				var obj = (T)Enum.Parse(typeof(T), names[i]);
				array[i] = EnumRefectToString<T, FieldNameAttribute>(obj);
			}

			return array;

		}
		//[LuaInterface.NoToLua]
		public static string[] ToEnumStringArray(Type type)
		{
			var names = type.GetEnumNames();
			int len = names.Length;
			string[] array = new string[len];
			for (int i = 0; i < len; i++)
			{
				var obj = Enum.Parse(type, names[i]);
				var objs = obj.GetType().GetField(obj.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (objs != null && objs.Length > 0)
				{
					DescriptionAttribute fn = objs[0] as DescriptionAttribute;
					array[i] = fn.name;
				}
				else
				{
					array[i] = names[i];
				}
			}
			return array;
		}

		//[LuaInterface.NoToLua]
		public static string EnumRefectToString<T, K>(T t) where T : System.Enum where K : System.Attribute
		{
			var objs = t.GetType().GetField(t.ToString()).GetCustomAttributes(typeof(K), false);
			if (objs != null && objs.Length > 0)
			{
				K fn = objs[0] as K;
				return fn.GetType().GetField("name")?.GetValue(fn)?.ToString();
			}
			return null;
		}

		public static int[] ToEnumIntArray<T>() where T : System.Enum
		{
			var Length = typeof(T).GetEnumNames().Length;
			int[] array = new int[Length];
			for (int i = 0; i < Length; i++)
			{
				array[i] = i;
			}
			return array;

		}

		/// <summary>
		/// 图像缩放
		/// </summary>
		/// <param name="originalTexture">原始图片</param>
		/// <param name="scaleFactor">缩放系数=缩放的边长/原始图片的边长</param>
		/// <returns>返回缩放后的图片</returns>
		public static Texture2D ScaleTextureBilinear(Texture2D originalTexture, float scaleFactor)
		{
			Texture2D newTexture = new Texture2D(Mathf.CeilToInt(originalTexture.width * scaleFactor), Mathf.CeilToInt(originalTexture.height * scaleFactor));
			float scale = 1.0f / scaleFactor;
			int maxX = originalTexture.width - 1;
			int maxY = originalTexture.height - 1;
			for (int y = 0; y < newTexture.height; y++)
			{
				for (int x = 0; x < newTexture.width; x++)
				{
					//像素坐标缩放
					float targetX = x * scale;
					float targetY = y * scale;
					int x1 = Mathf.Min(maxX, Mathf.FloorToInt(targetX));
					int y1 = Mathf.Min(maxY, Mathf.FloorToInt(targetY));
					int x2 = Mathf.Min(maxX, x1 + 1);
					int y2 = Mathf.Min(maxY, y1 + 1);

					float u = targetX - x1;
					float v = targetY - y1;
					float w1 = (1 - u) * (1 - v);
					float w2 = u * (1 - v);
					float w3 = (1 - u) * v;
					float w4 = u * v;
					//纹理颜色缩放
					Color color1 = originalTexture.GetPixel(x1, y1);
					Color color2 = originalTexture.GetPixel(x2, y1);
					Color color3 = originalTexture.GetPixel(x1, y2);
					Color color4 = originalTexture.GetPixel(x2, y2);
					Color color = new Color(Mathf.Clamp01(color1.r * w1 + color2.r * w2 + color3.r * w3 + color4.r * w4),
						Mathf.Clamp01(color1.g * w1 + color2.g * w2 + color3.g * w3 + color4.g * w4),
						Mathf.Clamp01(color1.b * w1 + color2.b * w2 + color3.b * w3 + color4.b * w4),
						Mathf.Clamp01(color1.a * w1 + color2.a * w2 + color3.a * w3 + color4.a * w4)
						);
					newTexture.SetPixel(x, y, color);
				}
			}

			return newTexture;
		}

		/// <summary>
		/// 文件流转换为精灵
		/// </summary>
		public Sprite GetSprite(string filePath, string name, int W, int H)
		{
			using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read)) //自动双清
			{
				fs.Seek(0, SeekOrigin.Begin);            //设定当前流的位置
				byte[] bytes = new byte[fs.Length];      //创建文件长度缓冲区
				fs.Read(bytes, 0, (int)fs.Length);      //读取文件
				Texture2D texture = new Texture2D(W, H, TextureFormat.ARGB32, false);
				texture.LoadRawTextureData(bytes);
				texture.Apply();
				Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
				sprite.name = name;
				return sprite;
			}
		}

		/// <summary>
		/// 加密或解密
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static byte[] CodeByte(byte[] buffer, bool isText = false)
		{
			const int SIZE = 20;
			const int CODE_NUMBER = 2;
			int[] CODE_KEYArray = new int[SIZE/2];
			int k = 0;
            for (int i = CODE_NUMBER; i <= SIZE; i++)
            {
				if (i % CODE_NUMBER == 0)
				{
					CODE_KEYArray[k++] = i;
				}
            }

			int CODEBYTE(int i, int b)
			{
				int bm = b;
				const int CODE_KEY = 64;
				const int CODE_POS = 12;
				if (i % CODE_POS == 0)
				{
					bm ^= CODE_KEY;
				}
				return bm;
			};

			int CODEText(int i, int b)
			{
				int bm = b;
				const int CODE_POS = 2;
				if (i % CODE_POS == 0)
				{
					bm ^= CODE_KEYArray[i% CODE_KEYArray.Length];
				}
				return bm;
			};

			byte[] bufferTemp = new byte[buffer.Length];

			if (isText)
			{
				for (int i = 0; i < buffer.Length; i++)
				{
					bufferTemp[i] = (byte)CODEText(i, buffer[i]);
				}
			}
			else
			{
				for (int i = 0; i < buffer.Length; i++)
				{
					bufferTemp[i] = (byte)CODEBYTE(i, buffer[i]);
				}
			}
			return bufferTemp;
		}

		/// <summary>
		/// 将字符串转换为字节
		/// </summary>
		/// <param name="st"></param>
		/// <returns></returns>
		public static byte[] CodeByte(string st,bool isText = false)
		{
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(st);
			buffer = CodeByte(buffer, isText);
			return buffer;
		}

		/// <summary>
		/// 将字节转换为字符串
		/// </summary>
		public static string DeCodeByte(byte[] bytes, bool isText = false)
		{
			byte[] buffer = CodeByte(bytes, isText);
			string content = System.Text.Encoding.UTF8.GetString(buffer);
			return content;
		}

		/// <summary>
		/// 读取字节文件
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static byte[] ReadAllBytes(string filePath)
		{
			using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read)) //自动双清
			{
				fs.Seek(0, SeekOrigin.Begin);            //设定当前流的位置
				byte[] bytes = new byte[fs.Length];      //创建文件长度缓冲区
				fs.Read(bytes, 0, (int)fs.Length);      //读取文件
				return bytes;
			}
		}

		/// <summary>
		/// 写入字节文件
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="buffer"></param>
		public static void WriteAllBytes(string filePath, byte[] buffer)
		{
			using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write)) //自动双清
			{
				fs.Seek(0, SeekOrigin.Begin);            //设定当前流的位置
				fs.Write(buffer, 0, buffer.Length);      //读取文件
			}
		}


		/// <summary>
		/// 字符串到颜色
		/// </summary>
		/// <param name="color">#3445FF</param>
		/// <returns></returns>
		public static Color StringToColor(string color)
		{
			ColorUtility.TryParseHtmlString(color, out Color resultColor);
			return resultColor;
		}

		public static string ColorToString(Color resultColor)
		{
			return ColorUtility.ToHtmlStringRGBA(resultColor);
		}

		public static byte[] GetBytes(string postData)
		{
			return System.Text.Encoding.UTF8.GetBytes(postData);
		}

		public static int GetNetworkState()
		{
			return (int)Application.internetReachability;
		}
    }
}