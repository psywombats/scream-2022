using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

    [SerializeField] private ListSelector selector = null;
    [SerializeField] private CanvasGroup canvas = null;
    [SerializeField] private CanvasGroup continueGroup = null;
    [SerializeField] private Text continueText = null;
    [SerializeField] private GameObject canvasParent = null;

    private static readonly string[] Continues = { "March 1 11:00AM", "March 1 3:00PM", "March 1 10:00PM", "MIDNIGHT", "March 2 5:00PM", "March 2 9:00PM", "TWILIGHT" };
    private int continueIndex = 0;

    public void Start() {
        DoMenu();
    }

    private async void DoMenu() { await DoMenuAsync(); }
    private async Task DoMenuAsync() {
        var selection = await selector.SelectItemAsync();
        switch (selection) {
            case 0:
                StartGame();
                break;
            case 1:
                ChooseBookmark();
                break;
            case 2:
                ExitGame();
                break;
            default:
                break;
        }
    }

    public void StartGame() {
        Global.Instance.StartCoroutine(StartRoutine());
    }

    public async void ChooseBookmark() {
        continueText.text = Continues[continueIndex];
        await CoUtils.RunTween(selector.GetComponent<CanvasGroup>().DOFade(0f, .25f));
        await CoUtils.RunTween(continueGroup.DOFade(1f, .25f));
        InputManager.Instance.PushListener("continue", (cmd, ev) => {
            if (ev != InputManager.Event.Down) {
                return true;
            }
            if (cmd == InputManager.Command.Left) {
                AudioManager.Instance.PlaySFX("cursor");
                continueIndex -= 1;
            }
            if (cmd == InputManager.Command.Right) {
                AudioManager.Instance.PlaySFX("cursor");
                continueIndex += 1;
            }
            if (continueIndex < 0) continueIndex = Continues.Length - 1;
            if (continueIndex >= Continues.Length) continueIndex = 0;
            continueText.text = Continues[continueIndex];
            if (cmd == InputManager.Command.Secondary || cmd == InputManager.Command.Menu) {
                Return();
            }

            if (cmd == InputManager.Command.Primary) {
                AudioManager.Instance.PlaySFX("selection");
                Continue();
            }

            return true;
        });
    }

    public async void Return() {
        await CoUtils.RunTween(continueGroup.DOFade(0f, .25f));
        await CoUtils.RunTween(selector.GetComponent<CanvasGroup>().DOFade(1f, .25f));
        selector.Selection = 1;
        await DoMenuAsync();
    }

    public void ExitGame() {
        Application.Quit();
    }
    
    public void Continue() {
        Global.Instance.StartCoroutine(ContinueRoutine());
    }
    private IEnumerator ContinueRoutine() {
        StartCoroutine(AudioManager.Instance.FadeOutRoutine(1));
        yield return FadeOutRoutine();
        ContinueSwitches.Activate(continueIndex);
        switch (continueIndex) {
            case 0:
                yield return Global.Instance.Serialization.StartGameRoutine("F2", "pt1target", OrthoDir.North);
                break;
            case 1:
                yield return Global.Instance.Serialization.StartGameRoutine("F2", "gazer", OrthoDir.North);
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
