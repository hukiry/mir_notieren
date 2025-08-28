/// <summary>
///  引用计数器
/// </summary>
public abstract class BaseRefObject
{
    protected int m_ReferencedCount = 0;

    public int ReferencedCount
    {
        get { return m_ReferencedCount; }
    }

    ~BaseRefObject()
    {
#if UNITY_EDITOR
        if (m_ReferencedCount != 0)
        {
            UnityEngine.Debug.Assert(false, "~RefObject" + this.GetType());
        }
#endif
    }

    public void Record()
    {
        m_ReferencedCount++;
    }

    public virtual void Release()
    {
        m_ReferencedCount--;

#if UNITY_EDITOR
        if (m_ReferencedCount < 0)
        {
            UnityEngine.Debug.Assert(false, "RefObject over killed");
        }
#endif

        if (m_ReferencedCount == 0)
        {
            DeleteSelf();
        }
    }

    protected abstract void DeleteSelf();

}
