﻿using DG.Tweening;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MapEvent))]
[RequireComponent(typeof(Rigidbody))]
public class AvatarEvent : MonoBehaviour, IInputListener {

    public static AvatarEvent Instance => MapManager.Instance.Avatar;

    [SerializeField] public GameObject firstPersonParent = null;
    [SerializeField] private MapCamera fpsCam = null;
    [Space]
    [SerializeField] private float degreesPerSecond = 120;
    [SerializeField] [Range(0f, 9f)] float mouseRotateSensitivity = 2f;
    [SerializeField] Vector2 RotationYBounds = new Vector2(-35, 25);
    [SerializeField] private float rayRange = 2.5f;

    private MapEvent @event;
    public MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private Rigidbody body;
    public Rigidbody Body => body ?? (body = GetComponent<Rigidbody>());

    public MapCamera FPSCam => fpsCam;

    // beast mode
    public float TurnRate { get; set; } = 1f;
    public float CantLook;
    public float PreventApproach;
    public float LookAway;
    // end beast mode

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

    public bool UseFirstPersonControl => true;

    public bool DisableHeightCrossing { get; set; }
    public bool FreeTraverse { get; set; }

    private Vector3 velocityThisFrame;
    private Vector2Int lastMousePos;
    private Vector2Int lastLoc;
    private Vector3 rotation;
    private Map lastMap;
    private float mouseLastMovedAt;
    private bool tracking;
    private bool trackingLastFrame;
    private bool fpLastFrame;

    public void Start() {
        if (MapManager.Instance.Avatar == null) {
            MapManager.Instance.Avatar = this;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public virtual void Update() {

        trackingLastFrame = tracking;
        tracking = false;
        lastMap = MapManager.Instance.ActiveMap;
        lastLoc = Event.Location;

        firstPersonParent.SetActive(UseFirstPersonControl);
        if (UseFirstPersonControl != fpLastFrame) {
            //MapManager.Instance.Camera = null;
        }
        if (UseFirstPersonControl) {
            HandleFPC();
        }
        lastMousePos = InputManager.Instance.GetMouse();

        if (velocityThisFrame != Vector3.zero) {
            var xcom = new Vector3(velocityThisFrame.x, 0, 0);
            var ycom = new Vector3(0, 0, velocityThisFrame.z);

            CheckPhysicsComponent(xcom, ycom);
            CheckPhysicsComponent(ycom, xcom);
        }

        Body.velocity = velocityThisFrame;
        velocityThisFrame = Vector3.zero;
        fpLastFrame = UseFirstPersonControl;

        HandleRay();
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
                    case InputManager.Command.Right:
                        TryStep(OrthoDir.East);
                        return false;
                    case InputManager.Command.Left:
                        TryStep(OrthoDir.West);
                        return false;
                    default:
                        return false;
                }
            case InputManager.Event.Up:
                switch (command) {
                    case InputManager.Command.Primary:
                        Interact();
                        return false;
                    case InputManager.Command.Secondary:
                    case InputManager.Command.Menu:
                        ShowMenu();
                        return false;
                    default:
                        return false;
                }
            default:
                return false;
        }
    }

    public void SetFacing(OrthoDir dirr) {
        var targetPos = firstPersonParent.transform.position + dirr.Px3D();
        var dir = (targetPos - firstPersonParent.transform.position).normalized;
        var lookAngles = Quaternion.LookRotation(dir);
        FPSCam.transform.localRotation = lookAngles;
    }

    public void PauseInput() {
        pauseCount += 1;
    }

    public void UnpauseInput() {
        if (pauseCount > 0) {
          pauseCount -= 1;
        }

    }

    public bool WantsToTrack() {
        return trackingLastFrame || tracking;
    }

    public IEnumerator RotateTowardRoutine(MapEvent other) {
        var targetPos = other.PositionPx + new Vector3(.5f, 0, .5f);
        var dir = (targetPos - firstPersonParent.transform.position).normalized;
        var lookAngles = Quaternion.LookRotation(dir).eulerAngles;

        return CoUtils.RunTween(FPSCam.transform.DORotate(lookAngles, .5f));
    }

    public bool CanCrossTileGradient(Vector2Int from, Vector2Int to) {
        if (FreeTraverse) {
            return true;
        }
        float fromHeight = Event.Map.Terrain.HeightAt(from);
        float toHeight = GetComponent<MapEvent>().Map.Terrain.HeightAt(to);
        return Mathf.Abs(fromHeight - toHeight) < 1.0f && toHeight > 0.0f;
    }

