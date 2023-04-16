using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BillboardingSpriteComponent))]
public class BillboardingSpriteEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var sprite = (BillboardingSpriteComponent)target;
        if (GUILayout.Button("Billboard")) {
            sprite.Billboard();
        }
    }
}
