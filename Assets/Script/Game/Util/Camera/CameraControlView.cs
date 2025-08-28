
using UnityEngine;
using System.Collections;

public class CameraControlView : MonoBehaviour 
{
    public bool mHandleInput = true;  //是否手势输入
    public bool mLimitOperate = false;        //是否限制用户操作
    public bool mUseSmoothEffect = true; //启用平滑效果
    public GameObject mTarget;

    public float mSmoothTime = 0.3f; //平滑时间

    public float mRotationV = 0.5f; //旋转角度(大于这个角度开始旋转)
    public float mRotSpeed = 6; //旋转速度

    public float mMinLimtY = 15; //角度限制(摆动最小角度)
    public float mMaxLimitY = 85; //(摆动最大角度)
    public float mMinEulerV = 2; //摆动角度(大于这个角度开始摆动)
    public float mCamerEulerv = 2; //每次摆动角度值

    public float mDistance = 40; //观察距离
    public float mMaxDistance = 60; //缩放最大值
    public float mMinDistance = 40; //缩放最小值  
    public float mDistanceStart = 10; //缩放距离(大于这个距离开始缩放)
    public float mZoomSpeed = 4; //缩放速度
    public float mFixDistance = 60; //默认开始摄像机距离

    

    //系统变量

    //旋转角度
    private float mX;
    private float mY;
    private Quaternion mRotation;

    private Touch mTouch0;
    private Touch mTouch1;
    private Vector2 mOldPosition1;
    private Vector2 mOldPosition2;
    private Vector2 mTempPosition1;
    private Vector2 mTempPosition2;
    private bool mNeedGetPos = false;

    //平滑效果变量
    private float mFixAngleX;
    private float mFixAngleY;
    private Vector3 mSmooth = Vector3.one;
    private Vector3 mMoveSmooth = Vector3.one;
    protected float mMoveSmoothTime = 0.1f;
    private Vector3 mTmpVector3 = Vector3.zero;


    //设置是否限制用户操作
    public void SetLimitOperate(bool limit)
    {
        mLimitOperate = limit;
    }

    void Start()
    {
        //初始化旋转角度
        mX = transform.eulerAngles.y;
        mY = transform.eulerAngles.x;
        mFixAngleX = mX;
        mFixAngleY = mY;
        mRotation = Quaternion.Euler(mY, mX, 0);
    }

