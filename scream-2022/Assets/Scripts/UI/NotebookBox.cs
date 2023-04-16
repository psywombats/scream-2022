using UnityEngine;
using System.Collections;
using DG.Tweening;

public class NotebookBox : TextAutotyper {

    [SerializeField] private CanvasGroup canvas = null;
    [Space]
    [SerializeField] private float fadeTime = 1f;

    public IEnumerator NotebookRoutine(string text) {
        textbox.text = "";
        Global.Instance.Input.PushListener(this);
        yield return CoUtils.RunTween(canvas.DOFade(1f, fadeTime));
        yield return CoUtils.Wait(.5f);
        yield return TypeRoutine(text);
        yield return CoUtils.RunTween(canvas.DOFade(0f, fadeTime));
        AvatarEvent.Instance.UnpauseInput();
        Global.Instance.Input.RemoveListener(this);
    }
}
