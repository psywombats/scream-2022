using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TitleController : MonoBehaviour {

    [SerializeField] private IntertitleController intertitle = null;
    [SerializeField] private IntertitleController intertitle2 = null;
    [SerializeField] private ListSelector selector = null;
    [SerializeField] private CanvasGroup canvas = null;
    [SerializeField] private GameObject canvasParent = null;

    public void Start() {
        intertitle.Autostart();
        DoMenuAsync();
    }

    private async void DoMenuAsync() {
        var selection = await selector.SelectItemAsync();
        switch (selection) {
            case 0:
                StartGame();
                break;
            case 1:
            case 2:
                ExitGame();
                break;
            default:
                break;
        }
    }

    private void StartGame() {
        Global.Instance.StartCoroutine(StartRoutine());
    }

    //private async Task<bool> ContinueGameAsync() {
    //    selector.gameObject.SetActive(false);
    //    return false;
    //}

    private void ExitGame() {
        Application.Quit();
    }

    private IEnumerator StartRoutine() {
        yield return intertitle.FadeOutRoutine();
        yield return CoUtils.RunTween(canvas.DOFade(0f, 1.5f));
        canvasParent.SetActive(false);
        yield return intertitle2.DisplayRoutine(
            "NO EVENT CAN BE WITHOUT\n" +
            "A BEGINNING\n\n" +
            "THIS IS THE IMMUTABLE LAW\n" +
            "OF CAUSALITY\n\n\n\n" +
            "DAY_1");
        yield return Global.Instance.Serialization.StartGameRoutine("RoomYours", "start");
        MapOverlayUI.Instance.Setting.Show("Allsaints' Pediatric Hospital");
        MapOverlayUI.Instance.Setting.Show("Ward #6");
        MapOverlayUI.Instance.Setting.Show("Room 604");

    }
}
