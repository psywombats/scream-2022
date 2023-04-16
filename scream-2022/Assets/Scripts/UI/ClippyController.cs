using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClippyController : TextAutotyper {

    [SerializeField] private CanvasGroup canvas = null;
    [SerializeField] private Image portraitImage = null;
    [Space]
    [SerializeField] private float fadeTime = 1f;

    public IEnumerator ShowRoutine(string portraitName, string text) {
        textbox.text = "";
        var portrait = IndexDatabase.Instance.Portraits.GetData(portraitName);
        portraitImage.sprite = portrait.portrait;
        Global.Instance.Input.PushListener(this);
        yield return CoUtils.RunTween(canvas.DOFade(1f, fadeTime));
        yield return CoUtils.Wait(.5f);
        yield return TypeRoutine(text);
        yield return CoUtils.RunTween(canvas.DOFade(0f, fadeTime));
        AvatarEvent.Instance.UnpauseInput();
        Global.Instance.Input.RemoveListener(this);
    }
}
