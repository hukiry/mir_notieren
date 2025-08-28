using UnityEngine;

public class UIClickScale : MonoBehaviour
{
	RectTransform rect;
	public float downScale = 0.9f;

	private float toScale = 1;
	private Vector3 localScale;
	void Awake()
	{
		rect = transform as RectTransform;
		UIEventListener.Get(transform.gameObject).onClickDown = OnPointerDown;
		UIEventListener.Get(transform.gameObject).onClickUp = OnPointerUp;
		localScale = rect.localScale;
	}
	private void OnPointerDown(GameObject eventData)
	{
		toScale = downScale;
	}

	private void OnPointerUp(GameObject eventData)
	{
		toScale = 1;
	}

	private void Update()
	{
		if (rect.localScale.x != toScale)
		{
			rect.localScale = Vector3.Lerp(rect.localScale, localScale * toScale, Time.deltaTime * 20);
		}
	}
}
