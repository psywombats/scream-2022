using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SettingBox : MonoBehaviour {

    [SerializeField] private Text text;
    [Space]
    [SerializeField] private float inTime = .7f;
    [SerializeField] private float waitTime = 1f;

    private RectTransform rect;
    private RectTransform Rect => rect ?? (rect = GetComponent<RectTransform>());

    private List<string> toShow = new List<string>();
    private bool showing;

    public void Show(string setting) {
        if (!toShow.Contains(setting)) {
            toShow.Add(setting);
        }
    }

    public void Scramble() {
        var s = "";
        foreach (var _ in text.text) {
            s += (char)Random.Range(97, 122);
        }
        text.text = s;
    }

    protected void Update() {
        if (toShow.Count > 0 && !showing) {
            showing = true;
            StartCoroutine(ShowRoutine());
        }
    }

    private IEnumerator ShowRoutine() {
        text.text = toShow[0];
        yield return null;
        yield return CoUtils.RunTween(Rect.DOAnchorPosX(text.rectTransform.rect.width + 48 * 2, inTime));
        yield return CoUtils.Wait(waitTime);
        yield return CoUtils.RunTween(Rect.DOAnchorPosX(0, inTime));
        toShow.RemoveAt(0);
        showing = false;
    }
}
