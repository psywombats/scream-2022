using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TitleController : MonoBehaviour {
    
    [SerializeField] private CanvasGroup canvas = null;
    [SerializeField] private CanvasGroup continueGroup = null;
    [SerializeField] private CanvasGroup mainGroup = null;
    [SerializeField] private GameObject canvasParent = null;

    private static readonly string[] Continues = { "March 1 11:00AM", "March 1 4:00PM", "March 1 9:00PM", "MIDNIGHT", "March 2 5:00PM", "March 2 10:00PM", "RECKONING" };

    public void StartGame() {
        Global.Instance.StartCoroutine(StartRoutine());
    }

    public async void ShowBookmarks() {
        continueGroup.gameObject.SetActive(true);
        await CoUtils.RunTween(mainGroup.DOFade(0f, .5f));
        await CoUtils.RunTween(continueGroup.DOFade(1f, .5f));
        mainGroup.gameObject.SetActive(false);
        
    }

    public async void HideBookmarks() {
        mainGroup.gameObject.SetActive(true);
        await CoUtils.RunTween(continueGroup.DOFade(0f, .5f));
        await CoUtils.RunTween(mainGroup.DOFade(1f, .5f));
        continueGroup.gameObject.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }
    
    public void ResumeBookmark(int mark) {
        Global.Instance.StartCoroutine(ResumeBookmarkRoutine(mark));
    }
    private IEnumerator ResumeBookmarkRoutine(int mark) {
        StartCoroutine(AudioManager.Instance.FadeOutRoutine(1));
        yield return FadeOutRoutine();
        ContinueSwitches.Activate(mark);
        switch (mark) {
            case 0:
                yield return Global.Instance.Serialization.StartGameRoutine("F2", "pt1target", OrthoDir.North);
                break;
            case 1:
                yield return Global.Instance.Serialization.StartGameRoutine("F2", "gazer", OrthoDir.North);
                break;
            case 2:
                yield return Global.Instance.Serialization.StartGameRoutine("Gazer", "chair", OrthoDir.South);
                break;
            case 3:
                yield return Global.Instance.Serialization.StartGameRoutine("F3", "elevator2", OrthoDir.East);
                break;
            case 4:
                yield return Global.Instance.Serialization.StartGameRoutine("Gazer", "chair", OrthoDir.South);
                break;
            case 5:
                yield return Global.Instance.Serialization.StartGameRoutine("F2", "noemi", OrthoDir.West);
                break;
            case 6:
                yield return Global.Instance.Serialization.StartGameRoutine("Gazer", "door", OrthoDir.North);
                break;
        }
    }

    private IEnumerator StartRoutine() {
        yield return FadeOutRoutine();
        yield return AudioManager.Instance.FadeOutRoutine(.5f);
        yield return Global.Instance.Serialization.StartGameRoutine("Corridor", "start", OrthoDir.West);
    }

    private IEnumerator FadeOutRoutine() {
        yield return CoUtils.RunTween(canvas.DOFade(0f, 1.5f));
        canvasParent.SetActive(false);
    }
}
