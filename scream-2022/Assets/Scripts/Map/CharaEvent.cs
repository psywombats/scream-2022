using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

[RequireComponent(typeof(MapEvent))]
public class CharaEvent : MonoBehaviour {

    private const float TargetHeight = 2.2f;
    private const float HighlightSpeed = 2f;

    [SerializeField] public DollComponent doll;
    [SerializeField] private SpeakerData speaker;
    [SerializeField] private bool phaseIn;
    [SerializeField] private float phaserDelay = .8f;
    [SerializeField] public OrthoDir dir = OrthoDir.South;

    private MapEvent @event;
    public MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private bool highlightNow;

    public void Start() {
        UpdateRenderer();

        if (phaseIn) {
            PhaseIn();
        }
        SetFacing(dir);
        Event.enableChild = doll.gameObject;
        doll.gameObject.SetActive(Event.IsSwitchEnabled);
    }

    private async void PhaseIn() {
        doll.offsetter.transform.localScale = new Vector3(0, 0, 1);
        await Task.Delay((int)(phaserDelay * 1000));
        doll.offsetter.transform.DOScaleY(1f, 1.5f).SetEase(Ease.OutBounce).Play();
        doll.offsetter.transform.DOScaleX(1f, 1.5f).SetEase(Ease.OutCubic).Play();
    }

    public void Update() {
        var a = doll.highlightRenderer.color.a;
        var targetA = highlightNow ? 1f : 0f;
        var delta = HighlightSpeed * Time.deltaTime;
        if (a < targetA) delta *= -1;
        a += delta;
        if (delta < 0 && a < targetA) a = targetA;
        if (delta > 0 && a > targetA) a = targetA;
        
        doll.highlightRenderer.color = new Color(
            doll.highlightRenderer.color.r,
            doll.highlightRenderer.color.g,
            doll.highlightRenderer.color.b,
            a);

        if (!AvatarEvent.Instance.InputPaused) {
            highlightNow = false;
        }
    }

    public void SetFacing(OrthoDir dir) {
        if (doll != null) {
            doll.offsetter.transform.localEulerAngles = new Vector3(0, dir.Rot3D(), 0);
        }
    }

    public void OnValidate() {
        UpdateRenderer();
        SetFacing(dir);
    }

    public void Interact() => Event.Interact();

    public void UpdateRenderer() {
        if (speaker != null) {
            var factor = TargetHeight / speaker.image.rect.height * speaker.image.pixelsPerUnit;
            foreach (var renderer in doll.renderers) {
                renderer.transform.localScale = new Vector3(factor, factor, factor);
                renderer.sprite = speaker.image;
            }
            doll.highlightRenderer.transform.localScale = new Vector3(factor, factor, factor);
            doll.highlightRenderer.sprite = speaker.glow;
        } 
    }

    public void HandleRay() {
        highlightNow = true;
    }
}
