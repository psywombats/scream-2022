using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CharaEvent))]
public class CharaEventEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        CharaEvent chara = (CharaEvent)target;
        
        OrthoDir facing = (OrthoDir)EditorGUILayout.EnumPopup("Facing", chara.Facing);
        if (facing != chara.Facing) {
            chara.Facing = facing;
            chara.UpdateAppearance();
            EditorUtility.SetDirty(target);
        }
        
        chara.UpdateAppearance(fixedTime: true);
    }
}
