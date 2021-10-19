using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugPanel))]
public class DebugPanelEditor : Editor {

    private string customLua;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (Application.IsPlaying(target)) {

            var lua = Global.Instance.Maps.Lua;

            if (!lua.IsRunning()) {
                EditorGUILayout.LabelField("Lua debug prompt!");
            } else {
                EditorGUILayout.LabelField("Running...");
                EditorGUI.BeginDisabledGroup(true);
            }

            customLua = EditorGUILayout.TextArea(customLua, new GUILayoutOption[] { GUILayout.Height(120) });
            GUILayout.Space(12);

            if (lua.IsRunning()) {
                EditorGUILayout.LabelField("Running...");
                EditorGUI.EndDisabledGroup();
            }

            if (!lua.IsRunning()) {
                if (GUILayout.Button("Run")) {
                    LuaScript script = new LuaScript(lua, customLua);
                    Global.Instance.StartCoroutine(script.RunRoutine(true));
                }
            } else {
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Force terminate")) {
                    lua.ForceTerminate();
                }
            }
        }
    }

    public override bool RequiresConstantRepaint() {
        return base.RequiresConstantRepaint() || (Application.IsPlaying(target) && Global.Instance.Maps.Lua.IsRunning());
    }
}
