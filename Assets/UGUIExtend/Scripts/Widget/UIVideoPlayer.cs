using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
[DrawIcon(typeof(VideoPlayer), null)]
public class UIVideoPlayer : MonoBehaviour
{
    private VideoPlayer m_VideoPlayer;

    private void Awake()
    {
        m_VideoPlayer = GetComponent<VideoPlayer>();
    }

    public static UIVideoPlayer Get(RawImage rawImage)
    {
        if (rawImage.gameObject.GetComponent<VideoPlayer>() == null)
        {
            rawImage.gameObject.AddComponent<VideoPlayer>();
        }

        UIVideoPlayer listener = rawImage.GetComponent<UIVideoPlayer>() ?? rawImage.gameObject.AddComponent<UIVideoPlayer>();
        return listener;
    }

    public void PlayVideo(VideoClip videoClip, RenderTexture renderTexture)
    {
        m_VideoPlayer.targetTexture?.Release();
        m_VideoPlayer.targetTexture = renderTexture;
        m_VideoPlayer.clip = videoClip;
        m_VideoPlayer.Play();
    }
}