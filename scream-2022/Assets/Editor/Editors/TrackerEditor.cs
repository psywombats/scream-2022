using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackerComponent))]
public class TrackerEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var tracker = (TrackerComponent)target;
        if (GUILayout.Button("Go to position")) {
            tracker.ManualUpdate();
        }
        if (GUILayout.Button("Memory position")) {
            tracker.MemorizePosition();
        }
    }
}
