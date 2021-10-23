using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ControlController : MonoBehaviour {

    public RectTransform set1;
    public RectTransform set2;

    public IEnumerator Trigger(RectTransform set) {
        var orig = set.anchoredPosition.y;
        yield return CoUtils.Wait(3);
        yield return CoUtils.RunTween(set.DOAnchorPosY(0, 1f));
        yield return CoUtils.Wait(3f);
        yield return CoUtils.RunTween(set.DOAnchorPosY(orig, 1f));
    }
}
