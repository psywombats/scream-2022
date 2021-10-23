using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharaEvent))]
[RequireComponent(typeof(MapEvent))]
[RequireComponent(typeof(Rigidbody))]
public class AvatarEvent : MonoBehaviour, IInputListener {

    public static AvatarEvent Instance => MapManager.Instance.Avatar;

    [SerializeField] private GameObject firstPersonParent = null;
    [SerializeField] private GameObject thirdPersonParent = null;
    [SerializeField] private MapCamera fpsCam = null;
    [SerializeField] private bool fpsOverride = false;
    [Space]
    [SerializeField] private float degreesPerSecond = 120;
    [SerializeField] [Range(0.1f, 9f)] float mouseRotateSensitivity = 2f;

    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";

    private MapEvent @event;
    public MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private CharaEvent chara;
    public CharaEvent Chara => chara ?? (chara = GetComponent<CharaEvent>());

    private Rigidbody body;
    public Rigidbody Body => body ?? (body = GetComponent<Rigidbody>());

    public MapCamera FPSCam => fpsCam;

    private static int pauseCount;
    public bool InputPaused {
        get {
            return pauseCount > 0;
        }
    }

    public bool CancelCollisions {
        set {
            Body.isKinematic = value;
        }
    }

    public bool UseFirstPersonControl {
        get {
            return Global.Instance.Data.GetSwitch("fp_only") || Global.Instance.Data.GetSwitch("night") || fpsOverride;
        }
    }

    public bool DisableHeightCrossing { get; set; }

    private Vector3 velocityThisFrame;
    private Vector3 lastMousePos;
    private Vector2Int lastLoc;
    private Vector3 rotation;
    private Map lastMap;
    private float mouseLastMovedAt;
    private bool tracking;
    private bool trackingLastFrame;
    private bool fpLastFrame;

    private static bool seen3P, seen1P;

    public void Start() {
        if (MapManager.Instance.Avatar == null) {
            MapManager.Instance.Avatar = this;
        }
    }

    public virtual void Update() {

        var cons = MapOverlayUI.Instance.controls;
        if (!seen1P && UseFirstPersonControl) {
            seen1P = true;
            cons.StartCoroutine(cons.Trigger(cons.set2));
        }
        if (!seen3P && !UseFirstPersonControl) {
            seen3P = true;
            //cons.StartCoroutine(cons.Trigger(cons.set1));
        }

        trackingLastFrame = tracking;
        tracking = false;
        lastMap = MapManager.Instance.ActiveMap;
        lastLoc = Event.Location;

        firstPersonParent.SetActive(UseFirstPersonControl);
        thirdPersonParent.SetActive(!UseFirstPersonControl);
        if (UseFirstPersonControl != fpLastFrame) {
            //MapManager.Instance.Camera = null;
        }
        if (UseFirstPersonControl) {
            HandleFPC();
        }
        lastMousePos = Input.mousePosition;

        if (velocityThisFrame != Vector3.zero) {
            var xcom = new Vector3(velocityThisFrame.x, 0, 0);
            var ycom = new Vector3(0, 0, velocityThisFrame.z);

            // TODO: don't switch directions if facing is okay
            Chara.Facing = OrthoDirExtensions.DirectionOf3D(velocityThisFrame);

            CheckPhysicsComponent(xcom, ycom);
            CheckPhysicsComponent(ycom, xcom);
        }

        Body.velocity = velocityThisFrame;
        velocityThisFrame = Vector3.zero;
        fpLastFrame = UseFirstPersonControl;
    }

    public void OnEnable() {
        InputManager.Instance.PushListener(this);
    }

    public void OnDisable() {
        InputManager.Instance.RemoveListener(this);
    }

    public bool OnCommand(InputManager.Command command, InputManager.Event eventType) {
        if (InputPaused || Event.IsTracking) {
            return true;
        }
        switch (eventType) {
            case InputManager.Event.Hold:
                switch (command) {
                    case InputManager.Command.Up:
                        TryStep(OrthoDir.North);
                        return false;
                    case InputManager.Command.Down:
                        TryStep(OrthoDir.South);
                        return false;
                    case InputManager.Command.StrafeRight:
                        TryStep(OrthoDir.East);
                        return false;
                    case InputManager.Command.StrafeLeft:
                        TryStep(OrthoDir.West);
                        return false;
                    case InputManager.Command.Right:
                        if (UseFirstPersonControl) {
                            TryTurn(1);
                        } else {
                            TryStep(OrthoDir.East);
                        }
                        return false;
                    case InputManager.Command.Left:
                        if (UseFirstPersonControl) {
                            TryTurn(-1);
                        } else {
                            TryStep(OrthoDir.West);
                        }
                        return false;
                    default:
                        return false;
                }
            case InputManager.Event.Up:
                switch (command) {
                    case InputManager.Command.Confirm:
                        Interact();
                        return false;
                    case InputManager.Command.Cancel:
                        ShowMenu();
                        return false;
                    case InputManager.Command.Debug:
                        SerializationManager.Instance.SaveToSlot(0);
                        return false;
                    default:
                        return false;
                }
            default:
                return false;
        }
    }

    public void PauseInput() {
        pauseCount += 1;
    }

    public void UnpauseInput() {
        pauseCount -= 1;
    }

    public bool WantsToTrack() {
        return trackingLastFrame || tracking;
    }

    public IEnumerator RotateTowardRoutine(MapEvent other) {
        var targetPos = other.PositionPx + new Vector3(.5f, 0, .5f);
        var dir = (targetPos - firstPersonParent.transform.position).normalized;
        var lookAngles = Quaternion.LookRotation(dir).eulerAngles;

        return CoUtils.RunTween(firstPersonParent.transform.DORotate(lookAngles, .5f));
    }

