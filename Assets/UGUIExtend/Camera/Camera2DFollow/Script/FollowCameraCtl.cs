using UnityEngine;
using UnityEngine.UI;

namespace Hukiry
{
    /// <summary>
    ///  挂载到角色上：头UI角色子节点下，可以改成动态加载
    ///  注意:相机和角色Objects不能做为父节点的关系存在 
    ///  Author:胡雄坤
    /// </summary>
    public class FollowCameraCtl : MonoBehaviour
    {

        [Header("世界地图的尺寸大小，以坐标原心点绘制,自定义")]
        [SerializeField] public float mapWidth;
        [SerializeField] public float mapHeight;
        [Header("跟随头顶UI,自定义")] [SerializeField] private GameObject childTopUI;


        //角色移动
        float visibleWidth, visibleHight;//可见屏幕宽和高
        float scaleWidth, scaleHeight;
        public Camera followCamera;
        [Header("==============启动缩放部分===================")]
        [SerializeField] private bool isEnableScale = false;
        [SerializeField] private float scaleSpeed = 3;
        private float defaultScaleSize;
        private float minScale, maxScale, curScale;
        [SerializeField] private float maxDelayScale=8, curDelayScale=1;

        // Start is called before the first frame update
        void Start()
        {
            //渲染世界UI画布：必须
            Canvas canvas = childTopUI.GetComponent<Canvas>() ?? childTopUI.AddComponent<Canvas>();
            //用于不同屏幕等比缩放的：必须
            CanvasScaler canvasScaler = childTopUI.GetComponent<CanvasScaler>() ?? childTopUI.AddComponent<CanvasScaler>();
            //不需要世界UI的射线检查（提供世界UI点击），注释掉
            GraphicRaycaster graphics = childTopUI.GetComponent<GraphicRaycaster>() ?? childTopUI.AddComponent<GraphicRaycaster>();

            //这里以原心点为坐标来计算的
            this.visibleWidth = mapWidth / 2F - UnityEngine.Screen.width / 200F;
            this.visibleHight = mapHeight / 2F - UnityEngine.Screen.height / 200F;
            //默认缩放值
            this.defaultScaleSize = UnityEngine.Screen.height / 200F;

            //这里已竖屏分辨率来计算的。
            followCamera.orthographicSize = UnityEngine.Screen.height / 200F;

            //设置缩放大小范围
            this.minScale = this.defaultScaleSize;//最小缩放
            this.maxScale = this.minScale + maxDelayScale;//最大缩放
            this.curScale = this.defaultScaleSize + curDelayScale;//当前缩放值
        }

        private void Update()
        {
            if (isEnableScale)
            {
                var axisScroll = Input.GetAxis("Mouse ScrollWheel");
                if (axisScroll != 0)
                {
                    this.curScale = this.curScale - axisScroll * this.scaleSpeed;
                    this.curScale = Mathf.Clamp(this.curScale, this.minScale, this.maxScale);
                    this.ScaleCamera(this.curScale);
                }
            }
   
        }

        /// <summary>
        /// 控制角色移动，相机位置关联主角色的位置
        /// </summary>
        /// <param name="offset">移动参数：自定义</param>
        public void MoveRole(Vector3 offset)
        {
            if (followCamera)
            {
                //角色移动后的位置 var pos = this.transform.position + offset;
                var pos = this.transform.position + offset;
                //校正相机
                this.CheckCamera(offset, pos);
                pos.z = -1;
                //角色位置由用户控制移动
                this.transform.position = pos;
            }
        }

        /// <summary>
        /// 控制相机移动
        /// </summary>
        /// <param name="moveOffset"></param>
        /// <param name="rolePos"></param>
        /// <returns></returns>
        private Vector3 CheckCamera(Vector3 moveOffset, Vector3 rolePos, float mx = 0, float my = 0)
        {
            var targetPos = this.followCamera.transform.position;
            if (Mathf.Abs(rolePos.x) <= (this.visibleWidth - this.scaleWidth))//角色坐标x在可见范围内移动相机
                targetPos.x += moveOffset.x;

            if (Mathf.Abs(rolePos.y) <= (this.visibleHight - this.scaleHeight))//角色坐标y在可见范围内移动相机
                targetPos.y += moveOffset.y;

            //缩放值是否大于0
            if (mx > 0 || my > 0)
            {
                var me = this.transform.position;
                //相机位置和玩家的位置大于0.1米，则处于需要缩放的状态
                if (Mathf.Abs(me.x - targetPos.x) > 0.1f)
                {
                    //改变相机位置
                    targetPos.x += (me.x < targetPos.x ? -mx : mx);
                }

                if (Mathf.Abs(me.y - targetPos.y) > 0.1f)
                {
                    targetPos.y += (me.y < targetPos.y ? -my : my);
                }
            }

            //相机位置重置
            //相机缩放的高度和宽度，来控制相机可见范围
            var minClampX = Mathf.Clamp(targetPos.x, -(this.visibleWidth - this.scaleWidth), this.visibleWidth - this.scaleWidth);
            var minClampY = Mathf.Clamp(targetPos.y, -(this.visibleHight - this.scaleHeight), this.visibleHight - this.scaleHeight);
            this.followCamera.transform.position = new Vector3(minClampX, minClampY, targetPos.z);
            return this.followCamera.transform.position;
        }


        private void ScaleCamera(float curScale)
        {
            this.followCamera.orthographicSize = curScale;
            this.scaleWidth = (curScale - this.defaultScaleSize) * this.followCamera.aspect;//缩放，会增加相机size宽度
            this.scaleHeight = (curScale - this.defaultScaleSize);//缩放，会增加相机size高度

            var mx = this.scaleWidth + this.followCamera.aspect; //相机超出边界时的移动x
            var my = this.scaleHeight + 1;//相机超出边界移动量y
            this.CheckCamera(Vector3.zero, Vector3.zero, mx, my);//缩放后校正相机视图区域，避免超出地图视图
        }
    }
}