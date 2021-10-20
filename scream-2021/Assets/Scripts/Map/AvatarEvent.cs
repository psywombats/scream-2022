using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharaEvent))]
[RequireComponent(typeof(MapEvent))]
[RequireComponent(typeof(Rigidbody))]
public class AvatarEvent : MonoBehaviour, IInputListener {

    public static AvatarEvent Instance => MapManager.Instance.Avatar;

    [SerializeField] private GameObject firstPersonParent = null;
    [SerializeField] private GameObject thirdPersonParent = null;
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

    private int pauseCount;
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
            return Global.Instance.Data.GetSwitch("night");
        }
    }

    private Vector3 velocityThisFrame;
    private Vector3 lastMousePos;
    private Vector2Int lastLoc;
    private Vector3 rotation;
    private Map lastMap;
    private float mouseLastMovedAt;
    private bool tracking;
    private bool trackingLastFrame;
    private bool fpLastFrame;

    public void Start() {
        MapManager.Instance.Avatar = this;
        InputManager.Instance.PushListener(this);
    }

    public virtual void Update() {
        trackingLastFrame = tracking;
        tracking = false;
        lastMap = MapManager.Instance.ActiveMap;
        lastLoc = Event.Location;

        firstPersonParent.SetActive(UseFirstPersonControl);
        thirdPersonParent.SetActive(!UseFirstPersonControl);
        if (UseFirstPersonControl != fpLastFrame) {
            MapManager.Instance.Camera = null;
        }
        if (UseFirstPersonControl) {
            HandleFPC();
        }

        if (velocityThisFrame != Vector3.zero) {
            var xcom = new Vector3(velocityThisFrame.x, 0, 0);
            var ycom = new Vector3(0, 0, velocityThisFrame.z);

            // TODO: don't switch directions if facing is okay
            Chara.Facing = OrthoDirExtensions.DirectionOf3D(velocityThisFrame);

            CheckPhysicsComponent(xcom);
            CheckPhysicsComponent(ycom);
        }

        Body.velocity = velocityThisFrame;
        velocityThisFrame = Vector3.zero;
        fpLastFrame = UseFirstPersonControl;
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

    private void CheckPhysicsComponent(Vector3 delta) {
        if (delta == Vector3.zero) {
            return;
        }

        delta = delta.normalized;
        var adjustedPos = transform.localPosition + .7f * delta;
        var adjustedLoc = MapEvent.WorldPositionToTileCoords(adjustedPos);
        if (adjustedLoc != Event.Location) {
            var h1 = Event.Map.Terrain.HeightAt(Event.Location);
            var h2 = Event.Map.Terrain.HeightAt(adjustedLoc);
            if ((Mathf.Abs(h1 - h2) == .5f) || (h1 - h2 == 1)) {
                // these are stairs and we are moving towards them
                velocityThisFrame = Vector3.zero;
                Chara.Facing = OrthoDirExtensions.DirectionOf3D(delta);
                StartCoroutine(Event.LinearStepRoutine(adjustedLoc));
            }
        }

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
        var reach = UseFirstPersonControl ? 1f : .75f;
        var target = Event.PositionPx + Chara.Facing.Px3D() * reach;
        List<MapEvent> targetEvents = GetComponent<MapEvent>().Map.GetEventsAt(target);
        foreach (MapEvent tryTarget in targetEvents) {
            if (tryTarget.IsSwitchEnabled && !tryTarget.IsPassableBy(Event) && tryTarget != Event) {
                tryTarget.GetComponent<Dispatch>().Signal(MapEvent.EventInteract, this);
                return;
            }
        }

        target = Event.PositionPx;
        targetEvents = GetComponent<MapEvent>().Map.GetEventsAt(target);
        foreach (MapEvent tryTarget in targetEvents) {
            if (tryTarget.IsSwitchEnabled && tryTarget.IsPassableBy(Event)) {
                tryTarget.GetComponent<Dispatch>().Signal(MapEvent.EventInteract, this);
                return;
            }
        }
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
        var c2 =  Quaternion.AngleAxis(firstPersonParent.transform.localEulerAngles.y, Vector3.up) * component;
        velocityThisFrame += c2;
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
        lastMousePos = Input.mousePosition;
        if (Time.time - mouseLastMovedAt > 3) {
            return;
        }

        rotation.x += Input.GetAxis(xAxis) * mouseRotateSensitivity;
        rotation.y += Input.GetAxis(yAxis) * mouseRotateSensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -20, 6);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        firstPersonParent.transform.localRotation = xQuat * yQuat;
    }
}