    public OrthoDir FPFacing() {
        var a = FPSCam.transform.localEulerAngles.y;
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

    public IEnumerator ShakeRoutine(float s = .2f) {
        FPSCam.osc.enabled = true;
        yield return CoUtils.Wait(s);
        FPSCam.osc.enabled = false;
        FPSCam.transform.localPosition = Vector3.zero;
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
            if (h1 - h2 >= 1 && !FreeTraverse) {
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

    private bool TryStep(OrthoDir dir) {
        var targetEvents = GetComponent<MapEvent>().Map.GetEventsAt(Event.PositionPx);
        foreach (var tryTarget in targetEvents) {
            if (tryTarget.IsSwitchEnabled && tryTarget.IsPassableBy(Event) && tryTarget != Event) {
                tryTarget.Collide(this);
                break;
            }
        }

        tracking = true;
        var component = dir.Px3D() * Event.tilesPerSecond * 1.2f;
        if (UseFirstPersonControl) {
            var c2 = Quaternion.AngleAxis(FPSCam.transform.localEulerAngles.y, Vector3.up) * component;
            velocityThisFrame += c2;
        } else {
            velocityThisFrame += component;
        }

        var beastly = Vector3.Angle(component, new Vector3(1, 0, 0)) / 180f;
        var beastlied = component * beastly;

        if (velocityThisFrame.x > 0 && PreventApproach > 0f) {
            velocityThisFrame.x *= (1f - PreventApproach);
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
        /*
        PauseInput();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        */
    }

    private void HandleFPC() {
        if (InputManager.Instance.GetMouse() != lastMousePos) {
            mouseLastMovedAt = Time.time;
        }
        if (Time.time - mouseLastMovedAt > .5) {
            //return;
        }

       
        var mouse = Mouse.current.delta;
        var inX = mouse.x.ReadValue();
        var inY = mouse.y.ReadValue();
        /*rotation.x += mouse.x.ReadValue() * mouseRotateSensitivity;
        rotation.y += mouse.y.ReadValue() * mouseRotateSensitivity;
        rotation.y = Mathf.Clamp(rotation.y, RotationYBounds.x, RotationYBounds.y);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
        */
        if (InputPaused) {
            return;
        }

        

        var trans = FPSCam.transform;

        var oldB = TowardsBeastLook();
        var oldRot = trans.rotation;

        trans.rotation *= Quaternion.AngleAxis(inY  * mouseRotateSensitivity, Vector3.left);

        var ang = trans.eulerAngles.x;
        while (ang > 180) ang -= 360;
        while (ang < -180) ang += 360;

        var xcom = inX * mouseRotateSensitivity * TurnRate;

        trans.rotation = Quaternion.Euler(
            Mathf.Clamp(ang, RotationYBounds.x, RotationYBounds.y),
            trans.eulerAngles.y + xcom,
            trans.eulerAngles.z
        );
        var newB = TowardsBeastLook();
        if (newB > oldB && CantLook > 0f) {
            trans.rotation = oldRot;
            var coef = Mathf.Max(0f, 1f - ((CantLook * TowardsBeastLook()) + .3f * PreventApproach));
            trans.rotation = Quaternion.Euler(
                Mathf.Clamp(ang, RotationYBounds.x, RotationYBounds.y),
                trans.eulerAngles.y + xcom * coef,
                trans.eulerAngles.z
            );
        }

        if (LookAway > 0) {
            var rate = TowardsBeastLook() * LookAway * PreventApproach;
            rate *= rate;
            var away = rate * Time.deltaTime * 120;
            oldB = TowardsBeastLook();
            oldRot = trans.rotation;

            trans.rotation = Quaternion.Euler(
                trans.eulerAngles.x,
                trans.eulerAngles.y + away,
                trans.eulerAngles.z);
            if (oldB < TowardsBeastLook()) {
                trans.rotation = oldRot;
                trans.rotation = Quaternion.Euler(
                    trans.eulerAngles.x,
                    trans.eulerAngles.y - away,
                    trans.eulerAngles.z);
            }
        }
    }

    private void HandleRay() {
        var chara = GetLookingChara();
        if (chara != null && !InputPaused) {
            chara.HandleRay();
        }
    }

    private CharaEvent GetLookingChara() {
        var cameraCenter = fpsCam.Cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, fpsCam.Cam.nearClipPlane));
        if (Physics.Raycast(cameraCenter, fpsCam.transform.forward, out var hit, 1000)) {
            var obj = hit.transform.gameObject;
            var chara = obj.GetComponent<CharaEvent>();
            if (chara != null && hit.distance < rayRange) {
                return chara;
            }
        }
        return null;
    }

    private void Interact() {
        var chara = GetLookingChara();
        if (chara != null) {
            chara.Interact();
        }
    }

    public float TowardsBeastLook() {
        var fwd = AvatarEvent.Instance.FPSCam.transform.forward;
        var angle = Vector3.Angle(fwd, new Vector3(1, 0, 0));
        return 1f - (angle / 180f);
    }
}
