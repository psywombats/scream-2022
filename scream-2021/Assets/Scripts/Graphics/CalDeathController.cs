using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalDeathController : MonoBehaviour {

    public CanvasGroup subcan = null;
    public GameObject eyeParent = null;
    public List<Sprite> sprites = null;
    public Image img = null;
    public Material transMat = null;


    public IEnumerator ShowRoutine(int version) {
        gameObject.SetActive(true);
        var can = GetComponent<CanvasGroup>();
        can.alpha = 0f;
        if (version == 0) {
            AudioManager.Instance.PlaySFX("madness");
            yield return CoUtils.RunTween(can.DOFade(1f, .1f));
            yield return CoUtils.Wait(2f);
            yield return can.DOFade(0f, 1.5f);
        } else {
            eyeParent.gameObject.SetActive(false);
            yield return CoUtils.RunTween(can.DOFade(1f, .1f));
            img.sprite = sprites[0];
            subcan.alpha = 1f;
            AudioManager.Instance.PlaySFX("madness");
            yield return CoUtils.Wait(.8f);
            yield return CoUtils.RunTween(subcan.DOFade(1f, .1f));
            for (var i = 1; i < sprites.Count; i += 1) {
                img.material = transMat;
                yield return CoUtils.Wait(.08f);
                img.sprite = sprites[i];
                AudioManager.Instance.PlaySFX("madness");
                yield return CoUtils.Wait(.08f);
                img.material = null;
                yield return CoUtils.Wait(.8f);
            }


            yield return CoUtils.RunTween(can.DOFade(0f, 1.5f));
            subcan.alpha = 0f;
        }
        gameObject.SetActive(false);
    }
}