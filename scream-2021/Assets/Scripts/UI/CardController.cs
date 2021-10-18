using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CardController : MonoBehaviour {

    [SerializeField] private Image image = null;

    public IEnumerator ShowRoutine(PortraitData cardData) {
        image.sprite = cardData.portrait;
        yield return GetComponent<CanvasGroup>().DOFade(1f, .7f);
        yield return InputManager.Instance.AwaitConfirm();
        yield return GetComponent<CanvasGroup>().DOFade(0f, .7f);
    }
}