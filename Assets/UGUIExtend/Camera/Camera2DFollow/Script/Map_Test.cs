using UnityEngine;
namespace Hukiry
{
    /// <summary>
    /// author:胡雄坤
    /// 测试提供
    /// </summary>
    public class Map_Test : MonoBehaviour
    {
        private FollowCameraCtl m_coreCameraCtl;
        private Vector3 m_targetpos;
        private bool m_isMove;
        [SerializeField]
        [Header("速度控制")]
        [Range(1, 10)]
        private float m_speed = 3;
        // Start is called before the first frame update

        private LineRenderer lineRenderer;
        private Vector2 velocity;
        void Start()
        {
            this.m_coreCameraCtl = GetComponent<FollowCameraCtl>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
            lineRenderer.material.SetColor("_Color", Color.yellow);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.m_targetpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.m_isMove = true;
                var pos = this.m_coreCameraCtl.transform.position;
                var dir = (this.m_targetpos - pos).normalized;//单位方向
                var rad = Mathf.Atan2(dir.y, dir.x);//计算方向上的弧度
                velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;//根据弧度计算单位速度
            }

            if (this.m_isMove)
            {
                var pos = this.m_coreCameraCtl.transform.position;
                if (Vector2.Distance(this.m_targetpos, pos) >= 0.1f)
                {
                    //此处为平滑移动测试，速度可以自定义
                    Vector3 offset = velocity * Time.deltaTime * this.m_speed;
                    this.m_coreCameraCtl.MoveRole(offset);
                }
                else
                {
                    var dir = (this.m_targetpos - pos).normalized;
                    this.m_coreCameraCtl.MoveRole(dir);
                    this.m_isMove = false;
                }

                Debug.DrawLine(pos, this.m_targetpos, Color.yellow);
                lineRenderer.positionCount = 2;
                lineRenderer.SetPositions(new Vector3[] {pos, this.m_targetpos });
            }
        }
    }
}