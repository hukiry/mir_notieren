using UnityEngine;

/// <summary>
/// 只对场景碰撞盒子生效
/// </summary>
[DrawIcon(typeof(AudioListener),null)]
public class ObjectListener : MonoBehaviour
{
    public delegate void SceneHukiryHandler(GameObject pointer);

    [SerializeField]
    public SceneHukiryHandler onClickDown = null, onClickUp = null, onClickUpButton = null, onClickDrag = null
        , onMouseEnter = null, onMouseExit = null;

    public static ObjectListener Get(GameObject go)
    {
        if (go.GetComponent<BoxCollider2D>() == null)
        {
            LogManager.LogError("BoxCollider2D is not null, object name is :" + go?.name);
        }

        ObjectListener listener = go.GetComponent<ObjectListener>() ?? go.AddComponent<ObjectListener>();
        return listener;
    }
    public static ObjectListener Get(Transform go) => Get(go.gameObject);
    public static ObjectListener Get(MonoBehaviour go) => Get(go.gameObject);
    private void OnMouseDown()
    {
        onClickDown?.Invoke(gameObject);
    }

    private void OnMouseUp()
    {
        onClickUp?.Invoke(gameObject);
    }

    private void OnMouseUpAsButton()
    {
        onClickUpButton?.Invoke(gameObject);
    }

    private void OnMouseEnter()
    {
        onMouseEnter?.Invoke(gameObject);
    }

    private void OnMouseDrag()
    {
        onClickDrag?.Invoke(gameObject);
    }

    private void OnMouseExit()
    {
        onMouseExit?.Invoke(gameObject);
    }
}
