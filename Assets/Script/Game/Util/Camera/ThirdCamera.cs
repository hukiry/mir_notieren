
using UnityEngine;
using System.Collections;

/// <summary>
///  第三人称摄像机
/// </summary>
public class ThirdCamera : MonoBehaviour {

    //目标对象
    public Transform target;

    // distance from target (used with zoom)
    public bool AllowZoom = true;
    public float Distance = 10.0f; 
    public float MinDistance = 2f;
    public float MaxDistance = 15f;
    public float ZoomSpeed = 1f;
    
    //是否允许X轴旋转
    public bool AllowXAxis = true;
    public float TargetAngleX = 0;
    public float MinAngleX = 0;
    public float MaxAngleX = 0;
    public float XAxisSpeed = 5f;

    //是否允许Ｙ轴旋转
    public bool AllowYAxis = true;
    public float TargetAngleY = 45;
    public float MinAngleY = 0;
    public float MaxAngleY = 90;
    public float YAxisSpeed = 2.5f;

    //private Vector2 trans = Vector2.zero;

    private float FixAngleX;
    private float FixAngleY;
    private float FixDistance;

    private Vector3 Smooth = Vector3.one;

    private Vector3 MoveSmooth = Vector3.one;

    protected float MoveSmoothTime = 0.1f;

    private Vector3 TmpVector3 = Vector3.zero;

    public static ThirdCamera Instance;

    void Awake()
    {
        ThirdCamera.Instance = this;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        PCUpdate();
#elif UNITY_ANDROID || UNITY_IPHONE
        PhoneUpdate();
#endif
    }

