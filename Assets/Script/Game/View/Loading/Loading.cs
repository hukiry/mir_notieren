using Hukiry;
using Hukiry.UI;
using UnityEngine;
using UnityEngine.UI;

public class Loading : UIViewMono<Loading>
{
	public UIProgressbarMask sliderFore;
	public Text percentTxt;
	private GameObject sliderBackGo;
	private RectTransform backTf;
	// Start is called before the first frame update
	void Awake()
	{
		backTf = transform.Find("back").GetComponent<RectTransform>();
		sliderBackGo = transform.Find("backSlider").gameObject;
		sliderFore = transform.Find("backSlider/sliderFore").GetComponent<UIProgressbarMask>();
		percentTxt = transform.Find("backSlider/percentTxt").GetComponent<Text>();
		sliderBackGo.SetActive(true);

		float y = 1920;
		float vx = (this.transform as RectTransform).rect.width;
		if (vx > 1080)
			y = vx * 1920 / 1080.0f;

        backTf.SetSizeDelta(vx, y);
    }

	public void ShowProgress(float progress, string title = "", string downTip = "")
	{
		this.gameObject.SetActive(true);
		if (sliderFore)
		{
			sliderFore.fillAmount = progress;
			percentTxt.text = (progress * 100).ToString("f2") + "%";
		}
	}

	public override void HideView()
	{
		this.gameObject.SetActive(false);
		Destroy(this.gameObject);
	}
}
