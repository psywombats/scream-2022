using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PortraitComponent : MonoBehaviour {

    private static readonly float fadeTime = 0.5f;
    private static readonly float highlightTime = 0.3f;
    private static readonly float inactiveAlpha = 0.5f;

    public NVLComponent nvl;
    public Image sprite;
    public Image altSprite;
    public bool moveSibling;

    public SpeakerData Speaker { get; private set; }
    public bool IsHighlighted { get; private set; }

    public void Clear() {
        Speaker = null;
        sprite.sprite = null;
        altSprite.sprite = null;
        IsHighlighted = false;
    }

    public IEnumerator EnterRoutine(SpeakerData speaker, string expr = null) {
        if (Speaker != null) {
            yield return ExitRoutine();
        }
        Speaker = speaker;
        sprite.sprite = speaker.GetExpr(expr);
        
        sprite.SetNativeSize();
        sprite.color = new Color(1, 1, 1, 0);
        IsHighlighted = true;
        var tween = sprite.DOFade(1.0f, fadeTime);
        yield return CoUtils.RunTween(tween);

        IsHighlighted = true;
    }

    public IEnumerator ExitRoutine() {
        if (Speaker != null) {
            var tween = sprite.DOFade(0.0f, fadeTime);
            yield return CoUtils.RunTween(tween);
            Clear();
        }
        if (moveSibling) {
            //transform.SetAsFirstSibling();
        }
    }

    public IEnumerator HighlightRoutine() {
        if (sprite.color.r == 1.0f) {
            yield break;
        }
        var tween = sprite.DOColor(new Color(1, 1, 1, 1), highlightTime);
        if (moveSibling) {
            //transform.SetAsLastSibling();
        }
        yield return CoUtils.RunTween(tween);

        IsHighlighted = true;
    }

    public IEnumerator ExpressRoutine(string expr) {
        if (!IsHighlighted) {
            yield return nvl.SetHighlightRoutine(Speaker);
        }
        altSprite.color = new Color(1, 1, 1, 0);
        altSprite.sprite = Speaker.GetExpr(expr);
        altSprite.SetNativeSize();
        yield return CoUtils.RunParallel(this,
            //CoUtils.RunTween(sprite.DOColor(new Color(inactiveAlpha, inactiveAlpha, inactiveAlpha, 1f), highlightTime)),
            CoUtils.RunTween(altSprite.DOFade(1f, highlightTime)));
        sprite.sprite = altSprite.sprite;
        sprite.color = Color.white;
        altSprite.color = Color.clear;
        altSprite.sprite = null;
    }

    public IEnumerator UnhighlightRoutine() {
        if (Speaker == null || sprite.color.r == inactiveAlpha) {
            yield break;
        }
        var tween = sprite.DOColor(new Color(inactiveAlpha, inactiveAlpha, inactiveAlpha, 1), highlightTime);
        yield return CoUtils.RunTween(tween);
        if (moveSibling) {
            //transform.SetAsFirstSibling();
        }

        IsHighlighted = false;
    }
}
