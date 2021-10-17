using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class KeywordController : MonoBehaviour {

    [SerializeField] private Vector2Int minStartPos;
    [SerializeField] private Vector2Int maxStartPos;
    [SerializeField] private Vector2 maxVelocity = new Vector2(100, 50);
    [SerializeField] private float maxScale = 3f;
    [SerializeField] private float durationBase = 1f;
    [Space]
    [SerializeField] private KeywordComponent prefab;

    private List<KeywordComponent> controllers = new List<KeywordComponent>();

    public IEnumerator ShowRoutine(string[] keywords) {
        foreach (var controller in controllers) {
            Destroy(controller.gameObject);
        }

        yield return CoUtils.RunTween(GetComponent<CanvasGroup>().DOFade(1f, .8f));
        
        foreach (var word in keywords) {
            var controller = Instantiate(prefab);
            controller.transform.SetParent(transform);
            controllers.Add(controller);
            controller.GetComponent<TextMeshProUGUI>().text = word;
            var rect = controller.GetComponent<RectTransform>();
            var canvas = controller.GetComponent<CanvasGroup>();
            rect.anchoredPosition = new Vector2Int(
                Random.Range(minStartPos.x, maxStartPos.x),
                Random.Range(minStartPos.y, maxStartPos.y));
            controller.Velocity = new Vector2(
                Random.Range(-.2f * maxVelocity.x, maxVelocity.x) * ((rect.anchoredPosition.x > (minStartPos.x + maxStartPos.x) / 2) ? -1 : 1),
                Random.Range(-.2f * maxVelocity.y, maxVelocity.y) * ((rect.anchoredPosition.y > (minStartPos.y + maxStartPos.y) / 2) ? -1 : 1));
            canvas.alpha = 0f;
            StartCoroutine(CoUtils.RunTween(canvas.DOFade(1f, durationBase)));
            StartCoroutine(CoUtils.RunTween(canvas.transform.DOScale(maxScale, durationBase * 3f).SetEase(Ease.OutCubic)));
            yield return CoUtils.Wait(durationBase / 3f);
        }
        yield return CoUtils.Wait(durationBase * 3f);

        yield return CoUtils.RunTween(GetComponent<CanvasGroup>().DOFade(0f, 1.5f));
    }
}
