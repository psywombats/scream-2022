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

    private MapEvent @event;
    public MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private CharaEvent chara;
    public CharaEvent Chara => chara ?? (chara = GetComponent<CharaEvent>());

    private Rigidbody body;
    public Rigidbody Body => body ?? (body = GetComponent<Rigidbody>());

    private Vector3 velocityThisFrame;
    private bool tracking;

    public void Start() {
        MapManager.Instance.Avatar = this;
        InputManager.Instance.PushListener(this);
        pauseCount = 0;
    }

    public virtual void Update() {
        tracking = false;
        Body.velocity = velocityThisFrame;
        velocityThisFrame = Vector3.zero;
        Body.angularVelocity = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public bool OnCommand(InputManager.Command command, InputManager.Event eventType) {
        if (InputPaused) {
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

    private void Interact() {
        // TODO: scream2021
        Vector2Int target = GetComponent<MapEvent>().Location + VectorForDir(GetComponent<CharaEvent>().Facing);
        List<MapEvent> targetEvents = GetComponent<MapEvent>().Map.GetEventsAt(target);
        foreach (MapEvent tryTarget in targetEvents) {
            if (tryTarget.IsSwitchEnabled && !tryTarget.IsPassableBy(Event)) {
                tryTarget.GetComponent<Dispatch>().Signal(MapEvent.EventInteract, this);
                return;
            }
        }

        target = GetComponent<MapEvent>().Location;
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
