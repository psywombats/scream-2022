using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(FieldSpritesheetComponent))]
[RequireComponent(typeof(MapEvent))]
[DisallowMultipleComponent]
public class CharaEvent : MonoBehaviour {

    public const float HitThick = .4f;
    private const float DesaturationDuration = 0.5f;
    private const float StepsPerSecond = 2.0f;

    [SerializeField] private bool directionFix;
    [SerializeField] public DollComponent doll;
    public SpriteRenderer Renderer => Doll.renderer;

    private Vector3 lastPosition;
    private Vector3 targetPx;
    private bool stepping;
    private bool wasSteppingLastFrame;
    private float moveTime;

    public MapEvent Event { get { return GetComponent<MapEvent>(); } }
    public Map Map { get { return Event.Map; } }
    public int StepCount => sprites.StepCount;

    public DollComponent Doll {
        get {
            if (doll == null) {
                doll = GetComponentInChildren<DollComponent>();
            }
            return doll;
        }
        set {
            doll = value;
        }
    }

    private bool overrideHide = false;
    public bool OverrideHide {
        get => overrideHide;
        set {
            overrideHide = value;
            UpdateEnabled(!overrideHide);
        }
    }

    private FieldSpritesheetComponent sprites;
    public FieldSpritesheetComponent Sprites {
        get {
            if (sprites == null) {
                sprites = GetComponent<FieldSpritesheetComponent>();
            }
            return sprites;
        }
    }

    [SerializeField] [HideInInspector] private OrthoDir _facing = OrthoDir.South;
    public OrthoDir Facing {
        get { return _facing; }
        set {
            if (_facing != value) {
                _facing = value;
                UpdateAppearance();
            }
        }
    }

    private List<SpriteRenderer> _renderers;
    protected List<SpriteRenderer> Renderers {
        get {
            if (_renderers == null) {
                _renderers = new List<SpriteRenderer> {
                    Renderer
                };
            }
            return _renderers;
        }
    }

    public void Start() {
        Doll.transform.localPosition = Vector3.zero;
        Doll.transform.rotation = Quaternion.identity;

        lastPosition = transform.position;

        GetComponent<Dispatch>().RegisterListener(MapEvent.EventEnabled, (object payload) => {
            UpdateEnabled((bool)payload);
        });
        GetComponent<Dispatch>().RegisterListener(MapEvent.EventInteract, (object payload) => {
            Facing = Event.DirectionTo(Global.Instance.Maps.Avatar.GetComponent<MapEvent>());
        });
        UpdateEnabled(Event.IsSwitchEnabled);
    }

    public void LateUpdate() {
        if (!Event.IsSwitchEnabled) {
            return;
        }
        if (!Event.IsTracking && Event != AvatarEvent.Instance.Event) {
            AutofaceDirection();
        }

        bool steppingThisFrame = IsSteppingThisFrame();
        stepping = steppingThisFrame || wasSteppingLastFrame;
        if (steppingThisFrame && !wasSteppingLastFrame) {
            moveTime += 1f / StepsPerSecond;
        } else if (!steppingThisFrame && !wasSteppingLastFrame) {
            moveTime = 0.0f;
        } else {
            moveTime += Time.deltaTime;
        }
        wasSteppingLastFrame = steppingThisFrame;
        lastPosition = transform.position;

        UpdateAppearance();
    }

    public void UpdateEnabled(bool enabled) {
        doll.collider.enabled = enabled;
        foreach (var renderer in Renderers) {
            renderer.enabled = enabled && !OverrideHide;
        }
        UpdateAppearance();
    }

    public void UpdateAppearance(bool fixedTime = false) {
        if (Renderer == null) {
            return;
        }
        Renderer.sprite = SpriteForMain(fixedTime);
    }

    public void FaceToward(MapEvent other) {
        if (directionFix) {
            return;
        }
        Facing = Event.DirectionTo(other);
    }

