
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpeakboxComponent))]
public class SpeakboxEditor : Editor {

    private Vector3 pos;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var box = (SpeakboxComponent)target;

        if (Application.isPlaying && GUILayout.Button("Test")) {
            box.StartCoroutine(box.TestRoutine());
        }
    }
}
