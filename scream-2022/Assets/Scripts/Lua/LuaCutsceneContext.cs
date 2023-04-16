﻿using UnityEngine;
using System.Collections;
using System;
using MoonSharp.Interpreter;
using System.Threading.Tasks;
using DG.Tweening;
using System.Linq;
using FMODUnity;

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

    protected override void RunRoutineFromLuaInternal(IEnumerator routine) {
        if (MapOverlayUI.Instance.Textbox.isDisplaying) {
            MapOverlayUI.Instance.Textbox.MarkHiding();
            base.RunRoutineFromLuaInternal(CoUtils.RunSequence(new IEnumerator[] {
                MapOverlayUI.Instance.Textbox.DisableRoutine(),
                routine,
            }));
        } else {
            base.RunRoutineFromLuaInternal(routine);
        }
    }

    public void RunTextboxRoutineFromLua(IEnumerator routine) {
        base.RunRoutineFromLuaInternal(routine);
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
        lua.Globals["debug"] = (Action<DynValue>)DebugLog;
        lua.Globals["setting"] = (Action<DynValue>)Setting;
        lua.Globals["spawnFollower"] = (Action<DynValue, DynValue>)SpawnFollower;
        lua.Globals["faceTo"] = (Action<DynValue>)FaceToward;
        lua.Globals["faceOther"] = (Action<DynValue, DynValue>)FaceOtherToward;
        lua.Globals["setDepthMult"] = (Action<DynValue>)SetDepthMult;
        lua.Globals["untrackCamera"] = (Action)UntrackCamera;
        lua.Globals["triggerSFX"] = (Action<DynValue>)TriggerSFX;
        lua.Globals["setHeightcrossing"] = (Action<DynValue>)SetHeightcrossing;
        lua.Globals["setSprite"] = (Action<DynValue, DynValue>)SetSprite;
        lua.Globals["alert"] = (Action)Alert;
        lua.Globals["startle"] = (Action)Startle;
        lua.Globals["cs_endgame"] = (Action)EndGame;
        lua.Globals["cs_search"] = (Action<DynValue>)Search;
        lua.Globals["cs_pathTo"] = (Action<DynValue>)PathTo;
        lua.Globals["cs_pathEvent"] = (Action<DynValue, DynValue, DynValue>)PathEvent;
        lua.Globals["cs_teleport"] = (Action<DynValue, DynValue, DynValue, DynValue>)TargetTeleport;
        lua.Globals["cs_fadeOutBGM"] = (Action<DynValue>)FadeOutBGM;
        lua.Globals["cs_fade"] = (Action<DynValue>)Fade;
        lua.Globals["cs_speak"] = (Action<DynValue, DynValue, DynValue>)Speak;
        lua.Globals["cs_speakPortrait"] = (Action<DynValue, DynValue>)SpeakPortrait;
        lua.Globals["cs_intertitle"] = (Action<DynValue>)Intertitle;
        lua.Globals["cs_notebook"] = (Action<DynValue>)Notebook;
        lua.Globals["cs_card"] = (Action<DynValue>)Card;
        lua.Globals["cs_keywords"] = (Action<DynValue>)Keywords;
        lua.Globals["cs_choice"] = (Action<DynValue, DynValue>)Choice;
        lua.Globals["cs_caldeath"] = (Action<DynValue>)Caldeath;
        lua.Globals["cs_walk"] = (Action<DynValue, DynValue, DynValue, DynValue>)Walk;
        lua.Globals["cs_rotateTo"] = (Action<DynValue>)RotateToward;
        lua.Globals["cs_leverLights"] = (Action)LeverLights;
        lua.Globals["cs_resettleCamera"] = (Action)ResettleCamera;
        lua.Globals["cs_clippy"] = (Action<DynValue, DynValue>)Clippy;
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

    private void Speak(DynValue speaker, DynValue text, DynValue targetEventName) {
        var speakerString = speaker.IsNil() ? null : speaker.String;
        var textString = text.IsNil() ? null : text.String;
        var targetEventString = targetEventName.IsNil() ? null : targetEventName.String;
        if (speaker.String.Contains(":")) {
            textString = speakerString.Split(':')[1].Substring(2);
            speakerString = speakerString.Split(':')[0];
        }
        if (textString != null) {
            textString = textString.Replace("TWIN", GetTwinName());
        }

        MapEvent @event = null;
        if (targetEventString != null && !targetEventString.Equals("bottom")) {
            @event = MapManager.Instance.ActiveMap.GetEventNamed(targetEventString);
            if (@event == null) {
                Debug.LogError("No event named: " + targetEventString);
            }
        } else if (speakerString == "Tess") {
            if (AvatarEvent.Instance.UseFirstPersonControl) {
                //targetEventString = "bottom";
            } else {
                @event = AvatarEvent.Instance.Event;
            }
        } else if (speakerString != null) {
            @event = MapManager.Instance.ActiveMap.GetEventNamed(speakerString);
        }
        RunTextboxRoutineFromLua(SpeakRoutine(speakerString, textString, @event, targetEventString != null && targetEventString.Equals("bottom")));
    }
    private IEnumerator SpeakRoutine(string speakerString, string textString, MapEvent @event, bool bottom = false) {
        var isProtag = speakerString == "Tess";
        var isTwin = speakerString == "TWIN";
        if (isTwin) {
            speakerString = GetTwinName();
            if (!Global.Instance.Data.GetSwitch("spoken_lines")) {
                @event.Chara.SetAppearanceByTag("tess_screen");
            }
        }
        if (isProtag) {
            AvatarEvent.Instance.Chara.SetAppearanceByTag("tess_screen");
        }
        if (@event != null) {
            yield return MapOverlayUI.Instance.Textbox.SpeakRoutine(speakerString, textString, @event.GetTextPos(), useTail:!isProtag, bottom: bottom);
        } else {
            yield return MapOverlayUI.Instance.Textbox.SpeakRoutine(speakerString, textString, Vector3.zero, useTail:!isProtag, bottom: bottom);
        }
        if (isProtag) {
            AvatarEvent.Instance.Chara.SetAppearanceByTag("tess");
        }
        if (isTwin) {
            @event.Chara.SetAppearanceByTag("tess");
        }
    }
    private string GetTwinName() {
        return   Global.Instance.Data.GetSwitch("use_cecily")   ? "Cecily"  :
                 Global.Instance.Data.GetSwitch("use_tess")     ? "Tess"    : "???";
    }

    private void SpeakPortrait(DynValue portraitTagValue, DynValue textValue) {
        var portraitTag = portraitTagValue.String;
        var text = textValue.String;
        var portrait = IndexDatabase.Instance.Portraits.GetData(portraitTag);
        RunTextboxRoutineFromLua(MapOverlayUI.Instance.Textbox.SpeakRoutine(" ", text, Vector3.zero, portrait));
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

    private void Intertitle(DynValue linesVal) {
        RunRoutineFromLua(MapOverlayUI.Instance.Intertitle.DisplayRoutine(linesVal.String));
    }

    private void Setting(DynValue textVal) {
        MapOverlayUI.Instance.Setting.Show(textVal.String);
    }

    private void PathTo(DynValue eventNameVal) {
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(eventNameVal.String);
        if (@event == null) {
            RunRoutineFromLua(CoUtils.Wait(0f));
            Debug.LogError("No event: " + eventNameVal.String);
        }
        RunRoutineFromLua(AvatarEvent.Instance.Event.LinearStepRoutine(@event.PositionPx));
    }

    private void PathEvent(DynValue moverVal, DynValue targetVal, DynValue shouldWait) {
        var target = MapManager.Instance.ActiveMap.GetEventNamed(targetVal.String);
        if (targetVal == null) {
            RunRoutineFromLua(CoUtils.Wait(0f));
            Debug.LogError("No event: " + targetVal.String);
        }
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(moverVal.String);
        if (shouldWait.Boolean) {
            RunRoutineFromLua(@event.LinearStepRoutine(target.PositionPx));
        } else {
            Global.Instance.StartCoroutine(@event.LinearStepRoutine(target.PositionPx));
        }
    }

    private void Search(DynValue text) {
        RunTextboxRoutineFromLua(MapOverlayUI.Instance.Textbox.SpeakRoutine("SYSTEM", text.String, 
            AvatarEvent.Instance.UseFirstPersonControl ? Vector3.zero : AvatarEvent.Instance.Event.PositionPx));
    }

    private void Notebook(DynValue text) {
        RunRoutineFromLua(MapOverlayUI.Instance.Notes.NotebookRoutine(text.String));
    }

    private void Clippy(DynValue portrait, DynValue text) {
        RunRoutineFromLua(MapOverlayUI.Instance.clippy.ShowRoutine(portrait.String, text.String));
    }

    private void Keywords(DynValue text) {
        RunRoutineFromLua(MapOverlayUI.Instance.Keywords.ShowRoutine(text.String.Split('\n')));
    }

    private void FaceToward(DynValue eventName) {
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(eventName.String);
        AvatarEvent.Instance.Chara.FaceToward(@event);
    }


    private void RotateToward(DynValue eventName) {
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(eventName.String);
        RunRoutineFromLua(AvatarEvent.Instance.RotateTowardRoutine(@event));
    }

    private void FaceOtherToward(DynValue eventName, DynValue targetName) {
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(eventName.String);
        var target = MapManager.Instance.ActiveMap.GetEventNamed(targetName.String);
        @event.Chara.FaceToward(target);
    }

    private void Card(DynValue cardName) {
        var cardData = IndexDatabase.Instance.Portraits.GetData(cardName.String);
        RunRoutineFromLua(MapOverlayUI.Instance.Cards.ShowRoutine(cardData));
    }

    private void Choice(DynValue a, DynValue b) {
        RunTextboxRoutineFromLua(CoUtils.TaskAsRoutine(ChooseAsync(a.String, b.String)));
    }
    private async Task ChooseAsync(string a, string b) {
        var selection = await MapOverlayUI.Instance.Textbox.ChooseAsync(a, b);
        lua.Globals["choice_result"] = Marshal(selection);
    }

    private void Caldeath(DynValue version) {
        var pupils = MapOverlayUI.Instance.Pupils;
        RunRoutineFromLua(pupils.ShowRoutine((int)version.Number));
    }

    private void Walk(DynValue eventLua, DynValue steps, DynValue directionLua, DynValue waitLua) {
        if (eventLua.Type == DataType.String) {
            var @event = Global.Instance.Maps.ActiveMap.GetEventNamed(eventLua.String);
            if (@event == null) {
                Debug.LogError("Couldn't find walk event " + eventLua.String);
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

    private void SpawnFollower(DynValue follower, DynValue target) {
        var followerName = follower.String;
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(followerName, includeDisabled: true);
        if (!@event.gameObject.activeSelf) {
            var targetEvent = MapManager.Instance.ActiveMap.GetEventNamed(target.String);
            @event.SetLocation(targetEvent.Location);
            @event.Chara.FaceToward(AvatarEvent.Instance.Event);
            @event.gameObject.SetActive(true);
        }
    }

    private void SetDepthMult(DynValue mult) {
        MapManager.Instance.Camera.GetComponent<DepthCamComponent>().RangeMultTarget = (float)mult.Number;
    }

    private void UntrackCamera() {
        MapManager.Instance.Camera.GetComponent<TrackerCam3D>().enabled = false;
        var trailer = UnityEngine.Object.FindObjectOfType<AvatarTrailComponent>();
        if (trailer != null) {
            trailer.enabled = false;
        }
    }

    private void TriggerSFX(DynValue eventName) {
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(eventName.String);
        @event.GetComponent<StudioEventEmitter>().Play();
    }

    private void SetHeightcrossing(DynValue val) {
        AvatarEvent.Instance.DisableHeightCrossing = !val.Boolean;
    }

    private void LeverLights() {
        RunRoutineFromLua(UnityEngine.Object.FindObjectOfType<SleeperLightController>().ShowRoutine());
    }

    private void SetSprite(DynValue targetName, DynValue spriteTag) {
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(targetName.String);
        @event.Chara.SetAppearanceByTag(spriteTag.String);
    }

    private void Alert() {
        Global.Instance.StartCoroutine(UnityEngine.Object.FindObjectOfType<NightAlertController>().AlertRoutine());
    }

    private void ResettleCamera() {
        RunRoutineFromLua(ResettleCameraRoutine());
    }
    private IEnumerator ResettleCameraRoutine() {
        yield return null;
        var oldHero = AvatarEvent.Instance;
        oldHero.gameObject.SetActive(false);
        var twin = MapManager.Instance.ActiveMap.GetEventNamed("d4_twin0", includeDisabled: true);
        twin.gameObject.SetActive(true);
        var @event = MapManager.Instance.ActiveMap.GetEventNamed("backupHero", includeDisabled: true);
        @event.gameObject.SetActive(true);
        Global.Instance.Data.SetSwitch("always_fp", true);
        var backupAva = @event.GetComponent<AvatarEvent>();
        var toCam = backupAva.FPSCam.GetCameraComponent();
        var fromCam = MapManager.Instance.Camera.GetCameraComponent();

        var duration = 2f;
        yield return CoUtils.RunParallel(Global.Instance, new IEnumerator[] {
            CoUtils.RunTween(fromCam.transform.DOMove(toCam.transform.position, duration)),
            CoUtils.RunTween(fromCam.transform.DORotate(toCam.transform.eulerAngles, duration)),
            CoUtils.RunTween(fromCam.DOFieldOfView(toCam.fieldOfView, duration)),
        });

        AvatarEvent.Instance.gameObject.name = "hero (old)";
        MapManager.Instance.Avatar = backupAva;
        backupAva.name = "hero";

        foreach (var ev in MapManager.Instance.ActiveMap.GetEvents<DoorEvent>()) {
            ev.RefreshDoor();
        }
    }

    private void EndGame() {
        RunRoutineFromLua(EndGameRoutine());
    }
    private IEnumerator EndGameRoutine() {
        var eg = MapOverlayUI.Instance.endgamer;
        var sg = MapOverlayUI.Instance.subendgamer;
        yield return CoUtils.RunTween(eg.DOFade(1f, 3f));
        yield return InputManager.Instance.ConfirmRoutine();
        yield return CoUtils.RunTween(sg.DOFade(0f, 3f));
        Application.Quit();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    private void Startle() {
        UnityEngine.Object.FindObjectOfType<OwenPianoEmitter>().Startle();
    }
}
