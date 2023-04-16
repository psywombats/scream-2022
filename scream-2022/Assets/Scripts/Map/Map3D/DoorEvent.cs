using UnityEngine;
using System.Collections;
using FMODUnity;

[RequireComponent(typeof(MapEvent))]
public class DoorEvent : MonoBehaviour {

    [SerializeField] private OrthoDir dir;
    [Space]
    [SerializeField] private SimpleSpriteAnimator animator;
    [Space]
    [SerializeField] private string mapName;
    [SerializeField] private string targetEventName;
    [SerializeField] private string lockedLua = "speak('Locked')";
    [SerializeField] [TextArea(5, 10)] private string lockedCondition;

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
        if (dir == OrthoDir.South) {
            if (AvatarEvent.Instance.UseFirstPersonControl) {
                animator.transform.localEulerAngles = new Vector3(0, 180, 0);
                animator.transform.localPosition = new Vector3(.5f, 1.325f, .01f);
                var c = animator.GetComponent<SpriteRenderer>().color;
                animator.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 1);
            }
        }
    }

    public virtual IEnumerator TeleportRoutine(AvatarEvent avatar, bool force = false) {
        if (avatar.GetComponent<CharaEvent>().Facing != dir && !avatar.UseFirstPersonControl) {
            yield break;
        }
        if (avatar.UseFirstPersonControl && AvatarEvent.Instance.FPFacing() != dir) {
            yield break;
        }
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
        var checkPos = Event.PositionPx;
        checkPos += dir.Px3D() * .5f;
        var dist = Vector3.Distance(avatar.Event.PositionPx, checkPos);
        if (!force && dist > .7f) {
            yield break;
        }

        GetComponent<StudioEventEmitter>().Play();

        avatar.PauseInput();
        avatar.CancelCollisions = true;
        if (force && dist > .7f) {
            yield return avatar.Event.LinearStepRoutine(transform.localPosition);
        }
        //if (avatar.UseFirstPersonControl) {
        //    StartCoroutine(avatar.RotateTowardRoutine(Event));
        //}
        yield return animator.PlayRoutine();
        yield return CoUtils.Wait(animator.frameDuration * 2);
        Vector3 targetPx = avatar.Event.PositionPx + dir.Px3D();
        yield return CoUtils.RunParallel(new IEnumerator[] {
            avatar.GetComponent<MapEvent>().LinearStepRoutine(targetPx),
            avatar.GetComponent<CharaEvent>().FadeRoutine(1.0f / avatar.GetComponent<MapEvent>().tilesPerSecond * 0.75f)
        }, this);
        yield return CoUtils.Wait(animator.frameDuration * 2);
        yield return Global.Instance.Maps.TeleportRoutine(mapName, targetEventName);
        yield return avatar.GetComponent<MapEvent>().LinearStepRoutine(avatar.Event.PositionPx + avatar.Chara.Facing.Px3D() * .2f);
        avatar.UnpauseInput();
        avatar.CancelCollisions = false;
    }
}
