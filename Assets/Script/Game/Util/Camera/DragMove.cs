
using System;
using UnityEngine;

/// <summary>
///  拖动场景对象
/// </summary>
public class DragMove : MonoBehaviour
{
    /// <summary>
    /// 地图面板层
    /// </summary>
    public int CityPlane = 8;

    /// <summary>
    /// 地图层
    /// </summary>
    public int CityMap = 9;

    //允许拖动
    public bool EnableDragMove = true;

    protected bool KeepMoving;
    protected Vector3 StartPosition;
    protected Vector3 SlidePosition;
    protected Vector3 CurrVelocity;
    public float MoveSpeed = 1f;

    public Action<GameObject, bool> OnMouseUpEevnt;

    //最大最小X轴边界
    public float MinBorderX = 20;
    public float MaxBorderX = 80;

    //最大最小Z轴边界
    public float MinBorderZ = 20;
    public float MaxBorderZ = 80;

    //允许方向键盘移动
    public bool EnableKeyMoving = true;
    public float KeyPaddingSpeed = 0.2f;

    //允许鼠标边缘移动
    public bool EnableMousePadding = false;
    public float PaddingSpeed = 2f;

    public float PaddingWidth = 10f;
    public float PaddingHeigh = 10f;

    //拖动对象
    public Transform DragObject;

    static DragMove mInstance = null;
    public static DragMove Instance
    {
        get { return mInstance; }
    }

    void Start()
    {
        mInstance = this;

        CityPlane = LayerMask.NameToLayer("CityPlane");
        CityMap   = LayerMask.NameToLayer("CityMap");
    }

    /// <summary>
    /// 设置拖拽对象
    /// </summary>
    /// <param name="target">拖动的对象</param>
    public void SetDragObject(Transform target)
    {
        this.DragObject = target; EnableDragMove = false; 
        this.KeepMoving = target ? false : KeepMoving;
    }

    /// <summary>
    /// 拖动到某个坐标
    /// </summary>
    /// <param name="target"></param>
    public void MoveToViewPoint(Vector3 target, Vector2 viewPoint)
    {
        var ScreenPos = Camera.main.ViewportToScreenPoint(viewPoint);

        var WorldPos1 = target;

        RaycastHit hitInfo;

        Ray ray = Camera.main.ScreenPointToRay(ScreenPos);
        if (Physics.Raycast(ray, out hitInfo, 100000, 1 << CityMap))
        {
            WorldPos1 = hitInfo.point;
        }

        var DragMove = target - WorldPos1;
        DragMove.y = 0; MoveSpeed = 10f;

        SlidePosition = DragMoveLimit(transform.position + DragMove);
    }

    /// <summary>
    /// 边界限制
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    protected Vector3 DragMoveLimit(Vector3 targetPos)
    {
        targetPos.x = Mathf.Clamp(targetPos.x, MinBorderX, MaxBorderX);
        targetPos.z = Mathf.Clamp(targetPos.z, MinBorderZ, MaxBorderZ);
        //targetPos.y = 0f;

        return targetPos;
    }

    public Vector3 GetHitPosition(int layer)
    {
        RaycastHit hitInfo;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 100000, 1 << layer))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }

    void Update()
    {
        if (Input.touchCount > 1) { KeepMoving = false; return; }

        //鼠标拖动移动
        if (EnableDragMove)
        {
            if (Input.GetMouseButton(0))
            {
                var newPosition = this.GetHitPosition(CityPlane);
                if (newPosition.Equals(Vector3.zero)) return;

                if (!KeepMoving)
                {
                    StartPosition = newPosition; 
                    SlidePosition = Vector3.zero;
                    KeepMoving = true; return;
                }

                var move = StartPosition - newPosition; move.y = 0;
                if (move.magnitude > 0.1f)
                {
                    transform.position = DragMoveLimit(transform.position + move);
                }
            }
        }

        if (!DragObject && KeepMoving && Input.GetMouseButtonUp(0))
        {
            MoveSpeed = 10f; KeepMoving = false;

            var move = StartPosition - GetHitPosition(CityPlane); move.y = 0;
            if (move.magnitude > 0.1f)
            {
                SlidePosition = DragMoveLimit(transform.position + move * 5);
            }
        }

        //滑动后的移动
        if (SlidePosition.magnitude > 0.1f && (MoveSpeed -= 0.3f) > 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, SlidePosition, ref CurrVelocity, Time.deltaTime * MoveSpeed);
        }

        //拖动物体
        if (DragObject && Input.GetMouseButton(0))
        {
            var newPosition = this.GetHitPosition(CityMap);
            if (newPosition.Equals(Vector3.zero)) return;

            //newPosition.y = 0;
            DragObject.position = newPosition;
        } 
        else if(DragObject && Input.GetMouseButtonUp(0))
        {
            if (OnMouseUpEevnt != null) { 
                OnMouseUpEevnt(DragObject.gameObject, false);
            }
        }

        //方向键移动
        if (EnableKeyMoving && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            Quaternion direction = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

            //水平
            if (Input.GetButton("Horizontal"))
            {
                Vector3 dirHor = transform.InverseTransformDirection(direction * Vector3.right);
                //target.Translate(dir * Input.GetAxisRaw("Horizontal"));

                Vector3 moveLR = dirHor * Input.GetAxisRaw("Horizontal") * KeyPaddingSpeed;
                transform.position = DragMoveLimit(transform.position + moveLR);
            }

            //垂直
            if (Input.GetButton("Vertical"))
            {
                Vector3 dirVer = transform.InverseTransformDirection(direction * Vector3.forward);
                //target.Translate(dir * Input.GetAxisRaw("Vertical"));

                Vector3 moveRL = dirVer * Input.GetAxisRaw("Vertical") * KeyPaddingSpeed;
                transform.position = DragMoveLimit(transform.position + moveRL);
            }
        }

        //鼠标边缘移动
        if (EnableMousePadding || DragObject)
        {
            Vector3 mousePos = Input.mousePosition;
            
            float RightSpeed = 0f; float ForwardSpeed = 0f;

            //上下
            if (mousePos.y <= 0) ForwardSpeed = PaddingSpeed * -3;
            else if (mousePos.y <= PaddingHeigh) ForwardSpeed = PaddingSpeed * -1;
            else if (mousePos.y >= Screen.height) ForwardSpeed = PaddingSpeed * 3;
            else if (mousePos.y > Screen.height - PaddingHeigh) ForwardSpeed = PaddingSpeed;

            //左右
            if (mousePos.x <= 0) RightSpeed = PaddingSpeed * -3;
            else if (mousePos.x <= PaddingWidth) RightSpeed = PaddingSpeed * -1;
            else if (mousePos.x >= Screen.width) RightSpeed = PaddingSpeed * 3;
            else if (mousePos.x > Screen.width - PaddingWidth) RightSpeed = PaddingSpeed;

            //减少运算
            if (Math.Abs(RightSpeed) > 0 || Math.Abs(ForwardSpeed) > 0)
            {
                Quaternion direction = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

                //上下
                Vector3 dirVer = transform.InverseTransformDirection(direction * Vector3.back);
                Vector3 moveUD = dirVer * ForwardSpeed * Time.deltaTime;
                transform.position = DragMoveLimit(transform.position + moveUD);

                //左右
                Vector3 dirHor = transform.InverseTransformDirection(direction * Vector3.left);
                Vector3 moveRL = dirHor * RightSpeed * Time.deltaTime;
                transform.position = DragMoveLimit(transform.position + moveRL);
            }
        }
    }
}