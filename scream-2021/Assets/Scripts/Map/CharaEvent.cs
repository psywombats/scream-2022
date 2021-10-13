using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(FieldSpritesheetComponent))]
[RequireComponent(typeof(MapEvent))]
[DisallowMultipleComponent]
public class CharaEvent : MonoBehaviour {

    private const float DesaturationDuration = 0.5f;
    private const float StepsPerSecond = 2.0f;

    [SerializeField] public new SpriteRenderer renderer;

    private Vector2 lastPosition;
    private Vector3 targetPx;
    private int oldX;

    public MapEvent Event { get { return GetComponent<MapEvent>(); } }
    public Map Map { get { return Event.Map; } }
    public int StepCount => sprites.StepCount;

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
            _facing = value;
            UpdateAppearance();
        }
    }

    private List<SpriteRenderer> _renderers;
    protected List<SpriteRenderer> Renderers {
        get {
            if (_renderers == null) {
                _renderers = new List<SpriteRenderer> {
                    renderer
                };
            }
            return _renderers;
        }
    }

    public void Start() {
        GetComponent<Dispatch>().RegisterListener(MapEvent.EventEnabled, (object payload) => {
            UpdateEnabled((bool)payload);
        });
        GetComponent<Dispatch>().RegisterListener(MapEvent.EventInteract, (object payload) => {
            Facing = Event.DirectionTo(Global.Instance.Maps.Avatar.GetComponent<MapEvent>());
        });
        UpdateEnabled(Event.IsSwitchEnabled);
    }

    public void Update() {
        //if (animates) {
        //    var elapsed = Time.time;
        //    var newX = Mathf.FloorToInt(elapsed * StepsPerSecond) % Sprites.StepCount;
        //    if (oldX != newX) {
        //        UpdateAppearance();
        //        oldX = newX;
        //    }
        //}
    }

    public void UpdateEnabled(bool enabled) {
        foreach (var renderer in Renderers) {
            renderer.enabled = enabled && !OverrideHide;
        }
        UpdateAppearance();
    }

    public void UpdateAppearance() {
        renderer.sprite = SpriteForMain();
    }

    public void FaceToward(MapEvent other) {
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

    public void SetAppearanceByTag(string fieldSpriteTag) {
        Sprites.SetByTag(fieldSpriteTag);
        UpdateAppearance();
    }

    public IEnumerator StepRoutine(OrthoDir dir) {
        throw new NotImplementedException();
    }

    private Sprite SpriteForMain() {
        int x = Mathf.FloorToInt(Time.time * StepsPerSecond) % Sprites.StepCount;
        return sprites.GetFrame(Facing, x);
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
        if (!cam) {
            return Facing;
        }

        Vector3 ourScreen = cam.GetCameraComponent().WorldToScreenPoint(transform.position);
        Vector3 targetWorld = Event.TileToWorldCoords(Event.Location + Facing.XY3D());
        targetWorld.y = Event.transform.position.y;
        Vector3 targetScreen = cam.GetCameraComponent().WorldToScreenPoint(targetWorld);
        Vector3 delta = targetScreen - ourScreen;
        return OrthoDirExtensions.DirectionOf2D(new Vector2(delta.x, -delta.y));
    }
}
