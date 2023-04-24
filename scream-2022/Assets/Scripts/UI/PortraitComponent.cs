using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

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

    public IEnumerator JoltRoutine() {
        var duration = .7f;
        if (!IsHighlighted) yield return nvl.SetHighlightRoutine(Speaker);
        yield return CoUtils.RunParallel(this, GlitchBrushRoutine(duration), GlitchRoutine(duration));
    }

    private IEnumerator GlitchBrushRoutine(float duration) {
        var interval = 50;
        var i = 0;
        altSprite.color = Color.white;
        for (var ms = 0; ms < duration * 1000; ms += interval) {
            i += Random.Range(0, 2) == 0 ? 1 : -1;
            if (i < 0) i += Speaker.glitchBrush.Count;
            if (i >= Speaker.glitchBrush.Count) i = 0;
            altSprite.sprite = Speaker.glitchBrush[i];
            altSprite.transform.localScale = new Vector3(
                altSprite.transform.localScale.x * (Random.Range(0, 2) == 0 ? 1 : -1),
                altSprite.transform.localScale.y * (Random.Range(0, 2) == 0 ? 1 : -1),
                altSprite.transform.localScale.z);
            altSprite.SetNativeSize();
            yield return CoUtils.Wait(interval / 1000f);
        }
        altSprite.sprite = null;
        altSprite.transform.localScale = new Vector3(
            Mathf.Abs(altSprite.transform.localScale.x),
            Mathf.Abs(altSprite.transform.localScale.y),
            Mathf.Abs(altSprite.transform.localScale.z));
        altSprite.color = Color.clear;
    }

    private IEnumerator GlitchRoutine(float duration) {
        var frameLength = (int)(duration * 1000f / 7f);
        sprite.sprite = Speaker.glitch;
        yield return CoUtils.Wait(frameLength / 1000f);
        sprite.sprite = Speaker.spooky;
        yield return CoUtils.Wait(frameLength / 1000f * 3f);
        sprite.sprite = Speaker.glitch;
        yield return CoUtils.Wait(frameLength / 1000f);
        sprite.sprite = Speaker.spooky;
        yield return CoUtils.Wait(frameLength / 1000f);
        sprite.sprite = Speaker.image;
    }
}
