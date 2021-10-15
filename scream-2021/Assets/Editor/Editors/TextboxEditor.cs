using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Textbox))]
public class TextboxEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Textbox box = (Textbox)target;

        if (GUILayout.Button("Memorize sizes")) {
            box.MemorizeSizes();
            EditorUtility.SetDirty(box);
        }

        if (Application.isPlaying && GUILayout.Button("Test")) {
            box.StartCoroutine(box.TestRoutine());
        }
    }
}
