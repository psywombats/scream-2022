using UnityEngine;
using System.Collections;
using System;
using MoonSharp.Interpreter;
using UnityEngine.SceneManagement;

public class LuaCutsceneContext : LuaContext {

    private static readonly string DefinesPath = "Lua/Defines/CutsceneDefines";

    public override IEnumerator RunRoutine(LuaScript script, bool canBlock) {
        if (canBlock && Global.Instance.Maps.Avatar != null) {
            Global.Instance.Maps.Avatar.PauseInput();
        }
        yield return base.RunRoutine(script, canBlock);
        if (canBlock && Global.Instance.Maps.Avatar != null) {
            Global.Instance.Maps.Avatar.UnpauseInput();
        }
    }

    public override void Initialize() {
        base.Initialize();
        LoadDefines(DefinesPath);
    }

    public override void RunRoutineFromLua(IEnumerator routine) {
            base.RunRoutineFromLua(routine);
    }

    public void RunTextboxRoutineFromLua(IEnumerator routine) {
        base.RunRoutineFromLua(routine);
    }

    protected void ResumeNextFrame() {
        Global.Instance.StartCoroutine(ResumeRoutine());
    }
    protected IEnumerator ResumeRoutine() {
        yield return null;
        ResumeAwaitedScript();
    }

    protected override void AssignGlobals() {
        base.AssignGlobals();
        lua.Globals["playBGM"] = (Action<DynValue>)PlayBGM;
        lua.Globals["playSound"] = (Action<DynValue>)PlaySound;
        lua.Globals["sceneSwitch"] = (Action<DynValue, DynValue>)SetSwitch;
        lua.Globals["wipe"] = (Action)Wipe;
        lua.Globals["clear"] = (Action)Wipe;
        lua.Globals["name"] = (Action<DynValue, DynValue>)SetName;
        lua.Globals["cs_teleport"] = (Action<DynValue, DynValue, DynValue, DynValue>)TargetTeleport;
        lua.Globals["cs_fadeOutBGM"] = (Action<DynValue>)FadeOutBGM;
        lua.Globals["cs_fade"] = (Action<DynValue>)Fade;
        lua.Globals["cs_walk"] = (Action<DynValue, DynValue, DynValue, DynValue>)Walk;
        lua.Globals["cs_enterADV"] = (Action<DynValue>)EnterNVL;
        lua.Globals["cs_exitADV"] = (Action)ExitADV;
        lua.Globals["cs_exit"] = (Action<DynValue>)Exit;
        lua.Globals["cs_enter"] = (Action<DynValue, DynValue, DynValue>)Enter;
    }

    // === LUA CALLABLE ============================================================================


    private void PlayBGM(DynValue bgmKey) {
        Global.Instance.Audio.PlayBGM(bgmKey.String);
    }

    private void PlaySound(DynValue soundKey) {
        Global.Instance.Audio.PlaySFX(soundKey.String);
    }

    private void Teleport(DynValue mapName, DynValue x, DynValue y, DynValue facingLua, DynValue rawLua) {
        OrthoDir? facing = null;
        if (!facingLua.IsNil()) facing = OrthoDirExtensions.Parse(facingLua.String);
        var loc = new Vector2Int((int)x.Number, (int)y.Number);
        var raw = rawLua.IsNil() ? false : rawLua.Boolean;
        RunRoutineFromLua(Global.Instance.Maps.TeleportRoutine(mapName.String, loc, facing, raw));
    }

    private void TargetTeleport(DynValue mapName, DynValue targetEventName, DynValue facingLua, DynValue rawLua) {
        OrthoDir? facing = null;
        if (!facingLua.IsNil()) facing = OrthoDirExtensions.Parse(facingLua.String);
        var raw = rawLua.IsNil() ? false : rawLua.Boolean;
        RunRoutineFromLua(Global.Instance.Maps.TeleportRoutine(mapName.String, targetEventName.String, facing, raw));
    }

    private void FadeOutBGM(DynValue seconds) {
        RunRoutineFromLua(Global.Instance.Audio.FadeOutRoutine((float)seconds.Number));
    }
    
    private void Walk(DynValue eventLua, DynValue steps, DynValue directionLua, DynValue waitLua) {
        if (eventLua.Type == DataType.String) {
            var @event = Global.Instance.Maps.ActiveMap.GetEventNamed(eventLua.String);
            if (@event == null) {
                Debug.LogError("Couldn't find event " + eventLua.String);
            } else {
                var routine = @event.StepMultiRoutine(OrthoDirExtensions.Parse(directionLua.String), (int)steps.Number);
                if (!waitLua.IsNil() && waitLua.Boolean) {
                    RunRoutineFromLua(routine);
                } else {
                    @event.StartCoroutine(routine);
                }
            }
        } else {
            var function = eventLua.Table.Get("walk");
            function.Function.Call(steps, directionLua, waitLua);
        }
    }
    
    private FadeData lastFade;
    private void Fade(DynValue type) {
        var typeString = type.String;
        FadeData fade;
        bool invert = false;
        if (typeString == "normal") {
            fade = lastFade;
            invert = true;
        } else {
            fade = IndexDatabase.Instance.Fades.GetData(typeString);
        }
        lastFade = fade;
        var globals = Global.Instance;
        RunRoutineFromLua(globals.Maps.Camera.fade.FadeRoutine(fade, invert));
    }
    
    public void EnterNVL(DynValue dontClearLua) {
        var dontClear = dontClearLua.IsNil() || dontClearLua.Boolean;
        RunRoutineFromLua(EnterNVLRoutine(dontClear));
    }
    private IEnumerator EnterNVLRoutine(bool dontClear) {
        yield return MapOverlayUI.Instance.adv.ShowRoutine(dontClear);
    }

    public void ExitADV() {
        RunRoutineFromLua(ExitNVLRoutine());
    }
    private IEnumerator ExitNVLRoutine() {
        yield return MapOverlayUI.Instance.adv.HideRoutine();
    }

    public void Enter(DynValue speakerNameLua, DynValue slotLua, DynValue altLua) {
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerNameLua.String);
        var slot = slotLua.String;
        var alt = altLua.IsNotNil() && altLua.Boolean;
        RunRoutineFromLua(EnterRoutine(speaker, slot, alt));
    }
    private IEnumerator EnterRoutine(SpeakerData speaker, string slot, bool alt = false) {
        yield return MapOverlayUI.Instance.adv.EnterRoutine(speaker, slot, alt);
    }

    public void Exit(DynValue speakerNameLua) {
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerNameLua.String);
        RunRoutineFromLua(ExitRoutine(speaker));
    }
    private IEnumerator ExitRoutine(SpeakerData speaker) {
        yield return MapOverlayUI.Instance.adv.ExitRoutine(speaker);
    }

    public void Wipe() {
        MapOverlayUI.Instance.adv.Wipe();
    }

    private void SetName(DynValue speakerLua, DynValue nameLua) {
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerLua.String);
        MapOverlayUI.Instance.adv.speakerNames[speaker] = nameLua.String;
    }
}
