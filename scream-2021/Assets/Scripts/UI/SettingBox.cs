using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class SettingBox : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI text;
    [Space]
    [SerializeField] private float inTime = .7f;
    [SerializeField] private float waitTime = 1f;

    private RectTransform rect;
    private RectTransform Rect => rect ?? (rect = GetComponent<RectTransform>());

    private List<string> toShow = new List<string>();
    private bool showing;

    public void Show(string setting) {
        toShow.Add(setting);
    }

    protected void Update() {
        if (toShow.Count > 0 && !showing) {
            StartCoroutine(ShowRoutine());
        }
    }

    private IEnumerator ShowRoutine() {
        text.text = toShow[0];
        toShow.RemoveAt(0);
        yield return CoUtils.RunTween(rect.DOAnchorPosX(rect.rect.width - 4, inTime));
        yield return CoUtils.Wait(waitTime);
        yield return CoUtils.RunTween(rect.DOAnchorPosX(0, inTime));
    }
}