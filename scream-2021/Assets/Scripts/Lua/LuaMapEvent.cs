using DG.Tweening;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All MapEvents own a lua equivalent that has functions stored on it
/// </summary>
[MoonSharpUserData]
public class LuaMapEvent {

    [MoonSharpHidden] private MapEvent mapEvent;
    [MoonSharpHidden] private Dictionary<string, DynValue> values;

    [MoonSharpHidden]
    public LuaMapEvent(MapEvent mapEvent) {
        this.mapEvent = mapEvent;
        values = new Dictionary<string, DynValue>();
    }

    // meant to be called with the key/value of a lualike property on a Tiled object
    // accepts nil and zero-length as no-ops
    [MoonSharpHidden]
    public void Set(string name, string luaChunk) {
        if (luaChunk != null && luaChunk.Length > 0) {
            var context = Global.Instance.Maps.Lua;
            try {
                values[name] = context.Load(luaChunk);
            } catch (Exception e) {
                Debug.LogError("Bad lua from " + mapEvent.name + ": " + luaChunk + "\n\n" + e);
            }
            
        }
    }

    [MoonSharpHidden]
    public void Run(string eventName, bool canBlock = true) {
        if (values.ContainsKey(eventName) && values[eventName] != null) {
            var context = Global.Instance.Maps.Lua;
            var luaValue = context.Marshal(this);
            context.lua.Globals["this"] = luaValue;
            LuaScript script = new LuaScript(context, values[eventName]);
            Global.Instance.StartCoroutine(script.RunRoutine(canBlock));
        }
    }

    [MoonSharpHidden]
    public DynValue Evaluate(string propertyName) {
        if (!values.ContainsKey(propertyName)) {
            return DynValue.Nil;
        } else {
            var context = Global.Instance.Maps.Lua;
            return context.Evaluate(values[propertyName]);
        }
    }

    [MoonSharpHidden]
    public bool EvaluateBool(string propertyName, bool defaultValue = false) {
        if (!values.ContainsKey(propertyName)) {
            return defaultValue;
        } else {
            DynValue result = Evaluate(propertyName);
            return result.Boolean;
        }
    }

    // === CALLED BY LUA === 

    public void face(string directionName) {
        mapEvent.GetComponent<CharaEvent>().Facing = OrthoDirExtensions.Parse(directionName);
    }

    public void faceToward(LuaMapEvent other) {
        mapEvent.GetComponent<CharaEvent>().Facing = mapEvent.DirectionTo(other.mapEvent);
    }

    public void faceIn() {
        var renderer = mapEvent.GetComponent<CharaEvent>().renderer;
        renderer.color = new Color(renderer.color.r, renderer.color.b, renderer.color.g, 0f);
        renderer.DOFade(1f, 0f);
    }

    public int x() {
        return mapEvent.Location.x;
    }

    public int y() {
        return mapEvent.Location.y;
    }

    public string facing() {
        return mapEvent.GetComponent<CharaEvent>().Facing.DirectionName().ToUpper();
    }

    public void debuglog() {
        Debug.Log("Debug: " + mapEvent.name);
    }

    public void path(int x, int y, bool wait = false) {
        throw new NotImplementedException();
    }

    public void walk(string directionName, int count) {
        throw new NotImplementedException();
    }

    public void wander() {
        throw new NotImplementedException();
    }

    public void randomFace() {
        mapEvent.GetComponent<CharaEvent>().Facing = (OrthoDir)UnityEngine.Random.Range(0, 4);
    }

    public void reverseStep() {
        throw new NotImplementedException();
    }

    public int terrainType() {
        throw new NotImplementedException();
    }

    public void hide() {
        mapEvent.GetComponent<CharaEvent>().OverrideHide = true;
    }

    public void show() {
        mapEvent.GetComponent<CharaEvent>().OverrideHide = false;
    }

    public void cs_step(string directionName) {
        
    }

    public void cs_pathTo(string targetName) {
        var context = Global.Instance.Maps.Lua;
        var target = Global.Instance.Maps.ActiveMap.GetEventNamed(targetName);
        context.RunRoutineFromLua(mapEvent.LinearStepRoutine(target.Location));
    }
}
