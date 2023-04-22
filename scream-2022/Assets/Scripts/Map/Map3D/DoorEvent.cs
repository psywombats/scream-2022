using UnityEngine;
using System.Collections;
using FMODUnity;

[RequireComponent(typeof(MapEvent))]
public class DoorEvent : MonoBehaviour {
    
    //[SerializeField] private SimpleSpriteAnimator animator;
    [Space]
    [SerializeField] private string mapName;
    [SerializeField] private string targetEventName;
    [SerializeField] OrthoDir dir;
    [SerializeField] [TextArea(3, 6)] private string lockedCondition;
    [SerializeField] [TextArea(3, 6)] private string lockedLua = "";
    [SerializeField] private string passSwitch;
    [SerializeField] private string glitchBackSwitch;
    [SerializeField] private string glitchBackMap;
    [SerializeField] private string glitchBackEvent;
    [SerializeField] private OrthoDir glitchBackDir;

    private MapEvent @event;
    public MapEvent Event => @event ?? (@event = GetComponent<MapEvent>());

    public void Start() {
        RefreshDoor();

        Event.OnCollide += () => Global.Instance.StartCoroutine(TeleportRoutine(AvatarEvent.Instance));
        Event.OnInteract += () => Global.Instance.StartCoroutine(TeleportRoutine(AvatarEvent.Instance, force: true)); 
        Event.LuaObject.Set("door", lockedCondition);
    }

    public void RefreshDoor() {

    }

    public virtual IEnumerator TeleportRoutine(AvatarEvent avatar, bool force = false) {
        if (string.IsNullOrEmpty(mapName)) {
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
        var emitter = GetComponent<StudioEventEmitter>();
        if (emitter != null) {
            emitter.Play();
        }

        if (!string.IsNullOrEmpty(passSwitch)) {
            Global.Instance.Data.SetSwitch("sumi_last_chris", false);
            Global.Instance.Data.SetSwitch("sumi_last_elevator", false);
            Global.Instance.Data.SetSwitch("sumi_last_noemi", false);
            Global.Instance.Data.SetSwitch("sumi_last_braulio", false);
            Global.Instance.Data.SetSwitch("sumi_last_gazer", false);
            Global.Instance.Data.SetSwitch(passSwitch, true);
        }

        avatar.PauseInput();
        avatar.CancelCollisions = true;
        var reFace = GetComponent<CharaEvent>().dir;

        yield return avatar.RotateTowardRoutine(Event);
        yield return Global.Instance.Maps.TeleportRoutine(mapName, targetEventName, dir);
        if (!string.IsNullOrEmpty(glitchBackSwitch) && !Global.Instance.Data.GetSwitch(glitchBackSwitch)) {
            yield return CoUtils.Wait(.75f);
            MapOverlayUI.Instance.setting.Scramble();
            Global.Instance.Data.SetSwitch("glitch_on", true);
            yield return CoUtils.Wait(.75f);
            Global.Instance.Data.SetSwitch("no_settings", true);
            yield return Global.Instance.Maps.TeleportRoutine(glitchBackMap, glitchBackEvent, glitchBackDir, isRaw: true);
            Global.Instance.Data.SetSwitch("glitch_on", false);
            Global.Instance.Data.SetSwitch("no_settings", false);
        }
        avatar.UnpauseInput();
        avatar.CancelCollisions = false;
    }

    public void OnValidate() {
        RefreshDoor();
    }
}
