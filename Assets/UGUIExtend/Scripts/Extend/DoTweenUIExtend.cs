using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Hukiry.UI;
using UnityEngine;
namespace Hukiry.UI
{
    public static class DoTweenUIExtend
    {
        public static TweenerCore<float, float, FloatOptions> DOFillAmount(this UIProgressbarMask target, float endValue, float duration)
        {
            if (endValue > 1) endValue = 1;
            else if (endValue < 0) endValue = 0;
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.fillAmount, x => target.fillAmount = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

    }
}
