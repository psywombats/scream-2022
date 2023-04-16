﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using FMODUnity;

[RequireComponent(typeof(FieldSpritesheetComponent))]
[RequireComponent(typeof(MapEvent))]
[DisallowMultipleComponent]
public class CharaEvent : MonoBehaviour {

    public const float HitThick = .4f;
    private const float DesaturationDuration = 0.5f;
    private const float StepsPerSecond = 3.0f;

    [SerializeField] public bool directionFix;
    [SerializeField] public bool useRelativeFix;
    [SerializeField] public OrthoDir relativeFixDir;
    [SerializeField] public DollComponent doll;
    public bool skipWalks = false;
    public bool noAnimate = false;
    public StudioEventEmitter Emitter => Doll.emitter;
    public SpriteRenderer Renderer => Doll.renderer;

    private Vector3 lastPosition;
    private Vector3 targetPx;
    private bool stepping;
    private float lastMoveTime;
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
            if (!directionFix) {
                Facing = Event.DirectionTo(Global.Instance.Maps.Avatar.GetComponent<MapEvent>());
            }
        });
        UpdateEnabled(Event.IsSwitchEnabled);
    }

    public void LateUpdate() {
        if (!Event.IsSwitchEnabled) {
            Emitter.Stop();
            return;
        }
        if (!Event.IsTracking && Event != AvatarEvent.Instance.Event && !directionFix) {
            AutofaceDirection();
        }

        bool steppingThisFrame = IsSteppingThisFrame();
        if (steppingThisFrame) {
            lastMoveTime = Time.time;
        }
        var wasSteppingLastFrame = (Time.time - lastMoveTime) < .05f;
        stepping = steppingThisFrame || wasSteppingLastFrame;
        if (steppingThisFrame && !wasSteppingLastFrame) {
            PlaySFX();
            moveTime += 1f / StepsPerSecond;
        } else if (Emitter != null && !steppingThisFrame && !wasSteppingLastFrame) {
            if (moveTime > 0f) {
                moveTime = 0.0f;
                Emitter.Stop();
            }
        } else {
            PlaySFX();
            moveTime += Time.deltaTime;
        }
        lastPosition = transform.position;

        UpdateAppearance();
    }

    private void PlaySFX()
    {
        if (Emitter != null) {
            Emitter.SetParameter("Floor_Type", Map.matIndex);
            if (Emitter != null && !Emitter.IsPlaying() && !skipWalks && Event.IsSwitchEnabled) {
                Emitter.Play();
            }
        }
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

    public bool IsVisible() {
        var pos = doll.renderer.transform.position + new Vector3(0, 1f, 0);
        var vp = MapManager.Instance.Camera.GetCameraComponent().WorldToViewportPoint(pos);
        return vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1 && vp.z >= 0 && vp.z > 0;
    }

    private Sprite SpriteForMain(bool fixedTime) {
        if (!fixedTime && !noAnimate) {
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

        if (useRelativeFix) {
            return relativeFixDir;
        }

        var pos = doll.renderer.transform.position;
        var dirToAva = OrthoDirExtensions.DirectionOf3D(pos - AvatarEvent.Instance.FPSCam.transform.position);
        var toAdd = (((int)dirToAva) * -1);
        while (toAdd < 0) toAdd += 4;
        toAdd %= 4;
        var newFace = (OrthoDir)(((int)Facing + toAdd) % 4);
        return newFace;
    }

    private bool IsSteppingThisFrame() {
        if (noAnimate) return false;
        var ava = GetComponent<AvatarEvent>();
        if (ava != null) return Event.IsTracking || ava.WantsToTrack();
        var position = transform.position;
        position.y = 0;
        var old = lastPosition;
        old.y = 0;
        var delta = position - old;
        return (delta.sqrMagnitude > 0.02 * Time.deltaTime && delta.sqrMagnitude < Map.UnitsPerTile)  
            || Event.IsTracking;
    }

    private void AutofaceDirection() {
        var delta = transform.position - lastPosition;
        delta.y = 0f;
        if (delta.sqrMagnitude / Time.deltaTime < .05) {
            return;
        }

        Facing = OrthoDirExtensions.DirectionOf3D(delta);
    }
}
