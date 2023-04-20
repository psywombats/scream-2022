using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

[RequireComponent(typeof(MapEvent))]
public class CharaEvent : MonoBehaviour {

    private const float TargetHeight = 1.8f;
    private const float HighlightSpeed = 2f;

    [SerializeField] public DollComponent doll;
    [SerializeField] private SpeakerData speaker;
    [SerializeField] private bool phaseIn;
    [SerializeField] private OrthoDir dir = OrthoDir.South;

    private MapEvent @event;
    public MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private bool highlightNow;

    public void Start() {
        UpdateRenderer();

        if (phaseIn) {
            PhaseIn();
        }
        SetFacing(dir);
    }

    private async void PhaseIn() {
        doll.offsetter.transform.localScale = new Vector3(0, 0, 1);
        await Task.Delay(1500);
        doll.offsetter.transform.DOScaleY(1f, 1.5f).SetEase(Ease.OutBounce).Play();
        doll.offsetter.transform.DOScaleX(1f, 1.5f).SetEase(Ease.OutCubic).Play();
    }

    public void Update() {
        var a = doll.renderer.color.a;
        var targetA = highlightNow ? 0f : 1f;
        var delta = HighlightSpeed * Time.deltaTime;
        if (a < targetA) delta *= -1;
        a += delta;
        if (delta < 0 && a < targetA) a = targetA;
        if (delta > 0 && a > targetA) a = targetA;

        //doll.renderer.color = new Color(
        //    doll.renderer.color.r,
        //    doll.renderer.color.g,
        //   doll.renderer.color.b,
        //    a);
        doll.highlightRenderer.color = new Color(
            doll.highlightRenderer.color.r,
            doll.highlightRenderer.color.g,
            doll.highlightRenderer.color.b,
            1f - a);

        highlightNow = false;
    }

    public void SetFacing(OrthoDir dirr) {
        doll.offsetter.transform.localEulerAngles = new Vector3(0, dirr.Rot3D(), 0);
    }

    public void OnValidate() {
        UpdateRenderer();
        SetFacing(dir);
    }

    public void Interact() => Event.Interact();

    public void UpdateRenderer() {
        if (speaker != null) {
            var factor = TargetHeight / speaker.image.rect.height * speaker.image.pixelsPerUnit;
            doll.renderer.transform.localScale = new Vector3(factor, factor, factor);
            doll.highlightRenderer.transform.localScale = new Vector3(factor, factor, factor);
            doll.renderer.sprite = speaker.image;
            doll.highlightRenderer.sprite = speaker.glow;
        } else {
            doll.renderer.sprite = null;
            doll.highlightRenderer.sprite = null;
        }
    }

    public void HandleRay() {
        highlightNow = true;
    }
}