    public void SetTransparent(bool trans) {
        var propBlock = new MaterialPropertyBlock();
        foreach (SpriteRenderer renderer in Renderers) {
            renderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_Trans", trans ? 1 : 0);
            renderer.SetPropertyBlock(propBlock);
        }
    }

    public void SetAppearance(SpritesheetData sprite) {
        sprites.Set(sprite);
        UpdateAppearance();
    }

    public void SetAppearanceByTag(string fieldSpriteTag) {
        Sprites.SetByTag(fieldSpriteTag);
        UpdateAppearance();
    }

    public IEnumerator StepRoutine(OrthoDir dir) {
        throw new NotImplementedException();
    }

    public IEnumerator FadeRoutine(float duration, bool inverse = false) {
        float val = inverse ? 1.0f : 0.0f;
        yield return CoUtils.RunTween(Renderer.DOColor(new Color(val, val, val), duration));
    }

    private Sprite SpriteForMain(bool fixedTime) {
        if (!fixedTime) {
            var x = (Mathf.FloorToInt(moveTime * StepsPerSecond) + 1) % Sprites.StepCount;
            return Sprites.GetFrame(DirectionRelativeToCamera(), x);
        } else {
            return Sprites.GetFrame(DirectionRelativeToCamera(), 1);
        }
        
    }

    public bool CanCrossTileGradient(Vector2Int from, Vector2Int to) {
        float fromHeight = Map.Terrain.HeightAt(from);
        float toHeight = GetComponent<MapEvent>().Map.Terrain.HeightAt(to);
        return Mathf.Abs(fromHeight - toHeight) < 1.0f && toHeight > 0.0f;
        //if (fromHeight < toHeight) {
        //    return toHeight - fromHeight <= unit.GetMaxAscent();
        //} else {
        //    return fromHeight - toHeight <= unit.GetMaxDescent();
        //}
    }

    private OrthoDir DirectionRelativeToCamera() {
        MapCamera cam = Application.isPlaying ? MapManager.Instance.Camera : FindObjectOfType<MapCamera>();
        if (!cam || !Application.isPlaying || !AvatarEvent.Instance.UseFirstPersonControl) {
            return Facing;
        }

        var pos = transform.position + new Vector3(.5f, 0, .5f);
        Vector3 ourScreen = cam.GetCameraComponent().WorldToScreenPoint(pos);
        Vector3 targetWorld = pos + Facing.Px3D() * .3f;
        targetWorld.y = pos.y;
        Vector3 targetScreen = cam.GetCameraComponent().WorldToScreenPoint(targetWorld);
        Vector3 delta = targetScreen - ourScreen;
        return OrthoDirExtensions.DirectionOf2D(new Vector2(delta.x, -delta.y));
    }

    private bool IsSteppingThisFrame() {
        var ava = GetComponent<AvatarEvent>();
        if (ava != null) return Event.IsTracking || ava.WantsToTrack();
        var position = transform.position;
        position.y = 0;
        var old = lastPosition;
        old.y = 0;
        var delta = position - old;
        return (delta.sqrMagnitude > 0.05 * Time.deltaTime && delta.sqrMagnitude < Map.UnitsPerTile)  
            || Event.IsTracking;
    }

    private void AutofaceDirection() {
        var delta = transform.position - lastPosition;
        delta.y = 0f;
        if (delta.sqrMagnitude / Time.deltaTime < .05) {
            return;
        }
        var xcom = new Vector3(delta.x, 0, 0);
        if (xcom != Vector3.zero && OrthoDirExtensions.DirectionOf3D(xcom) == Facing) {
            return;
        }
        var zcom = new Vector3(0, 0, delta.z);
        if (zcom != Vector3.zero && OrthoDirExtensions.DirectionOf3D(zcom) == Facing) {
            return;
        }
        Facing = OrthoDirExtensions.DirectionOf3D(delta);
    }
}
