﻿using UnityEngine;
using System.Collections;
using System;
using MoonSharp.Interpreter;
using System.Threading.Tasks;
using DG.Tweening;
using System.Linq;

public class LuaCutsceneContext : LuaContext {

    private static readonly string DefinesPath = "Lua/Defines/CutsceneDefines";

    public override IEnumerator RunRoutine(LuaScript script, bool canBlock) {
        if (canBlock && MapManager.Instance.Avatar != null) {
            MapManager.Instance.Avatar.PauseInput();
        }
        var oldInstance = Global.Instance.InstanceID;
        yield return base.RunRoutine(script, canBlock);
        if (Global.Instance.InstanceID != oldInstance) yield break;
        if (MapOverlayUI.Instance.Textbox.isDisplaying && canBlock) {
            yield return MapOverlayUI.Instance.Textbox.DisableRoutine();
        }
        if (canBlock && AvatarEvent.Instance != null) {
            MapManager.Instance.Avatar.UnpauseInput();
        }
    }

    public override void Initialize() {
        base.Initialize();
        LoadDefines(DefinesPath);
    }

    protected override async Task RunRoutineFromLuaInternal(IEnumerator routine) {
        if (MapOverlayUI.Instance.Textbox.isDisplaying) {
            MapOverlayUI.Instance.Textbox.MarkHiding();
            await base.RunRoutineFromLuaInternal(CoUtils.RunSequence(new IEnumerator[] {
                MapOverlayUI.Instance.Textbox.DisableRoutine(),
                routine,
            }));
        } else {
            await base.RunRoutineFromLuaInternal(routine);
        }
    }

    public async void RunTextboxRoutineFromLua(IEnumerator routine) {
        await base.RunRoutineFromLuaInternal(routine);
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
        lua.Globals["face"] = (Action<DynValue, DynValue>)Face;
        lua.Globals["cs_teleport"] = (Action<DynValue, DynValue, DynValue, DynValue>)TargetTeleport;
        lua.Globals["cs_targetTele"] = (Action<DynValue, DynValue, DynValue, DynValue>)TargetTeleport;
        lua.Globals["cs_fadeOutBGM"] = (Action<DynValue>)FadeOutBGM;
        lua.Globals["cs_fade"] = (Action<DynValue>)Fade;
        lua.Globals["cs_speak"] = (Action<DynValue, DynValue>)Speak;
    }

    // === LUA CALLABLE ============================================================================

    private void PlayBGM(DynValue bgmKey) {
        AudioManager.Instance.PlayBGM(bgmKey.String);
    }

    private void PlaySound(DynValue soundKey) {
        AudioManager.Instance.PlaySFX(soundKey.String);
    }

    private void Teleport(DynValue mapName, DynValue x, DynValue y, DynValue facingLua, DynValue rawLua) {
        OrthoDir? facing = null;
        if (!facingLua.IsNil()) facing = OrthoDirExtensions.Parse(facingLua.String);
        var loc = new Vector2Int((int)x.Number, (int)y.Number);
        var raw = rawLua.IsNil() ? false : rawLua.Boolean;
        RunRoutineFromLua(MapManager.Instance.TeleportRoutine(mapName.String, loc, facing, raw));
    }

    private void TargetTeleport(DynValue mapName, DynValue targetEventName, DynValue facingLua, DynValue rawLua) {
        OrthoDir? facing = null;
        if (!facingLua.IsNil()) facing = OrthoDirExtensions.Parse(facingLua.String);
        var raw = rawLua.IsNil() ? false : rawLua.Boolean;
        RunRoutineFromLua(MapManager.Instance.TeleportRoutine(mapName.String, targetEventName.String, facing, raw));
    }

    private void FadeOutBGM(DynValue seconds) {
        RunRoutineFromLua(AudioManager.Instance.FadeOutRoutine((float)seconds.Number));
    }

    private void Speak(DynValue speaker, DynValue text) {
        var speakerString = speaker.IsNil() ? null : speaker.String;
        var textString = text.IsNil() ? null : text.String;
        if (speaker.String.Contains(":")) {
            textString = speakerString.Split(':')[1].Substring(2);
            speakerString = speakerString.Split(':')[0];
        }
        speakerString = UIUtils.GlyphifyString(speakerString);
        textString = UIUtils.GlyphifyString(textString);
        RunTextboxRoutineFromLua(MapOverlayUI.Instance.Textbox.SpeakRoutine(speakerString, textString));
    }

    private void Face(DynValue eventName, DynValue dir) {
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(eventName.String);
        if (@event == null) {
            Debug.LogError("Couldn't find face event " + eventName.String);
        } else {
            @event.GetComponent<CharaEvent>().Facing = OrthoDirExtensions.Parse(dir.String);
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

    //private void Choose() {
    //    RunRoutineFromLua(CoUtils.TaskAsRoutine(ChooseAsync()));
    //}
    //private async Task ChooseAsync() {
    //    var menu = YesNoView.ShowDefault();
    //    var selection = await menu.DoMenuAsync();
    //    lua.Globals["selection"] = Marshal(selection);
    //}
}