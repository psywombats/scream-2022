using UnityEngine;
using System.Collections;
using System;
using MoonSharp.Interpreter;
using UnityEngine.UI;

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
        lua.Globals["cs_enterNVL"] = (Action<DynValue>)EnterNVL;
        lua.Globals["cs_exitNVL"] = (Action)ExitNVL;
        lua.Globals["cs_exit"] = (Action<DynValue>)Exit;
        lua.Globals["cs_enter"] = (Action<DynValue, DynValue, DynValue>)Enter;
        lua.Globals["cs_speak"] = (Action<DynValue, DynValue>)Speak;
        lua.Globals["monitorRoutine"] = (Action<DynValue>)MonitorRoutine;
        lua.Globals["setCorridorBias"] = (Action<DynValue>)SetCorridorBias;
        lua.Globals["setWake"] = (Action<DynValue>)SetWake;
        lua.Globals["cs_rotateTo"] = (Action<DynValue>)RotateToward;
        lua.Globals["cs_expr"] = (Action<DynValue, DynValue>)Express;
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

    public void ExitNVL() {
        RunRoutineFromLua(ExitNVLRoutine());
    }
    private IEnumerator ExitNVLRoutine() {
        yield return MapOverlayUI.Instance.adv.HideRoutine();
    }

    public void Enter(DynValue speakerNameLua, DynValue slotLua, DynValue exprLua) {
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerNameLua.String);
        var slot = slotLua.String;
        var alt = exprLua.String;
        RunRoutineFromLua(EnterRoutine(speaker, slot, alt));
    }
    private IEnumerator EnterRoutine(SpeakerData speaker, string slot, string expr = null) {
        yield return MapOverlayUI.Instance.adv.EnterRoutine(speaker, slot, expr);
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

    public void Speak(DynValue speakerNameLua, DynValue messageLua) {
        var speaker = IndexDatabase.Instance.Speakers.GetData(speakerNameLua.String);
        RunRoutineFromLua(SpeakRoutine(speaker, messageLua.String));
    }
    private IEnumerator SpeakRoutine(SpeakerData speaker, string message) {
        yield return MapOverlayUI.Instance.adv.SpeakRoutine(speaker, message);
    }

    public void MonitorRoutine(DynValue value) {
        var corridor = GameObject.FindObjectOfType<CorridorController>();
        _ = corridor.RunRoutineAsync(value.String);
    }

    public void SetCorridorBias(DynValue value) {
        var corridor = GameObject.FindObjectOfType<CorridorController>();
        corridor.Bias = (int)value.Number;
    }

    public void SetWake(DynValue value) {
        MapOverlayUI.Instance.adv.SetWake((int)value.Number);
    }

    private void RotateToward(DynValue eventName) {
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(eventName.String);
        RunRoutineFromLua(AvatarEvent.Instance.RotateTowardRoutine(@event));
    }

    public void Express(DynValue charaLu, DynValue expr) {
        RunRoutineFromLua(ExpressRoutine(charaLu.String, expr.String));
    }
    private IEnumerator ExpressRoutine(string charaTag, string expr) {
        var adv = MapOverlayUI.Instance.adv;
        var speaker = IndexDatabase.Instance.Speakers.GetData(charaTag);
        return adv.GetPortrait(speaker).ExpressRoutine(expr);
    }
}
