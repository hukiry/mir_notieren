using UnityEngine;
namespace Hukiry.UI
{
    // 人物移动脚本
    [RequireComponent(typeof(Joystick))]
    public class JoystickInput : MonoBehaviour
    {
        public static JoystickInput ins;
        private Joystick m_Joystick = null;
        [SerializeField]
        float speed = 5;

        [SerializeField]
        private bool is2D = false;

        [SerializeField]
        [Tooltip("Y轴正方向瞄准")]
        private bool isYAxis;

        private Transform player;
        private void Awake()
        {
            ins = this;

            m_Joystick = this.GetComponent<Joystick>();
            m_Joystick.onJoystick = this.JoyStickHandle;
        }

        public void SetPlayer(Transform player, float speed = 5)
        {
            this.player = player;
            this.speed = speed;
        }

        public void SetPlayer(MonoBehaviour player, float speed = 5)
        {
            this.player = player.transform;
            this.speed = speed;
        }


        void JoyStickHandle(Vector2 v)
        {
            if (v.magnitude != 0)
            {
                if (is2D)
                {
                    // Translate移动
                    this.JoyStickMove(v.x * Time.deltaTime, v.y * Time.deltaTime, 0);
                    //方向计算 也可以不用单位化：目标位置-自己的位置=方向
                    Vector3 dir = v.normalized;
                    //反正切，x/y=弧度->转角度：根据方向计算出自己与目标的角度
                    var angle = Mathf.Atan2(isYAxis ? dir.x : dir.y, isYAxis ? dir.y : dir.x) * Mathf.Rad2Deg;
                    //环绕z轴旋转：自己环绕z轴旋转angle
                    this.JoyStickRotation(Quaternion.AngleAxis(isYAxis ? -angle : angle, Vector3.forward));
                }
                else
                {
                    this.JoyStickMove(v.x * Time.deltaTime, 0, v.y * Time.deltaTime);// Translate移动
                    this.JoyStickRotation(Quaternion.LookRotation(new Vector3(v.x, 0, v.y)));
                }
            }
        }

        void JoyStickMove(float x, float y, float z)
        {
            this.player.Translate(x * speed, y * speed, z * speed, Space.World);
        }

        void JoyStickRotation(Quaternion quaternion)
        {
            this.player.rotation = quaternion;
        }
    }
}