    void Update()
    {
        //HandleMoveToTarget();

        if (mLimitOperate == false)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            PCUpdate();
#elif UNITY_ANDROID || UNITY_IPHONE
            PhoneUpdate();
#endif
            mOldPosition1 = mTempPosition1;
            mOldPosition2 = mTempPosition2;
        }
        SetCamera();
    }

    //处理PC手势
    private void PCUpdate()
    {
        if (this.mHandleInput == false)
        {
            return;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            mDistance = mDistance - Input.GetAxis("Mouse ScrollWheel") * mZoomSpeed;
        }

        else if (Input.GetMouseButton(1))
        {
            if ((Mathf.Abs(Input.GetAxis("Mouse X")) > 0 || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                mTempPosition1 = Input.mousePosition;
                mTempPosition2 = Input.mousePosition;
                ChangeCameraY(mOldPosition1, mOldPosition2, mTempPosition1, mTempPosition2);
            }
            else
            {
                HandleRotation();
            }
        }
    }

    void PhoneUpdate()
    {
        if (this.mHandleInput == false)
        {
            return;
        }

        if (Input.touchCount < 2)
        {
            mNeedGetPos = true;
            return;
        }

        else if (Input.touchCount == 2)
        {
            mTouch0 = Input.GetTouch(0);
            mTouch1 = Input.GetTouch(1);
            mTempPosition1 = mTouch0.position;
            mTempPosition2 = mTouch1.position;
            if (mNeedGetPos == true)
            {
                mOldPosition2 = mTempPosition2;
                mOldPosition1 = mTempPosition1;
                mNeedGetPos = false;
            }
        }

        bool mIsSway = false;
        if (mTouch0.phase == TouchPhase.Moved && mTouch1.phase == TouchPhase.Moved)
        {
            mIsSway = ChangeCameraY(mOldPosition1, mOldPosition2, mTempPosition1, mTempPosition2);
        }

        if (mIsSway == false && mTouch0.phase == TouchPhase.Moved || mTouch1.phase == TouchPhase.Moved)
        {
            int isSize = isEnlarge(mOldPosition1, mOldPosition2, mTempPosition1, mTempPosition2);
            if (isSize == 1)
            {
                mDistance = mDistance - mZoomSpeed * 0.1f;
            }
            else if (isSize == -1)
            {
                mDistance = mDistance + mZoomSpeed * 0.1f;
            }
            float mResultV = IsRotation(mOldPosition1, mOldPosition2, mTempPosition1, mTempPosition2);
            bool mStartRot = Mathf.Abs(mResultV) >= mRotationV;
            if (mStartRot)
            {
                mX = mX + mResultV * mRotSpeed * 0.25f;
                mRotation = Quaternion.Euler(mY, mX, 0);
            }
        }
    }

    //设置摄像机位置跟角度
    private void SetCamera()
    {
        if (mTarget == null) return;

        mDistance = Mathf.Clamp(mDistance, mMinDistance, mMaxDistance);
        if (mUseSmoothEffect == false)
        {
            //Quaternion.Lerp()
            //重新计算位置
            mTmpVector3 = mRotation * new Vector3(0, 0, -mDistance);
            Vector3 mPosition = mTarget.transform.position + mTmpVector3;
            //设置相机的角度和位置
            transform.position = mPosition;
            transform.rotation =  mRotation;
            transform.LookAt(mTarget.transform, Vector3.up);

        }
        else
        {
            //修正数据
            mFixAngleX = Mathf.SmoothDampAngle(mFixAngleX, mX, ref mSmooth.x, mSmoothTime);
            mFixAngleY = Mathf.SmoothDampAngle(mFixAngleY, mY, ref mSmooth.y, mSmoothTime);
            mFixDistance = Mathf.SmoothDamp(mFixDistance, mDistance, ref mSmooth.z, mSmoothTime);

            //数据有变化做重新修正
            if (mFixDistance != mDistance || mFixAngleX != mX || mFixAngleY != mY)
            {
                Quaternion rotation = Quaternion.Euler(mFixAngleY, mFixAngleX, 0);
                mTmpVector3 = rotation * new Vector3(0, 0, -mFixDistance);
                transform.rotation = rotation;
                Vector3 newPosition = mTarget.transform.position + mTmpVector3;
                transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref mMoveSmooth, mMoveSmoothTime);
            }
        }
    }

    //pc 旋转
    private void HandleRotation()
    {
        float mTempV = Input.GetAxis("Mouse X");
        if (Mathf.Abs(Input.GetAxis("Mouse X")) < Mathf.Abs(Input.GetAxis("Mouse Y")))
        {
            mTempV = Input.GetAxis("Mouse Y");
        }
        if (mTempV != 0)
        {
            mY = mRotation.eulerAngles.x;
            mX = mRotation.eulerAngles.y;
            mX = mX + mTempV * mRotSpeed;
            mRotation = Quaternion.Euler(mY, mX, 0);
            mX = mX + mTempV * mRotSpeed * 2;
        }
    }

    //pc phone检测摆动
    private bool ChangeCameraY(Vector2 oldPos1, Vector2 oldPos2, Vector2 tempPos1, Vector2 tempPos2)
    {
        if (tempPos1.y - oldPos1.y >= mMinEulerV && tempPos2.y - oldPos2.y >= mMinEulerV)
        {
            mY = mY - mCamerEulerv;
            mY = ClampAngle(mY, mMinLimtY, mMaxLimitY);
            mRotation = Quaternion.Euler(mY, mX, 0);
            return true;
        }
        else if (oldPos1.y - tempPos1.y >= mMinEulerV && oldPos2.y - tempPos2.y >= mMinEulerV)
        {
            mY = mY + mCamerEulerv;
            mY = ClampAngle(mY, mMinLimtY, mMaxLimitY);
            mRotation = Quaternion.Euler(mY, mX, 0);
            return true;
        }
        return false;
    }

    //phone 检测缩放
    private int isEnlarge(Vector2 oldPos1, Vector2 oldPos2, Vector2 tempPos1, Vector2 tempPos2)
    {
        float oldDis = Mathf.Sqrt((oldPos1.x - oldPos2.x) * (oldPos1.x - oldPos2.x) + (oldPos1.y - oldPos2.y) * (oldPos1.y - oldPos2.y));
        float newDis = Mathf.Sqrt((tempPos1.x - tempPos2.x) * (tempPos1.x - tempPos2.x) + (tempPos1.y - tempPos2.y) * (tempPos1.y - tempPos2.y));
        if (newDis - oldDis > mDistanceStart)
        {
            return 1;
        }
        else if (oldDis - newDis > mDistanceStart)
        {
            return -1;
        }
        return 0;
    }

    //phone 检测旋转
    private float IsRotation(Vector2 oldPos1, Vector2 oldPos2, Vector2 tempPos1, Vector2 tempPos2)
    {
        Vector2 vecA = oldPos1 - oldPos2;
        Vector2 vecB = tempPos1 - tempPos2;
        float perpDot = (vecA.x * vecB.y) - (vecA.y * vecB.x);
        float v1= Vector2.Dot(vecA,vecB);
        float v = Mathf.Rad2Deg * Mathf.Atan2(perpDot, v1);
        return v;
    }

    //角度限制
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    //
    private void HandleMoveToTarget()
    {
        //mMoveSpeed -= 0.1f;

        //if (mTargetRot != Quaternion.identity)
        //{
        //    mRotation = Quaternion.Lerp(mRotation, mTargetRot, Time.deltaTime * mMoveSpeed * 0.5f);
        //}

        //if (mTargetDis != 0)
        //{
        //    float Dis = 0;
        //    mDistance = Mathf.SmoothDamp(mDistance, mTargetDis, ref Dis, Time.deltaTime * mMoveSpeed * 0.5f);
        //}
    }

    private void SetIsMoveToTarget(bool move)
    {
        //if (mIsMoveToTarget == move)
        //{
        //    return;
        //}
        //mIsMoveToTarget = move;
        //if (mIsMoveToTarget == false)
        //{
        //    mTargetRot = Quaternion.identity;
        //    mTargetDis = 0;
        //}
    }
}