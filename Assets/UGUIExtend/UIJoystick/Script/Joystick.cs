namespace Hukiry.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// UI摇杆：支持屏幕空间和屏幕相机空间
    /// <list>BackGround  位置-自定义</list>
    /// <list>BackGround/Handle 位置zero</list>
    /// <list>BackGround/Direction 位置zero</list>
    /// </summary>
    [RequireComponent(typeof(RectTransform), typeof(UIBoxCollider))]
    public class Joystick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public float maxRadius = 100; //Handle 移动最大半径
        [SerializeField]
        Direction activatedAxis = (Direction)(-1);// 选择激活的轴向
        [SerializeField]
        bool showDirection = true;

        public System.Action<Vector2> onJoystick = null;
        private RectTransform backGround, handle, direction; // 摇杆背景、摇杆手柄、方向指引
        private Vector2 joystickValue = Vector2.zero; // 摇杆拖动量
        private Vector3 backGroundOriginLocalPostion,backGroundPressedPostion; //Background 的位置

        [System.Flags]
        public enum Direction
        {
            Horizontal = 1 << 0,
            Vertical = 1 << 1
        }
        private void Awake()
        {
            this.ResetLayout();
            backGroundOriginLocalPostion = backGround.localPosition;// 摇杆背景的本地坐标
        }

        public void ResetLayout()
        {
            backGround = GetTransform("Background", this.transform);         // 摇杆背景
            handle = GetTransform("Handle", backGround);      // 摇杆手柄
            direction = GetTransform("Direction", backGround);// 反向指引
            direction?.gameObject.SetActive(false);                  // 指引默认隐藏
        }

        private RectTransform GetTransform(string name, Transform parent)
        {
            var tf = parent.Find(name) as RectTransform;
            if (tf == null)
            {
                tf = new GameObject(name, typeof(CanvasRenderer), typeof(RectTransform)).GetComponent<RectTransform>();
                tf.SetParent(parent, false);
                tf.anchoredPosition = Vector2.zero;
                tf.localRotation = Quaternion.identity;
                tf.localScale = Vector3.one;

                if (name.Equals("Direction"))
                {
                    var tf1 = new GameObject("icon", typeof(CanvasRenderer), typeof(Image)).GetComponent<RectTransform>();
                    tf1.SetParent(tf, false);
                    tf1.anchoredPosition = Vector2.zero;
                    tf1.localRotation = Quaternion.identity;
                    tf1.localScale = Vector3.one;
                }
            }
            return tf;
        }

        void Update()
        {
            joystickValue.x = handle.anchoredPosition.x / maxRadius;
            joystickValue.y = handle.anchoredPosition.y / maxRadius;
            onJoystick?.Invoke(joystickValue);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            float z = backGround.position.z;
            //坐标转换到屏幕
            if (eventData.pressEventCamera)
                z = eventData.pressEventCamera.WorldToScreenPoint(backGround.position).z;
            // 摇杆按压位置
            backGroundPressedPostion = new Vector3()
            {
                x = eventData.position.x,
                y = eventData.position.y,
                z = z
            };

            //摇杆背景位置
            if (eventData.pressEventCamera)
                backGround.position = eventData.pressEventCamera.ScreenToWorldPoint(backGroundPressedPostion);
            else
                backGround.position = backGroundPressedPostion;
        }

        // 当鼠标拖拽时
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            Vector2 direction = eventData.position - (Vector2)backGroundPressedPostion;// 得到方位盘中心指向光标的向量
            float radius = Mathf.Clamp(Vector3.Magnitude(direction), 0, maxRadius);// 获取并锁定向量的长度 以控制 Handle 半径
            Vector2 localPosition = new Vector2()
            {
                x = (0 != (activatedAxis & Direction.Horizontal)) ? (direction.normalized * radius).x : 0,// 确认是否激活水平轴向
                y = (0 != (activatedAxis & Direction.Vertical)) ? (direction.normalized * radius).y : 0   // 确认是否激活竖直轴向，激活就搞事情
            };

            // 更新 Handle 位置
            handle.localPosition = localPosition;
            UpdateDirectionArrow(localPosition);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            direction.gameObject.SetActive(false);
            backGround.localPosition = backGroundOriginLocalPostion;
            //还原位置
            handle.localPosition = Vector3.zero;
        }

        // 更新指向器的朝向
        private void UpdateDirectionArrow(Vector2 position)
        {
            if (showDirection && position.magnitude != 0)
            {
                direction.gameObject.SetActive(true);// 显示
                direction.localEulerAngles = new Vector3(0, 0, Vector2.Angle(Vector2.right, position) * (position.y > 0 ? 1 : -1));// 设置本地角度
            }
            else if(direction.gameObject.activeSelf)
            {
                direction.gameObject.SetActive(false);
            }
        }
    }
} 
