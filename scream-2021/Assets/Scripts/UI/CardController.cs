using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CardController : MonoBehaviour {

    [SerializeField] private Image image = null;

    public IEnumerator ShowRoutine(PortraitData cardData) {
        image.sprite = cardData.portrait;
        if (!cardData.sudden) {
            image.color = Color.white;
            StartCoroutine(AudioRoutine(cardData));
            yield return GetComponent<CanvasGroup>().DOFade(1f, .7f);
        } else {
            image.color = Color.clear;
            yield return GetComponent<CanvasGroup>().DOFade(1f, .5f);
            PlaySFX(cardData);
            yield return CoUtils.RunTween(image.DOColor(Color.white, .1f));
        }
        yield return InputManager.Instance.ConfirmRoutine();
        yield return GetComponent<CanvasGroup>().DOFade(0f, .7f);
    }

    private IEnumerator AudioRoutine(PortraitData cardData) {
        yield return CoUtils.Wait(.4f);
        PlaySFX(cardData);
    }

    private void PlaySFX(PortraitData cardData) {
        if (cardData.sfxName != null && cardData.sfxName.Length > 0) {
            Global.Instance.Audio.PlaySFX(cardData.sfxName);
        }
    }
}