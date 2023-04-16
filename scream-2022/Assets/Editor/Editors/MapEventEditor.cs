﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEvent), true)]
public class MapEventEditor : Editor {

    private static readonly string DollPath = "Assets/Resources/Prefabs/MapEvent/Doll.prefab";

    public override void OnInspectorGUI() {
        MapEvent mapEvent = (MapEvent)target;

        if (GUI.changed) {
            mapEvent.SetScreenPositionToMatchTilePosition();
            mapEvent.SetDepth();
        }

        if (!mapEvent.GetComponent<CharaEvent>()) {
            if (GUILayout.Button("Add Chara Event")) {
                GameObject dollPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(DollPath);
                var doll = (GameObject)PrefabUtility.InstantiatePrefab(dollPrefab);
                doll.name = mapEvent.name + " (doll)";
                GameObjectUtility.SetParentAndAlign(doll, mapEvent.gameObject);
                CharaEvent chara = mapEvent.gameObject.AddComponent<CharaEvent>();
                chara.Doll = doll.GetComponent<DollComponent>();
                mapEvent.passable = false;
                Undo.RegisterCreatedObjectUndo(mapEvent, "Create " + doll.name);
                Selection.activeObject = doll;

                // hardcode weirdness
                doll.transform.localPosition = new Vector3(Map.UnitsPerTile / 2, 0, 0.25f);
            }
            GUILayout.Space(25.0f);
        }

        Vector2Int newPosition = EditorGUILayout.Vector2IntField("Tiles position", mapEvent.Location);
        if (newPosition != mapEvent.Location) {
            mapEvent.transform.hasChanged = false;
            mapEvent.SetLocation(newPosition);
        }

        base.OnInspectorGUI();
    }

    public void OnSceneGUI() {
        MapEvent mapEvent = (MapEvent)target;
        var handlePos = Handles.PositionHandle(((MapEvent)target).transform.position, Quaternion.identity);
        ((MapEvent)target).transform.position = new Vector3(
            Mathf.RoundToInt(handlePos.x * 2) / 2f,
            handlePos.y,
            Mathf.RoundToInt(handlePos.z * 2) / 2f);
        mapEvent.SetDepth();
    }

    //void OnEnable() { EditorApplication.update += Update; }
    //void OnDisable() { EditorApplication.update -= Update; }

    //void Update() {
    //    MapEvent mapEvent = (MapEvent)target;
    //    if (mapEvent.transform.hasChanged) {
    //        mapEvent.transform.hasChanged = false;
    //        mapEvent.SetDepth();
    //    }
    //}
}
