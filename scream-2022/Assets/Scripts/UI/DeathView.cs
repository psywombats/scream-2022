using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DeathView : MonoBehaviour {

    [SerializeField] private CanvasGroup mainCanvas;
    [SerializeField] private List<CanvasGroup> lines;
    [SerializeField] private RectTransform shaker;
    [SerializeField] private GameObject inversion;

    public IEnumerator RunRoutine() {
        gameObject.SetActive(true);
        yield return CoUtils.RunTween(mainCanvas.DOFade(1f, 3f));
        yield return CoUtils.Wait(.5f);

        foreach (var line in lines) {
            yield return CoUtils.Wait(.5f);
            yield return CoUtils.RunTween(line.DOFade(1f, 1f));
        }

        yield return CoUtils.Wait(1.3f);
        FindObjectOfType<MonsterController>().gameObject.SetActive(false);
        AudioManager.Instance.StopSFX();
        AudioManager.Instance.PlaySFX("squish");
        AudioManager.Instance.PlayBGM("none");
        inversion.gameObject.SetActive(true);
        yield return CoUtils.RunTween(shaker.DOShakeAnchorPos(.3f, vibrato: 50));

        yield return CoUtils.Wait(4f);
    }
}
