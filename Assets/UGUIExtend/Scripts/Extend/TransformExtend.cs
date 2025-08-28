namespace Hukiry
{
    using DG.Tweening;
    using DG.Tweening.Core;
    using DG.Tweening.Plugins.Options;
    using Hukiry.UI;
    using UnityEngine;
    using UnityEngine.UI;

    public static class TransformExtend
	{
		/// <summary>
		/// 设置位置
		/// </summary>
		/// <param name="tf"></param>
		/// <param name="position">Vector3</param>
		/// <param name="islocal">是本地坐标</param>
		public static void SetPosition(this Transform tf, Vector3 position, bool islocal = true)
		{
			if (tf == null) return;
			if (islocal)
			{
				tf.localPosition = position;
			}
			else
			{
				tf.position = position;
			}
		}
		/// <summary>
		/// 设置位置
		/// </summary>
		/// <param name="tf"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="islocal">是本地坐标</param>
		public static void SetPosition(this Transform tf, float x, float y, float z, bool islocal = true)
		{
			if (tf == null) return;
			if (islocal)
			{
				tf.localPosition = new Vector3(x, y, z);
			}
			else
			{
				tf.position = new Vector3(x, y, z);
			}
		}
		/// <summary>
		/// 设置欧拉角
		/// </summary>
		/// <param name="tf"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public static void SetEulerAngles(this Transform tf, float x, float y, float z)
		{
			if (tf == null) return;
			tf.localEulerAngles = new Vector3(x, y, z);
		}
		/// <summary>
		/// 设置欧拉角
		/// </summary>
		/// <param name="tf"></param>
		/// <param name="eulerAngles">Vector3</param>
		public static void SetEulerAngles(this Transform tf, Vector3 eulerAngles)
		{
			if (tf == null) return;
			tf.localEulerAngles = eulerAngles;
		}
		/// <summary>
		/// 设置缩放
		/// </summary>
		/// <param name="tf"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public static void SetScale(this Transform tf, float x, float y, float z)
		{
			if (tf == null) return;
			tf.localScale = new Vector3(x, y, z);
		}

		/// <summary>
		/// 设置缩放
		/// </summary>
		/// <param name="tf"></param>
		/// <param name="scale">Vector3</param>
		public static void SetScale(this Transform tf, Vector3 scale)
		{
			if (tf == null) return;
			tf.localScale = scale;
		}

		/// <summary>
		/// 设置四元数旋转
		/// </summary>
		/// <param name="tf">Transform</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="islocal">是本地坐标</param>
		public static void SetRotation(this Transform tf, float x, float y, float z, bool islocal = true)
		{
			if (tf == null) return;
			if (islocal)
			{
				tf.localRotation = Quaternion.Euler(x, y, z);
			}
			else
			{
				tf.rotation = Quaternion.Euler(x, y, z);
			}
		}

		/// <summary>
		/// 设置四元数旋转
		/// </summary>
		/// <param name="tf"></param>
		/// <param name="rotation">Vector3</param>
		/// <param name="islocal">是本地坐标</param>
		public static void SetRotation(this Transform tf, Vector3 rotation, bool islocal = true)
		{
			if (tf == null) return;

			if (islocal)
			{
				tf.localRotation = Quaternion.Euler(rotation);
			}
			else
			{
				tf.rotation = Quaternion.Euler(rotation);
			}
		}

		public static void SetAnchoredPosition(this Transform tf, float x, float? y = null, float? z = null, bool is3D = true)
		{
			if (tf == null) return;


			if (is3D)
			{
				var pos = (tf as RectTransform).anchoredPosition3D;
				float ny = !y.HasValue ? pos.y : y.Value;
				float nz = !z.HasValue ? pos.z : z.Value;
				(tf as RectTransform).anchoredPosition3D = new Vector3(x, ny, nz);
			}
			else
			{
				var pos = (tf as RectTransform).anchoredPosition3D;
				float ny = !y.HasValue ? pos.y : y.Value;
				(tf as RectTransform).anchoredPosition = new Vector2(x, ny);
			}
		}

		public static void SetSizeDelta(this Transform tf, float x, float? y = null)
		{
			if (tf == null) return;
			var pos = (tf as RectTransform).sizeDelta;
			float ny = !y.HasValue ? pos.y : y.Value;
			(tf as RectTransform).sizeDelta = new Vector2(x, ny);
		}


		/// <summary>
		/// 环绕目标点旋转
		/// </summary>
		/// <param name="target"></param>
		/// <param name="point">目标点</param>
		/// <param name="axis">旋转方向</param>
		/// <param name="endValue">增量旋转</param>
		/// <param name="duration">旋转多长时间</param>
		/// <returns></returns>
		public static TweenerCore<float, float, FloatOptions> DORotateAround(this Transform target, Vector3 point, Vector3 axis, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.rotation.z, x => {
				target.RotateAround(point, axis, x); 
			}, endValue, duration);
			t.SetTarget(target);
			return t;
		}

		/// <summary>
		/// 自己旋转
		/// </summary>
		/// <param name="target"></param>
		/// <param name="axis">旋转方向</param>
		/// <param name="endValue">增量旋转</param>
		/// <param name="duration">旋转多长时间</param>
		public static TweenerCore<float, float, FloatOptions> DORotateAround(this Transform target, Vector3 axis, float endValue, float duration)
		{
			TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.rotation.z, x => {
				target.Rotate(axis, x);
			}, endValue, duration);
			t.SetTarget(target);
			return t;
		}

		/// <summary>
		/// 尺寸变化
		/// </summary>
		/// <param name="target"></param>
		/// <param name="endSize"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public static TweenerCore<Vector2, Vector2, VectorOptions> DOSizeDelta(this Transform target, Vector2 endSize, float duration)
		{
			TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => (target  as RectTransform).sizeDelta, sizeDelta => {
				(target as RectTransform).sizeDelta = sizeDelta;
			}, endSize, duration);
			t.SetTarget(target);
			return t;
		}

		#region 查找TF

		public static GameObject FindGameObject(this Transform transform, string namePath)
		{
			var tf = transform.Find(namePath);
			if (tf) return tf.gameObject;
			return null;
		}
		public static Transform FindTransform(this Transform transform, string namePath)=> transform.Find(namePath);
		public static AtlasImage FindAtlasImage(this Transform transform, string namePath)
		{
			var tf = transform.Find(namePath);
			if (tf) return tf.GetComponent<AtlasImage>();
			return null;
		}
		public static UIProgressbarMask FindProgressbarMask(this Transform transform, string namePath)
		{
			var tf = transform.Find(namePath);
			if (tf) return tf.GetComponent<UIProgressbarMask>();
			return null;
		}
		public static Text FindText(this Transform transform, string namePath)
		{
			var tf = transform.Find(namePath);
			if (tf) return tf.GetComponent<Text>();
			return null;
		}
		public static ScrollRect FindScrollRect(this Transform transform, string namePath)
		{
			var tf = transform.Find(namePath);
			if (tf) return tf.GetComponent<ScrollRect>();
			return null;
		}
		public static HukirySupperText FindHukirySupperText(this Transform transform, string namePath)
		{
			var tf = transform.Find(namePath);
			if (tf) return tf.GetComponent<HukirySupperText>();
			return null;
		}
		public static InputField FindInputField(this Transform transform, string namePath)
		{
			var tf = transform.Find(namePath);
			if (tf) return tf.GetComponent<InputField>();
			return null;
		}
		public static RawImage FindRawImage(this Transform transform, string namePath)
		{
			var tf = transform.Find(namePath);
			if (tf) return tf.GetComponent<RawImage>();
			return null;
		}
		public static SpriteRenderer FindSpriteRenderer(this Transform transform, string namePath)
		{
			var tf = transform.Find(namePath);
			if (tf) return tf.GetComponent<SpriteRenderer>();
			return null;
		}
		public static TextMesh FindTextMesh(this Transform transform, string namePath)
		{
			var tf = transform.Find(namePath);
			if (tf) return tf.GetComponent<TextMesh>();
			return null;
		}

		public static AtlasImage FindAtlasImage(this Transform transform) => transform.GetComponent<AtlasImage>();
		public static UIProgressbarMask FindProgressbarMask(this Transform transform)=> transform.GetComponent<UIProgressbarMask>();
		public static Text FindText(this Transform transform) => transform.GetComponent<Text>();
		public static ScrollRect FindScrollRect(this Transform transform)=> transform.GetComponent<ScrollRect>();
		public static HukirySupperText FindHukirySupperText(this Transform transform) => transform.GetComponent<HukirySupperText>();
		public static InputField FindInputField(this Transform transform)=> transform.GetComponent<InputField>();
		public static RawImage FindRawImage(this Transform transform)=> transform.GetComponent<RawImage>();
		public static SpriteRenderer FindSpriteRenderer(this Transform transform)=> transform.GetComponent<SpriteRenderer>();
		public static TextMesh FindTextMesh(this Transform transform)=> transform.GetComponent<TextMesh>();
		#endregion
	}
}
