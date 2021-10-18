using UnityEngine;
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
        lua.Globals["faceToward"] = (Action<DynValue>)FaceToward;
        lua.Globals["cs_search"] = (Action<DynValue>)Search;
        lua.Globals["cs_teleport"] = (Action<DynValue, DynValue, DynValue, DynValue>)TargetTeleport;
        lua.Globals["cs_targetTele"] = (Action<DynValue, DynValue, DynValue, DynValue>)TargetTeleport;
        lua.Globals["cs_fadeOutBGM"] = (Action<DynValue>)FadeOutBGM;
        lua.Globals["cs_fade"] = (Action<DynValue>)Fade;
        lua.Globals["cs_speak"] = (Action<DynValue, DynValue, DynValue>)Speak;
        lua.Globals["cs_speakPortrait"] = (Action<DynValue, DynValue>)SpeakPortrait;
        lua.Globals["cs_intertitle"] = (Action<DynValue>)Intertitle;
        lua.Globals["cs_notebook"] = (Action<DynValue>)Notebook;
        lua.Globals["cs_card"] = (Action<DynValue>)Card;
        lua.Globals["cs_keywords"] = (Action<DynValue>)Keywords;
        lua.Globals["cs_choice"] = (Action<DynValue, DynValue>)Choice;
        lua.Globals["cs_caldeath"] = (Action)Caldeath;
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
        speakerString = UIUtils.GlyphifyString(speakerString);
        textString = UIUtils.GlyphifyString(textString);

        MapEvent @event = null;
        if (targetEventString != null) {
            @event = MapManager.Instance.ActiveMap.GetEventNamed(targetEventString);
            if (@event == null) {
                Debug.LogError("No event named: " + targetEventString);
            }
        } else if (speakerString != null) {
            @event = MapManager.Instance.ActiveMap.GetEventNamed(speakerString);
        }
        if (@event != null) { 
            RunTextboxRoutineFromLua(MapOverlayUI.Instance.Textbox.SpeakRoutine(speakerString, textString, @event.GetTextPos()));
        } else {
            RunTextboxRoutineFromLua(MapOverlayUI.Instance.Textbox.SpeakRoutine(speakerString, textString));
        }

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

    private void Search(DynValue text) {
        RunTextboxRoutineFromLua(MapOverlayUI.Instance.Textbox.SpeakRoutine("SYSTEM", text.String, AvatarEvent.Instance.Event.PositionPx));
    }

    private void Notebook(DynValue text) {
        RunRoutineFromLua(MapOverlayUI.Instance.Notes.NotebookRoutine(text.String));
    }

    private void Keywords(DynValue text) {
        RunRoutineFromLua(MapOverlayUI.Instance.Keywords.ShowRoutine(text.String.Split('\n')));
    }

    private void FaceToward(DynValue eventName) {
        var @event = MapManager.Instance.ActiveMap.GetEventNamed(eventName.String);
        AvatarEvent.Instance.Chara.FaceToward(@event);
    }

    private void Card(DynValue cardName) {
        var cardData = IndexDatabase.Instance.Portraits.GetData(cardName.String);
        RunRoutineFromLua(MapOverlayUI.Instance.Cards.ShowRoutine(cardData));
    }

    private void Choice(DynValue a, DynValue b) {
        RunRoutineFromLua(CoUtils.TaskAsRoutine(ChooseAsync(a.String, b.String)));
    }
    private async Task ChooseAsync(string a, string b) {
        var selection = await MapOverlayUI.Instance.Textbox.ChooseAsync(a, b);
        lua.Globals["choice_result"] = Marshal(selection);
    }

    private void Caldeath() {
        RunRoutineFromLua(CaldeathRoutine());
    }
    private IEnumerator CaldeathRoutine() {
        var pupils = MapOverlayUI.Instance.Pupils;
        pupils.alpha = 0f;
        pupils.gameObject.SetActive(true);
        yield return CoUtils.RunTween(pupils.DOFade(1f, .5f));
        yield return CoUtils.Wait(2f);
    }
}
