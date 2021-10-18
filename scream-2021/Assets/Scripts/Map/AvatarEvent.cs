using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharaEvent))]
[RequireComponent(typeof(MapEvent))]
[RequireComponent(typeof(Rigidbody))]
public class AvatarEvent : MonoBehaviour, IInputListener {

    public static AvatarEvent Instance => MapManager.Instance.Avatar;

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

    private MapEvent @event;
    public MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private CharaEvent chara;
    public CharaEvent Chara => chara ?? (chara = GetComponent<CharaEvent>());

    private Rigidbody body;
    public Rigidbody Body => body ?? (body = GetComponent<Rigidbody>());

    private Vector3 velocityThisFrame;
    private Vector2Int lastLoc;
    private bool tracking;

    public void Start() {
        if (MapManager.Instance.Avatar != null) {
            Destroy(gameObject);
            return;
        }
        MapManager.Instance.Avatar = this;
        InputManager.Instance.PushListener(this);
    }

    public virtual void Update() {
        tracking = false;

        if (Event.Location != lastLoc && !InputPaused) {
            var targetEvents = GetComponent<MapEvent>().Map.GetEventsAt(Event.PositionPx);
            foreach (var tryTarget in targetEvents) {
                if (tryTarget.IsSwitchEnabled && tryTarget.IsPassableBy(Event) && tryTarget != Event) {
                    tryTarget.GetComponent<Dispatch>().Signal(MapEvent.EventCollide, this);
                    break;
                }
            }
        }
        lastLoc = Event.Location;

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
        return tracking;
    }

    private void CheckPhysicsComponent(Vector3 delta) {
        if (delta == Vector3.zero) {
            return;
        }

        delta = delta.normalized;
        var adjustedPos = transform.localPosition + .7f * (Vector3.zero + delta);
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

        adjustedPos = transform.localPosition + .3f * (Vector3.zero + delta);
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
        var target = Event.PositionPx + Chara.Facing.Px3D() * .5f;
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
        tracking = true;
        var component = (Vector3.zero + dir.Px3D()) * Event.tilesPerSecond;
        velocityThisFrame += component;
        return true;
    }

    protected Vector2Int VectorForDir(OrthoDir dir) {
        return dir.XY3D();
    }

    private void ShowMenu() {
        // ??
    }
}