    public OrthoDir FPFacing() {
        if (!UseFirstPersonControl) {
            return chara.Facing;
        }

        var a = firstPersonParent.transform.localEulerAngles.y;
        while (a < -180) a += 360;
        while (a > 180) a -= 360;
        if (a >= -45 && a <= 45) {
            return OrthoDir.North;
        } else if (a >= 45 && a <= 135) {
            return OrthoDir.East;
        } else if (a >= 135 || a <= -135) {
            return OrthoDir.South;
        } else if (a >= -135 && a <= -45) {
            return OrthoDir.West;
        } else {
            throw new ArgumentException();
        }
    }

    private void CheckPhysicsComponent(Vector3 delta, Vector3 otherDelta) {
        if (delta == Vector3.zero) {
            return;
        }

        Vector3 adjustedPos;
        Vector2Int adjustedLoc;

        if (delta.sqrMagnitude > otherDelta.sqrMagnitude) {
            delta = delta.normalized;
            adjustedPos = transform.localPosition + .7f * delta;
            adjustedLoc = MapEvent.WorldPositionToTileCoords(adjustedPos);
            if (adjustedLoc != Event.Location) {
                var h1 = Event.Map.Terrain.HeightAt(Event.Location);
                var h2 = Event.Map.Terrain.HeightAt(adjustedLoc);
                if ((Mathf.Abs(h1 - h2) == .5f) || (h1 - h2 == 1)) {
                    // these are stairs and we are moving towards them
                    if (!DisableHeightCrossing) {
                        velocityThisFrame = Vector3.zero;
                        Chara.Facing = OrthoDirExtensions.DirectionOf3D(delta);
                        StartCoroutine(Event.LinearStepRoutine(adjustedLoc));
                    }
                }
            }
        }

        delta = delta.normalized;
        adjustedPos = transform.localPosition + .3f * delta;
        adjustedLoc = MapEvent.WorldPositionToTileCoords(adjustedPos);
        if (adjustedLoc != Event.Location) {
            var h1 = Event.Map.Terrain.HeightAt(Event.Location);
            var h2 = Event.Map.Terrain.HeightAt(adjustedLoc);
            if (h1 - h2 >= 1) {
                // this is a steep drop
                if (adjustedLoc.x != Event.Location.x) {
                    velocityThisFrame.x = 0;
                }
                if (adjustedLoc.y != Event.Location.y) {
                    velocityThisFrame.z = 0;
                }
            }
        }
    }

    private void Interact() {
        if (TryInteractWithReach(.65f)) {
            return;
        }
        if (UseFirstPersonControl && TryInteractWithReach(.8f)) {
            return;
        }
        
        var target = Event.PositionPx;
        var targetEvents = GetComponent<MapEvent>().Map.GetEventsAt(target);
        foreach (MapEvent tryTarget in targetEvents) {
            if (tryTarget.IsSwitchEnabled && tryTarget.IsPassableBy(Event)) {
                tryTarget.GetComponent<Dispatch>().Signal(MapEvent.EventInteract, this);
                return;
            }
        }
    }

    private bool TryInteractWithReach(float reach) {
        Vector3 target;
        if (UseFirstPersonControl) {
            target = firstPersonParent.transform.position + (firstPersonParent.transform.rotation * Vector3.forward) * reach;
            target -= new Vector3(.5f, 0, .5f);
        } else {
            target = Event.PositionPx + Chara.Facing.Px3D() * reach;
        }
        
        List<MapEvent> targetEvents = GetComponent<MapEvent>().Map.GetEventsAt(target);
        foreach (MapEvent tryTarget in targetEvents) {
            if (tryTarget.IsSwitchEnabled && !tryTarget.IsPassableBy(Event) && tryTarget != Event) {
                tryTarget.GetComponent<Dispatch>().Signal(MapEvent.EventInteract, this);
                return true;
            }
        }
        return false;
    }

    private bool TryStep(OrthoDir dir) {
        var targetEvents = GetComponent<MapEvent>().Map.GetEventsAt(Event.PositionPx);
        foreach (var tryTarget in targetEvents) {
            if (tryTarget.IsSwitchEnabled && tryTarget.IsPassableBy(Event) && tryTarget != Event) {
                tryTarget.GetComponent<Dispatch>().Signal(MapEvent.EventCollide, this);
                break;
            }
        }

        tracking = true;
        var component = dir.Px3D() * Event.tilesPerSecond;
        if (UseFirstPersonControl) {
            var c2 = Quaternion.AngleAxis(firstPersonParent.transform.localEulerAngles.y, Vector3.up) * component;
            velocityThisFrame += c2;
        } else {
            velocityThisFrame += component;
        }

        return true;
    }

    private void TryTurn(int sign) {
        firstPersonParent.transform.localEulerAngles = new Vector3(
            firstPersonParent.transform.localEulerAngles.x,
            firstPersonParent.transform.localEulerAngles.y + sign * Time.deltaTime * degreesPerSecond,
            0);
    }

    protected Vector2Int VectorForDir(OrthoDir dir) {
        return dir.XY3D();
    }

    private void ShowMenu() {
        // ??
    }

    private void HandleFPC() {
        if (InputPaused) {
            return;
        }
        if (!Input.mousePresent) {
            return;
        }
        if (Input.mousePosition != lastMousePos) {
            mouseLastMovedAt = Time.time;
        }
        if (Time.time - mouseLastMovedAt > .5) {
            return;
        }

        rotation.x += Input.GetAxis(xAxis) * mouseRotateSensitivity;
        rotation.y += Input.GetAxis(yAxis) * mouseRotateSensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -26, 15);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        //firstPersonParent.transform.localRotation = xQuat * yQuat;

        if (Input.GetMouseButtonUp(0)) {
            Interact();
        }
    }
}
