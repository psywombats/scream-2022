using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class KeywordComponent : MonoBehaviour {

    public Vector2 Velocity { get; set; }

    private RectTransform rect;
    public RectTransform Rect => rect ?? (rect = GetComponent<RectTransform>());

    protected void Update() {
        Rect.anchoredPosition += Velocity * Time.deltaTime;
    }
}
