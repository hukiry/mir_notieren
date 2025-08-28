using UnityEngine;
namespace Hukiry
{
    /// <summary>
    /// 编辑模式运行的脚本
    /// Author:胡雄坤
    /// </summary>
    [ExecuteAlways]
    public class DrawMap : MonoBehaviour
    {
#if UNITY_EDITOR

        private float mapWidth;
        private float mapHeight;

        public Transform role;

        Transform cameraTfg;
        private void InitSize()
        {
            FollowCameraCtl coreCameraCtl = GameObject.FindObjectOfType<FollowCameraCtl>();
            if (coreCameraCtl)
            {
                mapWidth = coreCameraCtl.mapWidth;
                mapHeight = coreCameraCtl.mapHeight;
                cameraTfg = coreCameraCtl.followCamera.transform;
            }
        }


        private void OnDrawGizmos()
        {
            InitSize();
            Vector3 pos1 = new Vector3(-mapWidth / 2, mapHeight / 2);
            Vector3 pos2 = new Vector3(mapWidth / 2, mapHeight / 2);
            Vector3 pos3 = new Vector3(mapWidth / 2, -mapHeight / 2);
            Vector3 pos4 = new Vector3(-mapWidth / 2, -mapHeight / 2);
            //绘制地图
            Debug.DrawLine(pos1, pos2, Color.red);
            Debug.DrawLine(pos2, pos3, Color.yellow);
            Debug.DrawLine(pos3, pos4, Color.green);
            Debug.DrawLine(pos4, pos1, Color.blue);

            if (role)
            {
                var p = role.position;
                float v = 1f;
                Debug.DrawLine(new Vector3(p.x - v, p.y), new Vector3(p.x + v, p.y), Color.red);
                Debug.DrawLine(new Vector3(p.x, p.y - v), new Vector3(p.x, p.y + v), Color.green);

            }

            //if (cameraTfg)
            //{
            //    Vector3 po1 = new Vector3(-Screen.width / 100, Screen.height / 100) + cameraTfg.position;
            //    Vector3 po2 = new Vector3(Screen.width / 100, Screen.height / 100) + cameraTfg.position;
            //    Vector3 po3 = new Vector3(Screen.width / 100, -Screen.height / 100) + cameraTfg.position;
            //    Vector3 po4 = new Vector3(-Screen.width / 100, -Screen.height / 100) + cameraTfg.position;
            //    Debug.DrawLine(po1, po2, Color.white);
            //    Debug.DrawLine(po2, po3, Color.yellow);
            //    Debug.DrawLine(po3, po4, Color.white);
            //    Debug.DrawLine(po4, po1, Color.yellow);
            //}

        }
#endif
    }

}