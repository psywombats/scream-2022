using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharaEvent))]
public class CharaEventEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var chara = (CharaEvent)target;
    }
}
