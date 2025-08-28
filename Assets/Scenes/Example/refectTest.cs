using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class refectTest : MonoBehaviour
{
    [SerializeField]
    private int MAX_bOUNCES=1;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float Lerp(float a, float b, float t) => (1 - t) * a + b * t;
    public float InverseLerp(float a, float b, float t) => (t-a) /(b-a);

    [SerializeField]
    private float angle=30;

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        if (target == null) return;
        if (Contains(target.position))
        {
            Gizmos.DrawLine(target.position, transform.position);
        }
        
        //桌球();
    }

    public bool Contains(Vector3 postion)
    {
        Vector3 vecDir = postion - transform.position;
        Vector3 vecToTarget = transform.InverseTransformVector(vecDir);
        vecToTarget.y = 0;
        var noraml = vecToTarget.normalized;
        return noraml.z < angle;
    }

    private void 桌球()
    {
        Ray ray = new Ray(transform.position, transform.right);
        for (int i = 0; i < MAX_bOUNCES; i++)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Gizmos.color = Color.red * 0.8f;
                Gizmos.DrawLine(ray.origin, hit.point);
                Vector2 reflect = Reflect(ray.direction, hit.normal);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(hit.point, (Vector2)hit.point + reflect);
                ray.direction = reflect;
                ray.origin = hit.point;
            }
            else break;
        }
    }

    Vector2 Reflect(Vector2 direction, Vector2 normal)
    {
        float proj = Vector2.Dot(direction, normal);
        return direction - 2 * proj * normal;
    }
}