    /// <summary>
    /// PC操作
    /// </summary>
    protected void PCUpdate()
    {
        if (AllowZoom)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0.0f) Distance -= ZoomSpeed;
            else if (scroll < 0.0f) Distance += ZoomSpeed;
        }

        //旋转控制
        if (Input.GetMouseButton(1) || (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
        {
            if (AllowXAxis)
            {
                TargetAngleX += Input.GetAxis("Mouse X") * XAxisSpeed;
            }

            if (AllowYAxis)
            {
                TargetAngleY -= Input.GetAxis("Mouse Y") * YAxisSpeed;
            }
        }
    }

    protected Vector2 OldVector1;
    protected Vector2 OldVector2;

    protected Vector2 NewVector1;
    protected Vector2 NewVector2;

    protected bool NeedResetPos;

    protected float mMinEulerV = 2;
    protected float mRotationV = 0.5f; //旋转角度(大于这个角度开始旋转)
    protected float mMinStartX = 10; //缩放距离(大于这个距离开始缩放)

    //pc phone检测摆动
    private bool ChangeCameraY(Vector2 oldPos1, Vector2 oldPos2, Vector2 newPos1, Vector2 newPos2)
    {
        Vector2 V1 = (newPos1 - oldPos1).normalized;
        Vector2 V2 = (newPos2 - oldPos2).normalized;

        if (Mathf.Abs(Vector2.Angle(V1, V2)) <= 30f)
        {
            float move1 = newPos1.y - oldPos1.y;
            float move2 = newPos2.y - oldPos2.y;

            if (Mathf.Abs(move1) >= mMinEulerV
                && Mathf.Abs(move2) >= mMinEulerV)
            {
                TargetAngleY += (move1 + move2) * YAxisSpeed * 0.05f; return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 观看某个坐标点
    /// </summary>
    /// <param name="target"></param>
    /// <param name="distance"></param>
    public void LookAt(Vector3 lookAt, float lookDistance)
    {
        this.Distance = lookDistance;

        target.transform.position = lookAt; 
    }

    /// <summary>
    /// 检查绽放
    /// </summary>
    /// <param name="A1"></param>
    /// <param name="B1"></param>
    /// <param name="A2"></param>
    /// <param name="B2"></param>
    /// <returns></returns>
    private int IsEnlarge(Vector2 A1, Vector2 B1, Vector2 A2, Vector2 B2)
    {
        float oldDis = Mathf.Sqrt((A1.x - B1.x) * (A1.x - B1.x) + (A1.y - B1.y) * (A1.y - B1.y));
        float newDis = Mathf.Sqrt((A2.x - B2.x) * (A2.x - B2.x) + (A2.y - B2.y) * (A2.y - B2.y));

        //float oldDis = (A1 - B1).magnitude;
        //float newDis = (A2 - B2).magnitude;

        if (newDis - oldDis > mMinStartX)
        {
            return 1;
        }
        else if (oldDis - newDis > mMinStartX)
        {
            return -1;
        }
        
        return 0;
    }

    //phone 检测旋转
    private float CheckRotation(Vector2 oldPos1, Vector2 oldPos2, Vector2 tempPos1, Vector2 tempPos2)
    {
        Vector2 vecA = (oldPos1 - oldPos2).normalized;
        Vector2 vecB = (tempPos1 - tempPos2).normalized;
        float perpDot = (vecA.x * vecB.y) - (vecA.y * vecB.x);
        float v1 = Vector2.Dot(vecA, vecB);
        float v = Mathf.Rad2Deg * Mathf.Atan2(perpDot, v1);
        return v;
    }

    /// <summary>
    /// 手机操作
    /// </summary>
    protected void PhoneUpdate()
    {
        if (Input.touchCount < 2) { return; }

        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
        {
            OldVector1 = touch0.position;
            OldVector2 = touch1.position;
        }

        NewVector1 = touch0.position;
        NewVector2 = touch1.position;

        //Y轴
        bool isStatic = false;
        if (touch0.phase == TouchPhase.Moved && touch1.phase == TouchPhase.Moved)
        {
            if (AllowYAxis)
            {
                ChangeCameraY(OldVector1, OldVector2, NewVector1, NewVector2);
            }
        }

        //判断缩放
        if (isStatic == false && 
            touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
        {
            int isSize = IsEnlarge(OldVector1, OldVector2, NewVector1, NewVector2);
            if (AllowZoom && isSize != 0)
            {
                Distance -= ZoomSpeed * isSize * 10f;
            }
            else if(AllowXAxis)
            {
                //旋转
                float mResultV = CheckRotation(OldVector1, OldVector2, NewVector1, NewVector2);

                if (Mathf.Abs(mResultV) >= mRotationV)
                {
                    TargetAngleX += mResultV * XAxisSpeed * 0.5f;
                }
            }
            
        }

        OldVector1 = touch0.position;
        OldVector2 = touch1.position;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        //float aspectRatio = Camera.main.aspect;

        //Camera.main.orthographicSize = 9.6f / (2 * aspectRatio);

        // scroll wheel used to zoom in/out

        if (!target) return;

        //修正数据
        FixAngleX = Mathf.SmoothDampAngle(FixAngleX, TargetAngleX, ref Smooth.x, 0.3f);

        TargetAngleY = ClampAngle(TargetAngleY, MinAngleY, MaxAngleY);
        FixAngleY = Mathf.SmoothDampAngle(FixAngleY, TargetAngleY, ref Smooth.y, 0.3f);

        Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
        FixDistance = Mathf.SmoothDamp(FixDistance, Distance, ref Smooth.z, 0.3f);

        //数据有变化做重新修正
        if (FixDistance != Distance || FixAngleX != TargetAngleX || FixAngleY != TargetAngleY)
        {
            TmpVector3.z = FixDistance;

            Quaternion rotation = Quaternion.Euler(FixAngleY, FixAngleX, 0);
            transform.rotation = rotation;

            // apply
            Vector3 newPosition = target.position + rotation * -TmpVector3;
            transform.position = newPosition;//Vector3.SmoothDamp(transform.position, newPosition, ref MoveSmooth, MoveSmoothTime);
        }
	}

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}