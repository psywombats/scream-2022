using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainSchema), true)]
[CanEditMultipleObjects]
public class MainSchemaEditor : CustomEditorBase {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Reset key")) {
            var schema = (MainSchema)target;
            schema.ResetKey();
        }
    }
}
