using UnityEngine;
using System.Collections;
using FMODUnity;

[RequireComponent(typeof(MapEvent))]
public class DoorEvent : MonoBehaviour {
    
    //[SerializeField] private SimpleSpriteAnimator animator;
    [Space]
    [SerializeField] private string mapName;
    [SerializeField] private string targetEventName;
    [SerializeField] [TextArea(5, 10)] private string lockedCondition;
    [SerializeField] [TextArea(5, 10)] private string lockedLua = "";

    private MapEvent @event;
    public MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    private void Start() {
        RefreshDoor();

        GetComponent<Dispatch>().RegisterListener(MapEvent.EventCollide, (object payload) => {
            Global.Instance.StartCoroutine(TeleportRoutine(AvatarEvent.Instance));
        });
        GetComponent<Dispatch>().RegisterListener(MapEvent.EventInteract, (object payload) => {
            Global.Instance.StartCoroutine(TeleportRoutine(AvatarEvent.Instance, force: true));
        });
        Event.LuaObject.Set("door", lockedCondition);
    }

    public void RefreshDoor() {

    }

    public virtual IEnumerator TeleportRoutine(AvatarEvent avatar, bool force = false) {

        if (!GetComponent<MapEvent>().IsSwitchEnabled) {
            yield break;
        }
        if (lockedCondition.Length > 0 && !Event.LuaObject.EvaluateBool("door")) {
            if (force) {
                Event.LuaObject.Set("locked", lockedLua);
                Event.LuaObject.Run("locked");
            }
            yield break;
        }
        GetComponent<StudioEventEmitter>().Play();

        avatar.PauseInput();
        avatar.CancelCollisions = true;
        yield return avatar.RotateTowardRoutine(Event);
        yield return Global.Instance.Maps.TeleportRoutine(mapName, targetEventName);
        yield return avatar.GetComponent<MapEvent>().LinearStepRoutine(avatar.Event.PositionPx + avatar.FPFacing().Px3D() * .2f);
        avatar.UnpauseInput();
        avatar.CancelCollisions = false;
    }

    public void OnValidate() {
        RefreshDoor();
    }
}
