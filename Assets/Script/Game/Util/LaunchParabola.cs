using UnityEngine;
using System.Collections;

namespace Hukiry {
	/// <summary>
	/// 抛物线脚本
	/// </summary>
	public class LaunchParabola : MonoBehaviour {
		public float altitude = 22;//顶垂线

        public bool isHide = false;//是否由外部控制隐藏

        private Vector3 startPos = Vector3.zero; //起始点
		private Vector3 endPos = Vector3.zero;	//结束点

		private float xSpeed;
		private float ySpeed;
		private float t0 = -1;
		private float g = -1;
		private float angel;

		private float absXSpeed;
		private float absYSpeed;

		private bool isCanPlay = false;

		public void StartLaunch(Vector3 startPos, Vector3 endPos) {
			this.gameObject.SetActive(true);
			transform.localPosition = startPos;
			this.startPos = startPos;
			this.endPos = endPos;

			t0 = 0;
			xSpeed = (endPos.x - startPos.x) / altitude;
			ySpeed = (endPos.y - startPos.y - (g * altitude * (altitude / 2))) / altitude;

			absXSpeed = Mathf.Abs(xSpeed);
			absYSpeed = Mathf.Abs(ySpeed);

			isCanPlay = true;
			MoveArrow();
		}

		private void FixedUpdate() {
			if (!isCanPlay && gameObject.activeSelf && !isHide) {
                this.gameObject.SetActive(false);

            }
			Vector3 pos = endPos - transform.localPosition;
			if (Mathf.Abs(pos.x) < absXSpeed * 0.5f && Mathf.Abs(pos.y) < absYSpeed * 0.5f) {
				isCanPlay = false;//在这里不能直接隐藏，要晚一帧，否则就感觉箭矢未到就消失了
			} else {
				MoveArrow();
			}
#if UNITY_EDITOR
            if (Input.GetMouseButton(0)) {
				StartLaunch(startPos, endPos);
			}
#endif
        }

		private void MoveArrow() {
			t0++;
			float x = startPos.x + (t0 * xSpeed);
			float y = startPos.y + (t0 * ySpeed) + (g * t0 * t0 / 2);
			transform.localPosition = new Vector3(x, y, 0);

			t0++;
			float sx = startPos.x + t0 * xSpeed;
			float sy = startPos.y + t0 * ySpeed + ((g * t0) * t0) / 2;
			t0--;

			float dx = sx - x;
			float dy = sy - y;
			angel = Mathf.Atan2(dy, dx);
			transform.localRotation = Quaternion.Euler(0, 0, angel * 180 / Mathf.PI + 180);
		}
	}
}