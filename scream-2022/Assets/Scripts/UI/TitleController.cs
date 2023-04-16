using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

    [SerializeField] private IntertitleController intertitle = null;
    [SerializeField] private IntertitleController intertitle2 = null;
    [SerializeField] private ListSelector selector = null;
    [SerializeField] private CanvasGroup canvas = null;
    [SerializeField] private CanvasGroup continueGroup = null;
    [SerializeField] private Text continueText = null;
    [SerializeField] private GameObject canvasParent = null;
    [SerializeField] private ControlController controls = null;

    private static readonly string[] Continues = { "DAY_1", "NIGHT_1", "DAY_2", "NIGHT_2", "DAY_3", "NIGHT_3", "DAY_4", "TWILIGHT" };
    private int continueIndex = 0;

    public void Start() {
        intertitle.Autostart();
        DoMenu();
        StartCoroutine(CoUtils.Delay(2.5f, controls.Trigger(controls.set1)));
    }

    private async void DoMenu() { await DoMenuAsync(); }
    private async Task DoMenuAsync() {
        var selection = await selector.SelectItemAsync();
        switch (selection) {
            case 0:
                StartGame();
                break;
            case 1:
                var result = await ContinueGameAsync();
                InputManager.Instance.RemoveListener("continue");
                if (!result) {
                    await CoUtils.RunTween(continueGroup.DOFade(0f, .25f));
                    await CoUtils.RunTween(selector.GetComponent<CanvasGroup>().DOFade(1f, .25f));
                    selector.Selection = 1;
                    await DoMenuAsync();
                }
                break;
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

    private async Task<bool> ContinueGameAsync() {
        continueText.text = Continues[continueIndex];
        await CoUtils.RunTween(selector.GetComponent<CanvasGroup>().DOFade(0f, .25f));
        await CoUtils.RunTween(continueGroup.DOFade(1f, .25f));
        var comp = new TaskCompletionSource<bool>();
        InputManager.Instance.PushListener("continue", (cmd, ev) => {
            if (ev != InputManager.Event.Down) {
                return true;
            }
            if (cmd == InputManager.Command.Left || cmd == InputManager.Command.StrafeLeft) {
                AudioManager.Instance.PlaySFX("cursor");
                continueIndex -= 1;
            }
            if (cmd == InputManager.Command.Right || cmd == InputManager.Command.StrafeRight) {
                AudioManager.Instance.PlaySFX("cursor");
                continueIndex += 1;
            }
            if (continueIndex < 0) continueIndex = Continues.Length - 1;
            if (continueIndex >= Continues.Length) continueIndex = 0;
            continueText.text = Continues[continueIndex];
            if (cmd == InputManager.Command.Cancel) {
                comp.SetResult(false);
            }

            if (cmd == InputManager.Command.Confirm) {
                AudioManager.Instance.PlaySFX("selection");
                Global.Instance.StartCoroutine(ContinueRoutine());
                comp.SetResult(true);
            }

            return true;
        });
        return await comp.Task;
    }

    private void ExitGame() {
        Application.Quit();
    }

    private IEnumerator ContinueRoutine() {
        StartCoroutine(AudioManager.Instance.FadeOutRoutine(1));
        yield return FadeOutRoutine();
        ContinueSwitches.Activate(continueIndex);
        if (continueIndex == Continues.Length - 1) {
            yield return Global.Instance.Serialization.StartGameRoutine("Office1", "target");
        } else {
            yield return Global.Instance.Serialization.StartGameRoutine("RoomYours", "start");
        }
        
    }

    private IEnumerator StartRoutine() {
        yield return FadeOutRoutine();
        yield return intertitle2.DisplayRoutine(
            "NO EVENT CAN BE WITHOUT\n" +
            "A BEGINNING\n\n" +
            "THIS IS THE IMMUTABLE LAW\n" +
            "OF CAUSALITY\n\n\n\n" +
            "DAY_1");
        yield return AudioManager.Instance.FadeOutRoutine(.5f);
        yield return Global.Instance.Serialization.StartGameRoutine("RoomYours", "start");
        MapOverlayUI.Instance.Setting.Show("Allsaints' Pediatric Hospital");
        MapOverlayUI.Instance.Setting.Show("Ward #6");
        MapOverlayUI.Instance.Setting.Show("Room 604");

    }

    private IEnumerator FadeOutRoutine() {
        yield return intertitle.FadeOutRoutine();
        yield return CoUtils.RunTween(canvas.DOFade(0f, 1.5f));
        canvasParent.SetActive(false);
    }
}
