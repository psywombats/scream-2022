using UnityEngine;

using System.IO;
using MoonSharp.Interpreter;
using System;

[UnityEditor.AssetImporters.ScriptedImporter(1, new string[] { "scene", "lua" })]
public class LuaImporter : UnityEditor.AssetImporters.ScriptedImporter {

    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext context) {
        var script = ScriptableObject.CreateInstance<LuaSerializedScript>();
        var text = File.ReadAllText(context.assetPath);
        script.luaString = text;
        context.AddObjectToAsset("Script", script);
        context.SetMainObject(script);

        var testScript = new Script();
        try {
            testScript.LoadString(text);
        } catch (Exception e) {
            Debug.LogError("Script doesn't compile: " + context.assetPath + "\n\n" + e);
        }
    }
}
