using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TitleController : MonoBehaviour {

    [SerializeField] private IntertitleController intertitle = null;
    [SerializeField] private IntertitleController intertitle2 = null;
    [SerializeField] private ListSelector selector = null;
    [SerializeField] private CanvasGroup canvas = null;

    public void Start() {
        intertitle.Autostart();
        DoMenuAsync();
    }

    private async void DoMenuAsync() {
        while (true) {
            var selection = await selector.SelectItemAsync();
            switch (selection) {
                case 0:
                    StartGame();
                    break;
                case 1:
                    var continued = await ContinueGameAsync();
                    if (continued) {
                        break;
                    } else {
                        selector.Selection = 1;
                        continue;
                    }
                case 2:
                    ExitGame();
                    break;
                default:
                    continue;
            }
        }
    }

    private void StartGame() {
        StartCoroutine(StartRoutine());
    }

    private async Task<bool> ContinueGameAsync() {
        selector.gameObject.SetActive(false);
        return false;
    }

    private void ExitGame() {
        Application.Quit();
    }

    private IEnumerator StartRoutine() {
        yield return CoUtils.RunParallel(this,
            CoUtils.RunTween(canvas.DOFade(0f, 2f)),
            intertitle.FadeOutRoutine());
        yield return intertitle2.DisplayRoutine(
            "NO EVENT CAN BE WITHOUT\n" +
            "A BEGINNING\n\n" +
            "THIS IS THE IMMUTABLE LAW\n" +
            "OF CAUSALITY\n\n\n\n" +
            "DAY_1");
        yield return MapManager.Instance.TeleportRoutine("RoomYours", "start", OrthoDir.South);

    }
}
