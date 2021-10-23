using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeakboxComponent : TextAutotyper {

    private static readonly string SystemSpeaker = "SYSTEM";

    [SerializeField] private float animationSeconds = 0.2f;
    [SerializeField] private float moveDuration = .4f;
    [Space]
    [SerializeField] private TextMeshProUGUI namebox = null;
    [SerializeField] private RectTransform mainBox = null;
    [SerializeField] private Image portraitImage = null;
    [SerializeField] private RectTransform tailTrans = null;
    [SerializeField] private RectTransform topTailTrans = null;
    [SerializeField] private RectTransform boxTrans = null;
    [SerializeField] private RectTransform portraitTrans = null;
    [Space]
    [SerializeField] private Vector2 margin = new Vector2(340, 48);
    [SerializeField] private Vector2 tailMargin = new Vector2(64, 48);
    [Space]
    [SerializeField] private GameObject selectionArea = null;
    [SerializeField] private ListSelector choiceSelector = null;
    [SerializeField] private SelectableCell cellA = null;
    [SerializeField] private SelectableCell cellB = null;

    public bool isDisplaying { get; private set; }

    private Vector3 pos = Vector3.zero;
    private PortraitData portrait = null;

    protected override void Start() {
        base.Start();
        textbox.text = "";
        GetComponent<CanvasGroup>().alpha = 0f;
        cellA.onSelectionChange += Cell_onSelectionChange;
        cellB.onSelectionChange += Cell_onSelectionChange;
    }

    private void Cell_onSelectionChange(SelectableCell cell, bool enabled) {
        var choice = cell.GetComponent<ChoiceCell>();
        choice.meshA.gameObject.SetActive(enabled);
        choice.meshB.gameObject.SetActive(!enabled);
    }

    public static Vector3 ViewportToCanvasPosition(Canvas canvas, Vector3 viewportPosition) {
        var centerBasedViewPortPosition = viewportPosition;
        var canvasRect = canvas.GetComponent<RectTransform>();
        var scale = canvasRect.sizeDelta;
        return Vector3.Scale(centerBasedViewPortPosition, scale);
    }

    public static Vector3 ScreenToCanvasPosition(Canvas canvas, Vector3 screenPosition) {
        var viewportPosition = new Vector3(screenPosition.x / Screen.width,
                                           screenPosition.y / Screen.height,
                                           0);
        return ViewportToCanvasPosition(canvas, viewportPosition);
    }

    public IEnumerator SetupForPos(Vector3 pos, float duration, bool useTail = true, bool bottom = false) {
        var screen = MapManager.Instance.Camera.GetCameraComponent().WorldToScreenPoint(pos);
        screen = ScreenToCanvasPosition(transform.parent.GetComponent<Canvas>(), screen);
        screen += new Vector3(0, 88f, 0);
        var rect = GetComponent<RectTransform>();

        if (pos == Vector3.zero || bottom) {
            screen = new Vector2(640, 72);
        }
        var boxX = Mathf.Clamp(screen.x, margin.x, 1280 - margin.x);
        var boxY = Mathf.Clamp(screen.y, margin.y, 720 - margin.y * 2.5f);
        var tailX = Mathf.Clamp(screen.x, tailMargin.x, 1280 - tailMargin.x);
        var tailY = Mathf.Clamp(screen.y, tailMargin.y, 720 - margin.y * 2.5f);

        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(boxTrans.DOAnchorPos(new Vector2(boxX, boxY), duration)),
            CoUtils.RunTween(tailTrans.DOAnchorPos(new Vector2(tailX, tailY), duration)),
            CoUtils.RunTween(topTailTrans.DOAnchorPos(new Vector2(tailX, tailY), duration)),
            CoUtils.RunTween(tailTrans.GetComponent<CanvasGroup>().DOFade(((screen.y > 96) && useTail) ? 1 : 0, duration)),
            CoUtils.RunTween(topTailTrans.GetComponent<CanvasGroup>().DOFade(((screen.y <= 96) && useTail) ? 1 : 0, duration)),
        }, this);
    }

    public void MarkHiding() {
        isDisplaying = false;
    }

    public IEnumerator TestRoutine() {
        isDisplaying = true;
        while (true) {
            yield return DisableRoutine();
            yield return CoUtils.Wait(1.0f);
            yield return SpeakRoutine("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do " +
                "eiusmod tempor incididunt ut labore et dolore magna aliqua.");
            yield return SpeakRoutine("Diaghilev", "Or thus spake the ancestors.", new Vector3(5, 4, 5));
            yield return SpeakRoutine("Diaghilev", "Etc.", new Vector3(5, 4, 5));
            yield return SpeakRoutine("Homasa", "Hello I am someone completely different", new Vector3(8, 4, 6));
            yield return SpeakRoutine("eom");
        }
    }

    public async Task<int> ChooseAsync(string choiceA, string choiceB) {
        if (!isDisplaying) {
            namebox.text = "";
            await SetupForPos(AvatarEvent.Instance.Event.GetTextPos(), 0f, false);
            await EnableRoutine();
        } else {
            await CoUtils.RunParallel(new IEnumerator[] {
                    EraseNameRoutine(animationSeconds / 2.0f),
                    EraseTextRoutine(animationSeconds / 2.0f),
                    SetupForPos(AvatarEvent.Instance.Event.GetTextPos(), moveDuration, false)
                }, this);
        }
        pos = new Vector3(-1, -1, -1); // dummy to always check false
        selectionArea.SetActive(true);
        cellA.GetComponent<ChoiceCell>().Populate(choiceA);
        cellB.GetComponent<ChoiceCell>().Populate(choiceB);
        var result = await choiceSelector.SelectItemAsync();
        selectionArea.SetActive(false);
        return result;
    }

    public IEnumerator SpeakRoutine(string text, Vector3 worldPos) => SpeakRoutine(SystemSpeaker, text, worldPos);
    public IEnumerator SpeakRoutine(string text) => SpeakRoutine(SystemSpeaker, text, Vector3.zero);
    public IEnumerator SpeakRoutine(string speakerName, string text) => SpeakRoutine(speakerName, text, Vector3.zero);
    public IEnumerator SpeakRoutine(string speakerName, string text, Vector3 worldPos, PortraitData portrait = null, bool useTail = true, bool bottom = false) {
        if (text == null || text.Length == 0) {
            text = speakerName;
            speakerName = SystemSpeaker;
        }
        if (speakerName == SystemSpeaker) {
            SetNameboxEnabled(false);
        }
        useTail &= speakerName != SystemSpeaker;
        if (!isDisplaying) {
            pos = worldPos;
            yield return SetupForPos(pos, 0f, useTail, bottom);
            if (speakerName == SystemSpeaker) {
                SetNameboxEnabled(false);
            }
            namebox.text = speakerName;
            namebox.GetComponent<CanvasGroup>().alpha = 1f;

            if (speakerName != SystemSpeaker) {
                SetNameboxEnabled(true);
            }
            
            StartCoroutine(HandlePortraitRoutine(portrait, 0f));

            yield return EnableRoutine();
        } else {
            StartCoroutine(HandlePortraitRoutine(portrait, animationSeconds / 2f));
            if (pos != worldPos) {
                pos = worldPos;
                yield return CoUtils.RunParallel(new IEnumerator[] {
                    EraseNameRoutine(animationSeconds / 2.0f),
                    EraseTextRoutine(animationSeconds / 2.0f),
                }, this);
                yield return SetupForPos(pos, moveDuration, useTail, bottom);
            } else {
                yield return EraseTextRoutine(animationSeconds / 2.0f);
            }
            if (namebox.text != speakerName && speakerName != SystemSpeaker) {
                namebox.GetComponent<CanvasGroup>().alpha = 1f;
                namebox.text = speakerName;
            }
        }
        if (speakerName == SystemSpeaker) {
            namebox.text = "";
        }

        namebox.GetComponent<CanvasGroup>().alpha = 1f;
        textbox.GetComponent<CanvasGroup>().alpha = 1f;
        if ((speakerName == "Tess" || speakerName == "???") && !Global.Instance.Data.GetSwitch("spoken_lines")) {
            AudioManager.Instance.PlaySFX("talk_MC");
        } else {
            AudioManager.Instance.PlaySFX("talk_NPC");
        }
        yield return TypeRoutine(text);
    }

    private IEnumerator EnableRoutine() {
        isDisplaying = true;
        Global.Instance.Input.PushListener(this);

        yield return CoUtils.RunParallel(new IEnumerator[] {
            ShowMainBoxRoutine(animationSeconds),
        }, this);
    }
    public IEnumerator DisableRoutine() {
        isDisplaying = false;
        yield return CoUtils.RunParallel(new IEnumerator[] {
            EraseNameRoutine(animationSeconds / 2.0f),
            EraseTextRoutine(animationSeconds / 2.0f),
            CloseMainBoxRoutine(animationSeconds),
        }, this);
        SetupPortrait(null);
        Global.Instance.Input.RemoveListener(this);
    }

    protected virtual void SetNameboxEnabled(bool enabled) {
        namebox.enabled = enabled;
    }
    protected virtual bool IsNameboxEnabled() {
        return namebox.enabled;
    }

    protected virtual IEnumerator EraseTextRoutine(float seconds) {
        yield return CoUtils.RunTween(textbox.GetComponent<CanvasGroup>().DOFade(0.0f, seconds));
    }
    protected virtual IEnumerator EraseNameRoutine(float seconds) {
        yield return CoUtils.RunTween(namebox.GetComponent<CanvasGroup>().DOFade(0.0f, seconds));
    }

    protected virtual IEnumerator ShowMainBoxRoutine(float seconds) {
        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(GetComponent<CanvasGroup>().DOFade(1f, seconds)),
            CoUtils.RunTween(mainBox.DOLocalMoveY(mainBox.localPosition.y + 12f, seconds, true))
        }, this);
    }
    protected virtual IEnumerator CloseMainBoxRoutine(float seconds) {
        yield return CoUtils.RunParallel(new IEnumerator[] {
            CoUtils.RunTween(GetComponent<CanvasGroup>().DOFade(0f, seconds)),
            CoUtils.RunTween(mainBox.DOLocalMoveY(mainBox.localPosition.y - 12f, seconds, true))
        }, this);
    }

    private IEnumerator HandlePortraitRoutine(PortraitData data, float duration) {
        if (data != portrait) {
            if (portrait != null) {
                yield return CoUtils.RunTween(portraitTrans.GetComponent<CanvasGroup>().DOFade(0f, duration));
            }
            portraitTrans.GetComponent<CanvasGroup>().alpha = 0f;
            SetupPortrait(data);
            if (data != null) {
                yield return CoUtils.RunTween(portraitTrans.GetComponent<CanvasGroup>().DOFade(1f, duration));
            }
        }
    }

    private void SetupPortrait(PortraitData data) {
        portrait = data;
        if (data != null) {
            portraitImage.sprite = data.portrait;
        } else {
            portraitTrans.GetComponent<CanvasGroup>().alpha = 0f;
        }
        portraitTrans.anchoredPosition = new Vector2(0, data == null ? 0 : data.offset);
        portraitImage.SetNativeSize();
    }
}